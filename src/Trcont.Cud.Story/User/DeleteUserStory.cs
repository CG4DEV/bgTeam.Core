namespace Trcont.Cud.Story.User
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System;
    using System.Threading.Tasks;

    public class DeleteUserStory : IStory<DeleteUserStoryContext, StoryResponse<bool>>
    {
        private readonly IAppLogger _appLogger;
        private readonly IRepository _repository;

        public DeleteUserStory(IAppLogger logger)
        {
            _appLogger = logger;
            _repository = UserHelper.GetRepository(logger);
        }

        public StoryResponse<bool> Execute(DeleteUserStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<StoryResponse<bool>> ExecuteAsync(DeleteUserStoryContext context)
        {
            if (context.UserGuid == Guid.Empty)
            {
                return new StoryResponse<bool>("Идентификатор пользователя не может быть пустым");
            }

            if (!(await UserHelper.UserExistAsync(context.UserGuid, _repository)))
            {
                return new StoryResponse<bool>("Пользователь не найден");
            }

            string sql = @"DELETE FROM dbo.Users WHERE UserGUID = @UserGuid;";

            var result = Convert.ToBoolean(await _repository.ExecuteAsync(sql, context));

            return new StoryResponse<bool>(result);
        }
    }
}
