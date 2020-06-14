using System.Collections.Generic;

namespace TTBackEnd.Shared
{
    public class RegisterResult
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Error { get; set; }
    }
}
