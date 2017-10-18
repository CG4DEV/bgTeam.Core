namespace Trcont.Cud.Story.User
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Cud.Domain.Dto;

    public class CreateUserStory : IStory<CreateUserStoryContext, StoryResponse<UserDto>>
    {
        private readonly IAppLogger _appLogger;
        private readonly IRepository _repository;

        public CreateUserStory(IAppLogger logger)
        {
            _appLogger = logger;
            _repository = UserHelper.GetRepository(logger);
        }

        public StoryResponse<UserDto> Execute(CreateUserStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<StoryResponse<UserDto>> ExecuteAsync(CreateUserStoryContext context)
        {
            var resp = CheckContext(context);
            if (!resp.Success)
            {
                return resp;
            }

            if (await LoginExistAsync(context))
            {
                return new StoryResponse<UserDto>("Логин уже используется");
            }

            if (!context.ContractGuid.HasValue)
            {
                return new StoryResponse<UserDto>("Не указан контракт пользователя");
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

            return await CreateUserAsync(context);
        }

        private async Task<StoryResponse<UserDto>> CreateUserAsync(CreateUserStoryContext context)
        {
            string createSql = @"
                INSERT INTO dbo.Users
                (
                  UserGUID ,FirmGUID ,Login ,PasswordHash ,PasswordSalt ,FirstName ,MiddleName ,LastName ,E_Mail, AccessDate, ContractGUID, ClientGUID, PhoneNumber
                )
                VALUES
                (
                  @UserGuid, @FirmGuid, @Login, @Password, @Salt, @FirstName, @MiddleName, @LastName, @Email, GETDATE(), @ContractGuid, @ClientGuid, @PhoneNumber
                );";

            string salt = Guid.NewGuid().ToString();
            string hash = UserHelper.GetHash(context.Password, salt);

            var prms = new
            {
                UserGuid = Guid.NewGuid(),
                FirmGuid = context.FirmGuid,
                Login = context.Login,
                Password = hash,
                Salt = salt,
                Email = context.Email,
                FirstName = context.FirstName,
                MiddleName = context.MiddleName,
                LastName = context.LastName,
                ContractGuid = context.ContractGuid,
                ClientGuid = context.ClientGuid,
                PhoneNumber = context.PhoneNumber
            };

            await _repository.ExecuteAsync(createSql, prms);

            var data = await UserHelper.GetUserAsync(prms.UserGuid, _repository);
            return new StoryResponse<UserDto>(data);
        }

        private async Task<bool> LoginExistAsync(CreateUserStoryContext context)
        {
            string sql = @"SELECT COUNT(*) FROM Users WHERE Login = @Login";
            int cnt = await _repository.GetAsync<int>(sql, new { Login = context.Login });
            return Convert.ToBoolean(cnt);
        }

        private StoryResponse<UserDto> CheckContext(CreateUserStoryContext context)
        {
            Type type = context.GetType();
            PropertyInfo[] infos = type.GetProperties()
                .Where(x => !x.CustomAttributes.Any(y => y.AttributeType.Equals(typeof(System.ComponentModel.DefaultValueAttribute))))
                .ToArray();

            foreach (var prop in infos)
            {
                if (prop.GetValue(context) == null)
                {
                    return new StoryResponse<UserDto>($"Поле не может быть пустым: {prop.Name}");
                }
            }

            return new StoryResponse<UserDto>();
        }
    }
}
