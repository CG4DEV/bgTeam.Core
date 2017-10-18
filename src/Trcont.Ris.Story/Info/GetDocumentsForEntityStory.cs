namespace Trcont.Ris.Story.Info
{
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Domain.Enums;

    public class GetDocumentsForEntityStory : IStory<GetDocumentsForEntityStoryContext, IEnumerable<DocumentsDto>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        private static Dictionary<DocumentsOwnerTypeEnum, IEnumerable<PageDocTypeEnum>> _validPagesByOwner = new Dictionary<DocumentsOwnerTypeEnum, IEnumerable<PageDocTypeEnum>>
        {
            { DocumentsOwnerTypeEnum.Orders, new[] { PageDocTypeEnum.Instractions, PageDocTypeEnum.Documents, PageDocTypeEnum.Bills } },
            { DocumentsOwnerTypeEnum.Contracts, new[] { PageDocTypeEnum.Documents, PageDocTypeEnum.Bills, PageDocTypeEnum.Additional } }
        };

        public GetDocumentsForEntityStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IEnumerable<DocumentsDto> Execute(GetDocumentsForEntityStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<DocumentsDto>> ExecuteAsync(GetDocumentsForEntityStoryContext context)
        {
            if (!context.OwnerId.HasValue)
            {
                throw new ArgumentNullException(nameof(context.OwnerId));
            }

            var otype = (DocumentsOwnerTypeEnum)context.OwnerTypeId;
            if (otype == DocumentsOwnerTypeEnum.None)
            {
                throw new ArgumentNullException(nameof(context.OwnerTypeId));
            }

            if (_validPagesByOwner[otype].All(x => x != context.Page && context.Page != PageDocTypeEnum.None))
            {
                throw new ArgumentException($"В сущности \"{otype.GetDescription()}\" нет раздела \"{context.Page.GetDescription()}\"");
            }

            var sql = @"
            SELECT
              d.Id,
              d.IrsGuid,
              e.OwnerTypeId,
              e.Id AS OwnerId,
              d.DocumentsTypeId,
              d.Code,
              d.Name,
              dt.Name AS DocumentsTypeName,
              d.DocumentDate,
              d.CreateDate,
              d.TimeStamp,
              dt.PrintDocType  
            FROM Entities e WITH(NOLOCK)
              INNER JOIN Documents2Entity d2e WITH(NOLOCK) ON d2e.OwnerId = e.Id
              INNER JOIN Documents d WITH(NOLOCK) ON d.Id = d2e.DocumentId
              INNER JOIN DocumentsType dt WITH(NOLOCK) ON dt.Id = d.DocumentsTypeId
              INNER JOIN PageDocType pdt WITH (NOLOCK) ON pdt.Id = dt.PageDocTypeId
            WHERE e.Id = @OwId AND e.OwnerTypeId = @OwTypeId ";

            if (context.Page != PageDocTypeEnum.None)
            {
                sql += "AND pdt.Id = @PageTypeId";
            }

            return await _repository.GetAllAsync<DocumentsDto>(sql, new { OwTypeId = context.OwnerTypeId, OwId = context.OwnerId, PageTypeId = context.Page });
        }
    }
}
