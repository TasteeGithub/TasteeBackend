using System;
using System.Collections.Generic;
using System.Text;

namespace Tastee.Domain.Models
{
    public class AWSUploadResult<T> where T : class
    {
        public bool Status { get; set; } = false;
        public int StatusCode { get; set; } = 200;
        public T Data { get; set; }
    }
}
