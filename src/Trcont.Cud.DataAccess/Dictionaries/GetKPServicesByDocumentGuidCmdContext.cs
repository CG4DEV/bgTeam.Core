namespace Trcont.Cud.DataAccess.Dictionaries
{
    using System;

    public class GetKPServicesByDocumentGuidCmdContext
    {
        public GetKPServicesByDocumentGuidCmdContext(Guid documentGuid)
        {
            this.DocumentGuid = documentGuid;
        }

        public Guid DocumentGuid { get; set; }
    }
}