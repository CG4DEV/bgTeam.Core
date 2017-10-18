namespace Trcont.Cud.DataAccess.ETSNG
{
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using System;
    using System.Threading.Tasks;

    public class GetGNGByETSNGCommand : ICommand<GetGNGByETSNGContext, string>
    {
        private readonly IRepository _repository;

        public GetGNGByETSNGCommand(IRepository repository)
        {
            _repository = repository;
        }

        public string Execute(GetGNGByETSNGContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<string> ExecuteAsync(GetGNGByETSNGContext context)
        {
            context.ETSNGCode.CheckNull(nameof(context.ETSNGCode));
                return await _repository.GetAsync<string>(
                    @"SELECT TOP 1 rs.CodeFull
                    FROM RefGNG rs
                        JOIN Reference r ON r.ReferenceGUID=rs.ReferenceGUID
                        JOIN RefGNG2ETSNGPos rp on rs.ReferenceGUID = rp.RefGNGGUID
                        JOIN RefETSNG re on rp.RefETSNGPosGUID = re.ETSNGPosGUID
                    WHERE re.code= @ETSNGCode AND LEN(rs.CodeFull)=8
                    AND (R.ActualBeginDate IS NULL OR R.ActualBeginDate < GETDATE())
                    AND (R.ActualEndDate IS NULL OR R.ActualEndDate > GETDATE())",
                    new { ETSNGCode = context.ETSNGCode });
        }
    }
}
