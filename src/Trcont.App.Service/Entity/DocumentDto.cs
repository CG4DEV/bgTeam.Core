namespace Trcont.App.Service.Entity
{
    public class DocumentDto
    {
        public byte[] Content { get; set; }

        public string FileExtension { get; set; }

        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }
    }
}
