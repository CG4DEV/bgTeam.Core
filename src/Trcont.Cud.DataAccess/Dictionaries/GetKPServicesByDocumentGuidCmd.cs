namespace Trcont.Cud.DataAccess.Dictionaries
{
    using bgTeam.DataAccess;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Trcont.Domain.Entity;

    public class GetKPServicesByDocumentGuidCmd : ICommand<GetKPServicesByDocumentGuidCmdContext, IEnumerable<KpServices>>
    {
        private readonly IRepository _repository;

        public GetKPServicesByDocumentGuidCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<KpServices> Execute(GetKPServicesByDocumentGuidCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<KpServices>> ExecuteAsync(GetKPServicesByDocumentGuidCmdContext context)
        {
            return await _repository.GetAllAsync<KpServices>(
               @"SELECT
                  ks.*
                FROM KPServices ks
                WHERE RefDocumentGUID = @DocumentGuid AND IsActive = 1
                ORDER BY ks.ArmIndex ASC",
                new { DocumentGuid = context.DocumentGuid });
        }
    }
}
