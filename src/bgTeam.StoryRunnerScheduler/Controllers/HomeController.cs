namespace bgTeam.StoryRunnerScheduler.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("/")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return "Scheduler story runner v1.0";
        }
    }
}
