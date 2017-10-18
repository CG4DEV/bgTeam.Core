namespace Trcont.Cud.Story.User
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Cud.Domain.Dto;

    public class UpdateUserStory : IStory<UpdateUserStoryContext, StoryResponse<UserDto>>
    {
        private readonly IAppLogger _appLogger;
        private readonly IRepository _repository;

        public UpdateUserStory(IAppLogger logger)
        {
            _appLogger = logger;
            _repository = UserHelper.GetRepository(logger);
        }

        public StoryResponse<UserDto> Execute(UpdateUserStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<StoryResponse<UserDto>> ExecuteAsync(UpdateUserStoryContext context)
        {
            if (context.UserGuid == Guid.Empty)
            {
                return new StoryResponse<UserDto>("Идентификатор пользователя не может быть пустым");
            }

            if (!(await UserHelper.UserExistAsync(context.UserGuid, _repository)))
            {
                return new StoryResponse<UserDto>("Пользователь не найден");
            }

            if (!string.IsNullOrWhiteSpace(context.Email))
            {
                if (!UserHelper.EmailIsValid(context.Email))
                {
                    return new StoryResponse<UserDto>("Неверный формат электронной почты");
                }

                if (await UserHelper.EmailExistAsync(context.Email, _repository))
                {
                    return new StoryResponse<UserDto>("Электронная почта уже используется");
                }
            }

            await UpdateUserAsync(context);

            var data = await UserHelper.GetUserAsync(context.UserGuid, _repository);

            return new StoryResponse<UserDto>(data);
        }

        private async Task UpdateUserAsync(UpdateUserStoryContext context)
        {
            List<string> fields = new List<string>();
            var sql = new StringBuilder(@"
                UPDATE dbo.Users 
                SET");
            sql.AppendLine();

            if (context.FirmGuid != Guid.Empty)
            {
                fields.Add("                  FirmGUID = @FirmGuid");
            }

            if (context.Email != null)
            {
                fields.Add("                 E_Mail = @Email");
            }

            if (context.FirstName != null)
            {
                fields.Add("                 FirstName = @FirstName");
            }

            if (context.MiddleName != null)
            {
                fields.Add("                 MiddleName = @MiddleName");
            }

            if (context.LastName != null)
            {
                fields.Add("                 LastName = @LastName");
            }

            if (context.PhoneNumber != null)
            {
                fields.Add("                 PhoneNumber = @PhoneNumber");
            }

            if (context.ClientGuid.HasValue)
            {
                fields.Add("                 ClientGUID = @ClientGuid");
            }

            if (context.ContractGuid.HasValue)
            {
                fields.Add("                 ContractGUID = @ContractGuid");
            }

            if (context.DisableTwoFactorAuth.HasValue)
            {
                fields.Add("                 DisableTwoFactorAuth = @DisableTwoFactorAuth");
            }

            if (context.TwoFactorAuthTokenKey != null)
            {
                fields.Add("                 TwoFactorAuthTokenKey = @TwoFactorAuthTokenKey");
            }

            fields.Add("                 AccsessDate = GETDATE()");

            sql.AppendLine(string.Join(",\r\n", fields));

            sql.AppendLine("                WHERE UserGUID = @UserGuid;");

            await _repository.ExecuteAsync(sql.ToString(), context);
        }
    }
}
