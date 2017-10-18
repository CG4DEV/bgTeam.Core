using bgTeam;
using bgTeam.DataAccess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trcont.Cud.Domain.Dto;

namespace Trcont.Cud.Story.User
{
    public class GetUsersStory : IStory<GetUsersStoryContext, StoryResponse<IEnumerable<FullUserDto>>>
    {
        private readonly IRepository _repository;
        private readonly IAppLogger _logger;

        public GetUsersStory(IAppLogger logger)
        {
            _logger = logger;
            _repository = UserHelper.GetRepository(logger);
        }

        public StoryResponse<IEnumerable<FullUserDto>> Execute(GetUsersStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<StoryResponse<IEnumerable<FullUserDto>>> ExecuteAsync(GetUsersStoryContext context)
        {
            var sql = @"
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
                  u.TwoFactorAuthTokenKey,
                  u.PasswordHash,
                  u.PasswordSalt,
                  u.Active,
                  u.IsClient,
                  u.IsExtAgent,
                  u.EmailVerified,
                  u.PhoneVerified
                FROM Users u
                WHERE u.AccessDate >= @AccessDate";

            var data = await _repository.GetAllAsync<FullUserDto>(sql, new { AccessDate = context.AccessDate.Date } );

            return new StoryResponse<IEnumerable<FullUserDto>>(data);
        }
    }
}
