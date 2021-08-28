using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Shared.Models.Files
{
    public class UploadFilesResponse : Response
    {
        public List<string> Urls { get; set; }
    }
}
