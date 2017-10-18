namespace Trcont.Ris.Story.Info
{
    using System;
    using Trcont.Ris.Domain.Enums;

    public class GetDocumentPrintStoryContext
    {
        public int DocId { get; set; }

        public DocTypeEnumType DocType { get; set; }

        public FileExtensionEnum FileExtension { get; set; }

        public Guid LangGuid { get { return new Guid("7250baf8-a095-4e0d-9787-6e3b91e411ef"); } }
    }
}
