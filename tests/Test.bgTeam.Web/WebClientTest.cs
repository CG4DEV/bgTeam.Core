using bgTeam;
using bgTeam.Impl;
using bgTeam.Web.Impl;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Test.bgTeam.Web
{

    public class WebClientTest
    {
        private readonly IAppLogger _logger = new AppLoggerDefault();

        [Fact]
        public void Test_RequestGet()
        {
            var client = new WebClient(_logger, "https://jsonplaceholder.typicode.com/");


           // var header = new Dictionary<string, object>();

            //header.Add("Content-Type", "text/html");
            //param.Add("token", "037313259a17be837be3bd04a51bf678");

            var res = client.GetAsync<ToDos>("todos/1").Result;

            Assert.NotNull(res);
        }

        [Fact]
        public void Test_RequestGet2()
        {
            var client = new WebClient(_logger, "https://rating.kinopoisk.ru/");

            var res = client.GetAsync<string>("961957.xml").Result;

            Assert.NotNull(res);
        }

        public class ToDos
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("userId")]
            public int UserId { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("completed")]
            public bool Completed { get; set; }
        }
    }
}
