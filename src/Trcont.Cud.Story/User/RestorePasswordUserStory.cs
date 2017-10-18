namespace Trcont.Cud.Story.User
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RestorePasswordUserStory : IStory<RestorePasswordUserStoryContext, StoryResponse<bool>>
    {
        private readonly IRepository _repository;

        public RestorePasswordUserStory(IAppLogger applogger)
        {
            _repository = UserHelper.GetRepository(applogger);
        }

        public StoryResponse<bool> Execute(RestorePasswordUserStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<StoryResponse<bool>> ExecuteAsync(RestorePasswordUserStoryContext context)
        {
            if (context.UserGuid == Guid.Empty)
            {
                return new StoryResponse<bool>("Идентификатор пользователя не может быть пустым");
            }

            if (!(await UserHelper.UserExistAsync(context.UserGuid, _repository)))
            {
                return new StoryResponse<bool>("Пользователь не найден");
            }

            var newSalt = Guid.NewGuid().ToString();
            var newhash = UserHelper.GetHash(context.NewPassword, newSalt);
            int res = await _repository.ExecuteAsync(
                "UPDATE dbo.Users SET PasswordHash = @Hash ,PasswordSalt = @Salt WHERE UserGUID = @User",
                new { Hash = newhash, Salt = newSalt, User = context.UserGuid });

            return new StoryResponse<bool>(Convert.ToBoolean(res));
        }
    }
}
