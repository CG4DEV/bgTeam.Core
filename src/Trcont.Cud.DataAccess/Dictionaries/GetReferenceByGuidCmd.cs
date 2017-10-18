namespace Trcont.Cud.DataAccess.Dictionaries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using Trcont.Domain;
    using DapperExtensions.Mapper;

    public class GetReferenceByGuidCmd : ICommand<GetReferenceByGuidCmdContext, IEnumerable<Reference>>
    {
        private readonly IRepository _repository;

        public GetReferenceByGuidCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Reference> Execute(GetReferenceByGuidCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<Reference>> ExecuteAsync(GetReferenceByGuidCmdContext context)
        {
            return await _repository.GetAllAsync<Reference>(
                "SELECT * FROM Reference WHERE ReferenceGUID IN (SELECT id FROM @Guids)",
                new { @Guids = new GuidDbType(context.Guids.Where(x => x.HasValue).Distinct().Select(x => x.Value)) });
        }
    }
}
