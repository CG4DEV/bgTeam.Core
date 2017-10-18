namespace Trcont.Ris.Story.Info
{
    using Trcont.Ris.Domain.Enums;

    public class GetDocumentsForEntityStoryContext
    {
        public int? OwnerId { get; set; }

        public int OwnerTypeId { get; set; }

        public PageDocTypeEnum Page { get; set; }
    }
}