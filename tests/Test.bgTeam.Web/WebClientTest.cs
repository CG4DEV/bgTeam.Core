using bgTeam.Web.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Test.bgTeam.Web
{
    [TestClass]
    public class WebClientTest
    {
        [TestMethod]
        public void Test_RequestGet()
        {
            var client = new WebClient("http://getmovie.cc");


            var param = new Dictionary<string, object>();

            param.Add("id", 902939);
            param.Add("token", "037313259a17be837be3bd04a51bf678");

            var res = client.GetAsync<FilmInfo>("api/kinopoisk.json", param).Result;

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void Test_RequestGet2()
        {
            var client = new WebClient("https://rating.kinopoisk.ru/");

            var res = client.GetAsync<string>("961957.xml").Result;

            Assert.IsNotNull(res);
        }

        public class FilmInfo
        {
            public string id { get; set; }
            public string name_ru { get; set; }
            public string name_en { get; set; }
            public string year { get; set; }
            public string country { get; set; }
            public object slogon { get; set; }
            public string genre { get; set; }
            public object budget { get; set; }
            public object fees_usa { get; set; }
            public object fees_world { get; set; }
            public object fees_rus { get; set; }
            public object audience { get; set; }
            public string premier { get; set; }
            public string premier_rus { get; set; }
            public object reliz_dvd { get; set; }
            public object reliz_bluray { get; set; }
            public string description { get; set; }
            public object trivia { get; set; }
            public object trivia_blooper { get; set; }
            public string poster_film_big { get; set; }
            public object poster_film_small { get; set; }
            public object trailer { get; set; }
            public object trailer_duration { get; set; }
            public object rate_pg { get; set; }
            public object age_limit { get; set; }
            public object time_film { get; set; }
            public string studio { get; set; }
            public object comments { get; set; }
        }
    }
}
