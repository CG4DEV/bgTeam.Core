namespace bgTeam.Web.Builders
{
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Text;

    public class StringContentBuilder : IContentBuilder
    {
        public HttpContent Build(object param)
        {
            return new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
        }
    }
}
