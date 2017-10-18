namespace Trcont.Cud.DataAccess.Dictionaries
{
    using bgTeam.DataAccess;
    using DapperExtensions.Mapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.Cud.Domain.Dto;

    public class GetPointTypesCmd : ICommand<GetPointTypesCmdContext, IEnumerable<PointTypeDto>>
    {
        private readonly IRepository _repository;

        public GetPointTypesCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<PointTypeDto> Execute(GetPointTypesCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<PointTypeDto>> ExecuteAsync(GetPointTypesCmdContext context)
        {
            var placeGuids = context.PlaceGuids.Where(x => x != Guid.Empty);
            if (!placeGuids.Any())
            {
                return Enumerable.Empty<PointTypeDto>();
            }

            var sql = @"SELECT
                          p.IRSGuid AS PlaceGuid,
                          p.PointType
                        FROM Point p
                        WHERE p.IRSGuid IN (SELECT Id FROM @PlaceGuids)";
            return await _repository.GetAllAsync<PointTypeDto>(sql, new { PlaceGuids = new GuidDbType(placeGuids) });
        }
    }
}
