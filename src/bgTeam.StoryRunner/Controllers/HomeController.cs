namespace bgTeam.StoryRunner.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("/")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return "Story runner v1.0";
        }
    }
}
