namespace Tastee.Shared
{
    public class LoginResult
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
    }
}
