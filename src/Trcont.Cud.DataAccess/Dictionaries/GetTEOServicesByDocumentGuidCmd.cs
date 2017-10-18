namespace Trcont.Cud.DataAccess.Dictionaries
{
    using bgTeam.DataAccess;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Trcont.Domain.Irs;

    public class GetTEOServicesByDocumentGuidCmd : ICommand<GetTEOServicesByDocumentGuidCmdContext, IEnumerable<TeoServices>>
    {
        private readonly IRepository _repository;

        public GetTEOServicesByDocumentGuidCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<TeoServices> Execute(GetTEOServicesByDocumentGuidCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<TeoServices>> ExecuteAsync(GetTEOServicesByDocumentGuidCmdContext context)
        {
            return await _repository.GetAllAsync<TeoServices>(
               @"SELECT 
                  ts.*
                FROM TEOServices ts
                WHERE RefDocumentGUID = @DocumentGuid
                ORDER BY ts.ArmIndex ASC",
                new { DocumentGuid = context.DocumentGuid });
        }
    }
}
