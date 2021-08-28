using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.Files
{
    public class UploadTmpFolderResponse
    {
        public Dictionary<int, string> ImgDictionary { get; set; }
        public string FolderPath;
    }
}
