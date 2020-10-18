using Microsoft.AspNetCore.Http;
using System;
using Tastee.Application.Interfaces;

namespace Tastee.Application.Services
{
    public class FileService : IFileService
    {
        public string[] Upload(IFormFileCollection files)
        {
            return new string[] { "xyz.abc", "kfc.lsk" };
            //throw new NotImplementedException();
        }
    }
}
