namespace Trcont.Cud.Story.User
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ChangePasswordUserStory : IStory<ChangePasswordUserStoryContext, StoryResponse<bool>>
    {
        private readonly IRepository _repository;

        public ChangePasswordUserStory(IAppLogger applogger)
        {
            _repository = UserHelper.GetRepository(applogger);
        }

        public StoryResponse<bool> Execute(ChangePasswordUserStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<StoryResponse<bool>> ExecuteAsync(ChangePasswordUserStoryContext context)
        {
            if (context.UserGuid == Guid.Empty)
            {
                return new StoryResponse<bool>("Идентификатор пользователя не может быть пустым");
            }

            if (!(await UserHelper.UserExistAsync(context.UserGuid, _repository)))
            {
                return new StoryResponse<bool>("Пользователь не найден");
            }

            var userHash = await _repository.GetAsync<Proxy>("SELECT u.PasswordHash, u.PasswordSalt FROM Users u WHERE u.UserGUID = @UserGuid", new { UserGuid = context.UserGuid });

            var contextHash = UserHelper.GetHash(context.OldPassword, userHash.PasswordSalt);

            if (userHash.PasswordHash.Equals(contextHash))
            {
                var newSalt = Guid.NewGuid().ToString();
                var newhash = UserHelper.GetHash(context.NewPassword, newSalt);
                await _repository.ExecuteAsync(
                    "UPDATE dbo.Users SET PasswordHash = @Hash ,PasswordSalt = @Salt WHERE UserGUID = @User",
                    new { Hash = newhash, Salt = newSalt, User = context.UserGuid });
                return new StoryResponse<bool>(true);
            }
            else
            {
                return new StoryResponse<bool>("Старый пароль не совпадает");
            }
        }

        private class Proxy
        {
            public string PasswordHash { get; set; }

            public string PasswordSalt { get; set; }
        }
    }
}
