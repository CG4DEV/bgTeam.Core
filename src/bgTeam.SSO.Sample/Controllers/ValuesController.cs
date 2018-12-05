namespace bgTeam.SSO.Sample.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [Authorize]
        [HttpGet]
        public ActionResult<string> Get()
        {
            return $"Authenticated as {HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value}";
        }
    }
}
