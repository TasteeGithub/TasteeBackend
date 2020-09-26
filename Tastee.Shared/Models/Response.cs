using System.Collections.Generic;

namespace Tastee.Shared
{
    public class Response
    {
        public bool Successful { get; set; }
        public string Message { get; set; }
        public int ErrorCode { get; set; }
    }
}
