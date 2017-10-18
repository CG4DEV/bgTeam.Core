namespace Trcont.Cud.Story.User
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System;
    using System.Net.Mail;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Cud.DataAccess;
    using Trcont.Cud.Domain.Dto;
    using bgTeam.Infrastructure.DataAccess;

    public static class UserHelper
    {
        public static IRepository GetRepository(IAppLogger logger)
        {
            return new RepositoryDapper(GetConnection(logger));
        }

        public static IConnectionFactory GetConnection(IAppLogger logger)
        {
            return new ConnectionFactoryMsSql(logger, GetConnSettings());
        }

        public static IConnectionSetting GetConnSettings()
        {
            return new UserDBConnection();
        }

        public static string GetHash(string password, string salt)
        {
            string pw = string.Format("{0}{1}", password, salt);
            byte[] bytes = Encoding.Unicode.GetBytes(pw);
            StringBuilder stringBuilder = new StringBuilder();

            using (SHA1 sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] hash = sha1.ComputeHash(bytes, 0, bytes.Length);
                for (int i = 0; i < hash.Length; i++)
                {
                    stringBuilder.Append(hash[i].ToString("x2"));
                }
            }

            return stringBuilder.ToString();
        }

        public static bool EmailIsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static async Task<bool> EmailExistAsync(string email, IRepository repository)
        {
            string sql = @"SELECT COUNT(*) FROM Users WHERE E_Mail = @Email";
            int cnt = await repository.GetAsync<int>(sql, new { Email = email });
            return Convert.ToBoolean(cnt);
        }

        public static async Task<UserDto> GetUserAsync(Guid userGuid, IRepository repository)
        {
            string getSql = @"
                SELECT u.UserGUID,
                  u.FirmGuid,
                  u.Login,
                  u.FirstName,
                  u.MiddleName,
                  u.LastName,
                  u.E_Mail AS Email,
                  u.PhoneNumber,
                  u.ClientGUID AS ClientGuid,
                  u.ContractGUID AS ContractGuid,
                  u.DisableTwoFactorAuth,
                  u.TwoFactorAuthTokenKey
                FROM Users u
                WHERE u.UserGuid = @UserGuid";
            return await repository.GetAsync<UserDto>(getSql, new { UserGuid = userGuid });
        }

        public static async Task<bool> UserExistAsync(Guid userGuid, IRepository repository)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE UserGUID = @UserGuid";
            int cnt = await repository.GetAsync<int>(sql, new { UserGuid = userGuid });
            return Convert.ToBoolean(cnt);
        }
    }
}
