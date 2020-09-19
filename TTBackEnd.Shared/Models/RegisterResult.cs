using System.Collections.Generic;

namespace Tastee.Shared
{
    public class RegisterResult
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Error { get; set; }
    }
}
