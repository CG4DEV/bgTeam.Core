namespace Trcont.Ris.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<string> Index()
        {
            return "Ris WebApi .NET Core 2.0";
        }
    }
}
