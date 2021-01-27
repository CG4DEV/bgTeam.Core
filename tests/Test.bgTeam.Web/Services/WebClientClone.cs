using System.Net.Http;
using bgTeam.Web;
using bgTeam.Web.Impl;

namespace Test.bgTeam.Web.Services
{
    public interface IWebClientClone : IWebClient
    {
    }

    public class WebClientClone : WebClient, IWebClientClone
    {
        public WebClientClone(HttpClient client) : base(client)
        {
        }
    }
}
