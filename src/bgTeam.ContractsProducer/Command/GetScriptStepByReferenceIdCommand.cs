namespace bgTeam.ContractsProducer.Command
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using bgTeam.ContractsProducer.Dto;

    public class GetScriptStepByReferenceIdCommand : ICommand<GetScriptStepByReferenceIdCommandContext, IEnumerable<ScriptStepDto>>
    {
        private readonly IRepository _repository;

        public GetScriptStepByReferenceIdCommand(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ScriptStepDto> Execute(GetScriptStepByReferenceIdCommandContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<ScriptStepDto>> ExecuteAsync(GetScriptStepByReferenceIdCommandContext context)
        {
            var sql =
                @"SELECT d.DSScriptStepId,
                       d.PosInd,
                       d.StepType,
                       d.Caption,
                       dsql.BeginTrans,
                       dsql.CommitTrans,
                       dsql.RollBackTrans,
                       i.Content
                FROM DSScriptStep d
                INNER JOIN DSScriptStepSQL dsql ON d.DSScriptStepId = dsql.DSScriptStepId
                LEFT JOIN Images i ON dsql.ImageId = i.ImageId
                WHERE ReferenceId = @ReferenceId";

            return await _repository.GetAllAsync<ScriptStepDto>(sql, new { ReferenceId = context.ReferenceId });
        }
    }
}
