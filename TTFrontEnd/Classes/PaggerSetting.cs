using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TTBackEnd.Shared;

namespace TTFrontEnd.Classes
{
    public class Pagger<T> : Controller
    {
        public async Task<JsonResult> GetDataFromApi(HttpClient client, PaggerSetting pageSetting, string api)
        {
            int pageSize = pageSetting.length != null ? Convert.ToInt32(pageSetting.length) : 0;
            int skip = pageSetting.start != null ? Convert.ToInt32(pageSetting.start) : 0;
            int pageIndex = skip / pageSize + 1;
            //var  = (api.Contains("?") ? $"&" : $"?" + $"pageSize={pageSize}&pageIndex={pageIndex}");
            api += api.Contains("?") ? $"&" : $"?";
            api += $"pageSize={pageSize}&pageIndex={pageIndex}";
            var apiResult = await Utils.GetApiAsync(client,api);
            var rs = JsonConvert.DeserializeObject<PaggingModel<T>>(apiResult);

            //total number of rows counts
            int recordsTotal = rs.TotalRows;
            //Paging
            var data = rs.ListData;

            //Returning Json Data
            return Json(new { pageSetting.draw, recordsFiltered = recordsTotal, recordsTotal, data });
        }
    }
    /// <summary>
    /// Lấy thông tin từ pagger về
    /// </summary>
    public class PaggerSetting
    {
        /// <summary>
        /// Lần thực hiện
        /// </summary>
        public string draw { get; set; }

        /// <summary>
        /// Vị trí row bắt đầu (Skip number of Rows count)
        /// </summary>
        public string start { get; set; }

        /// <summary>
        /// Số lượng recore ( page size) (Paging Length 10,20...)
        /// </summary>
        public string length { get; set; }
    }
}