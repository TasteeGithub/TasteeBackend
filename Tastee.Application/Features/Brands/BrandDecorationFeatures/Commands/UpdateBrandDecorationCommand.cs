using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tastee.Application.Interfaces;
using Tastee.Application.Utilities;
using Tastee.Application.Wrappers;
using Tastee.Infrastucture.Data.Context;
using Tastee.Shared;
using Tastee.Shared.Models.Brands.BrandDecorations;

namespace Tastee.Application.Features.Brands.BrandDecorationFeatures.Commands
{
    public class UpdateBrandDecorationCommand : IRequest<Response>
    {
        public UpdateBrandDecorationModel Model { get; set; }
        public string UserEmail { get; set; }
        public class UpdateBrandDecorationCommandHandler : IRequestHandler<UpdateBrandDecorationCommand, Response>
        {
            private readonly IFileService _fileService;
            private readonly IBrandService _brandService;
            private readonly IConfiguration _configuration;
            public UpdateBrandDecorationCommandHandler(IFileService fileService, IBrandService brandService, IConfiguration configuration)
            {
                _fileService = fileService;
                _brandService = brandService;
                _configuration = configuration;
            }
            public async Task<Response> Handle(UpdateBrandDecorationCommand request, CancellationToken cancellationToken)
            {
                var updateModel = request.Model;
                var decoration = _brandService.GetBrandDecorationByBrandId(updateModel.BrandID);
                if (decoration == null)
                    return new Response() { Successful = false, Message = "Decoration not found" };

                if (!updateModel.Widgets.TryParseJson(out WidgetsModel uWidgetModel))
                {
                    return new Response() { Successful = false, Message = "Decoration not valid" };
                }

                WidgetsModel cWidgetModel = _brandService.BuildBrandDecoration(decoration);
                if (updateModel.Files != null && updateModel.Files.Count != 0)
                {
                    var rs = _fileService.UploadTmpFolder(updateModel.Files.Select(x => x.File).ToList());
                    var key_perfix = _fileService.GenerateS3KeyPrefix(decoration.Id, UploadFileType.Image, ObjectType.Decoration);
                    var uploadResult = await _fileService.UploadFolderToS3BucketAsync(rs.FolderPath, key_perfix);
                    if (!String.IsNullOrEmpty(uploadResult))
                    {
                        return new Response() { Successful = false, Message = "Update Image Failed" };
                    }
                    _fileService.DeleteFolder(rs.FolderPath);

                    var listWidgetImages = new List<WidgetImages>();
                    var imgDict = new Dictionary<string, string>();
                    var listKeys = rs.ImgDictionary.Keys.ToList();
                    for (int i = 0; i < listKeys.Count(); i++)
                    {
                        var key = listKeys[i];
                        string bucketName = _configuration["AWS:BucketName"];
                        var url = _fileService.GenerateAwsFileUrl(bucketName, String.Format("{0}/{1}", key_perfix, rs.ImgDictionary[key])).Data;
                        imgDict.Add(updateModel.Files[i].Name, url);
                    }
                    ReplaceImage(ref uWidgetModel, cWidgetModel, imgDict);
                }
                decoration.UpdatedBy = request.UserEmail;
                return await _brandService.UpdateBrandDecorationAsync(decoration, uWidgetModel); ;
            }

            private void ReplaceImage(ref WidgetsModel uWidgetModel, WidgetsModel cWidgetModel, Dictionary<string, string> imgDict)
            {
                //var currentImages = new List<string>();

                // InfoWidget
                //currentImages.Add(cWidgetModel.InfoWidget.BrandImage);
                if (imgDict.Keys.Contains(uWidgetModel.InfoWidget.BrandImage))
                {
                    uWidgetModel.InfoWidget.BrandImage = imgDict[uWidgetModel.InfoWidget.BrandImage];
                }

                //SingleBanner
                //foreach (var widget in cWidgetModel.SingelBannerWidget)
                //{
                //    currentImages.Add(widget.Image);

                //}
                if (uWidgetModel.SingelBannerWidget != null)
                    foreach (var widget in uWidgetModel.SingelBannerWidget)
                    {
                        if (imgDict.Keys.Contains(widget.Image))
                        {
                            widget.Image = imgDict[widget.Image];
                        }
                    }

                //SilderBanner
                //foreach (var widget in cWidgetModel.SliderBannerWidget)
                //{
                //    currentImages.AddRange(widget.Images);

                //}
                if (uWidgetModel.SliderBannerWidget != null)
                    foreach (var widget in uWidgetModel.SliderBannerWidget)
                    {
                        var newImgs = new List<string>();
                        foreach (var image in widget.Images)
                        {
                            if (imgDict.Keys.Contains(image))
                                newImgs.Add(imgDict[image]);
                            else
                                newImgs.Add(image);
                        }
                        widget.Images = newImgs;
                    }

                //return currentImages;
            }
        }
    }
}
