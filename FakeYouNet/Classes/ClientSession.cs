using System.Net;

namespace FakeYouNet
{
    public class ClientSession
    {
        public Cookie AuthCookie { get; set; }
        public string AuthToken { get; set; }
    }
}