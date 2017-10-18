namespace Trcont.Ris.DataAccess.Common
{
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Entity;

    public class SaveDocumentByEntityIdCmd : ICommand<SaveDocumentByEntityIdCmdContext, bool>
    {
        private readonly IRepositoryEntity _repositoryEn;
        private readonly IRepository _repository;

        public SaveDocumentByEntityIdCmd(IRepositoryEntity repositoryEn, IRepository repository)
        {
            _repositoryEn = repositoryEn;
            _repository = repository;
        }

        public bool Execute(SaveDocumentByEntityIdCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<bool> ExecuteAsync(SaveDocumentByEntityIdCmdContext context)
        {
            context.EntityId.CheckNull(nameof(context.EntityId));
            context.Document.CheckNull(nameof(context.Document));

            var entity = await _repositoryEn.GetAsync<Entities>(x => x.Id == context.EntityId && x.OwnerTypeId == context.EntityType);

            if (entity == null)
            {
                entity = new Entities()
                {
                    Id = context.EntityId,
                    OwnerTypeId = context.EntityType
                };
                await _repository.ExecuteAsync(
                "INSERT INTO Entities (Id, OwnerTypeId) VALUES (@Id, @OwnerTypeId);",
                entity);
            }

            await _repositoryEn.InsertAcync(context.Document);
            
            var doc2En = new Documents2Entity()
            {
                DocumentId = context.Document.Id.Value,
                OwnerId = entity.Id.Value,
                CreateDate = DateTime.Now,
                TimeStamp = DateTime.Now
            };

            await _repositoryEn.InsertAcync(doc2En);
            return true;
        }
    }
}
