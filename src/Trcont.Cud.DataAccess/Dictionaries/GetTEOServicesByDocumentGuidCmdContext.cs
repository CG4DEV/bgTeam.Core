namespace Trcont.Cud.DataAccess.Dictionaries
{
    using System;

    public class GetTEOServicesByDocumentGuidCmdContext
    {
        public GetTEOServicesByDocumentGuidCmdContext(Guid documentGuid)
        {
            this.DocumentGuid = documentGuid;
        }

        public Guid DocumentGuid { get; set; }
    }
}
