namespace Trcont.Booking.DataAccess.Dictionaries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using EntityDataBaseRsi;

    public class GetImagesCommand : ICommand<GetImagesCommandContext, IEnumerable<Images>>
    {
        private readonly IRepositoryEntity _repository;

        public GetImagesCommand(IRepositoryEntity repository)
        {
            _repository = repository;
        }

        public IEnumerable<Images> Execute(GetImagesCommandContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<Images>> ExecuteAsync(GetImagesCommandContext context)
        {
            return await _repository.GetAllAsync<Images>(x => x.ImageId == context.Id);
        }
    }
}
