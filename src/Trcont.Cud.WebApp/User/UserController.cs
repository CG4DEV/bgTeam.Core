namespace Trcont.Cud.WebApp.User
{
    using bgTeam;
    using System;
    using System.Threading.Tasks;
    using Trcont.Cud.Domain.Dto;
    using Trcont.Cud.Story;
    using Trcont.Cud.Story.User;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IAppLogger _appLogger;
        private readonly IStoryBuilder _storyBuilder;

        public UserController(IAppLogger appLogger, IStoryBuilder storyBuilder)
        {
            _appLogger = appLogger;
            _storyBuilder = storyBuilder;
        }

        [HttpPost]
        public async Task<StoryResponse<UserDto>> Create(CreateUserStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<StoryResponse<UserDto>>();
        }

        [HttpPost]
        public async Task<StoryResponse<UserDto>> Update(UpdateUserStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<StoryResponse<UserDto>>();
        }

        [HttpPost]
        public async Task<StoryResponse<bool>> Delete(DeleteUserStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<StoryResponse<bool>>();
        }

        [HttpPost]
        public async Task<StoryResponse<bool>> ChangePassword(ChangePasswordUserStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<StoryResponse<bool>>();
        }

        [HttpPost]
        public async Task<StoryResponse<bool>> RestorePassword(RestorePasswordUserStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<StoryResponse<bool>>();
        }

        [HttpPost]
        public async Task<StoryResponse<IEnumerable<FullUserDto>>> GetUsers(GetUsersStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<StoryResponse<IEnumerable<FullUserDto>>>();
        }
    }
}
