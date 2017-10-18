namespace Trcont.Cud.WebApp.Home
{
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public string Index()
        {
            return "Cud ASP.NET Core WebApi v2.0";
        }
    }
}
