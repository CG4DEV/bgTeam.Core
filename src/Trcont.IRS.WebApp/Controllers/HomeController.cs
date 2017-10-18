namespace Trcont.IRS.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public string Index()
        {
            return "IRS ASP.NET Core WebApi v.2";
        }
    }
}
