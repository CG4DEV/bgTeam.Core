namespace Trcont.Cud.WebApp.Common
{
    using bgTeam;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Trcont.Cud.Domain.Dto;
    using Trcont.Cud.Story.Common;

    [Route("[controller]/[action]")]
    public class CommonController : Controller
    {
        private readonly IAppLogger _appLogger;
        private readonly IStoryBuilder _storyBuilder;

        public CommonController(IAppLogger appLogger, IStoryBuilder storyBuilder)
        {
            _appLogger = appLogger;
            _storyBuilder = storyBuilder;
        }

        [HttpPost]
        public async Task<ClientDto> GetContrAgentByGuid(GetContrAgentByGuidStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<ClientDto>();
        }
    }
}
