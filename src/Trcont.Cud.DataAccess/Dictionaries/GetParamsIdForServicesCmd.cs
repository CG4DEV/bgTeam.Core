namespace Trcont.Cud.DataAccess.Dictionaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using DapperExtensions.Mapper;
    using Trcont.Cud.Domain.Dto;

    public class GetParamsIdForServicesCmd : ICommand<GetParamsIdForServicesCmdContext, IEnumerable<ServiceParameterDto>>
    {
        private readonly IRepository _repository;

        public GetParamsIdForServicesCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ServiceParameterDto> Execute(GetParamsIdForServicesCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<ServiceParameterDto>> ExecuteAsync(GetParamsIdForServicesCmdContext context)
        {
            var sql = @"SELECT * FROM ServiceType2ParameterTypes WHERE ServiceGUID IN (SELECT id FROM @ReferenceIds)";

            return await _repository.GetAllAsync<ServiceParameterDto>(sql, new { ReferenceIds = new GuidDbType(context.ServiceGuids) });
        }
    }
}
