namespace Trcont.Ris.Domain.Dto
{
    public class FileResponseDto
    {
        public byte[] Content { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsError { get; set; }

        public string FileName { get; set; }
    }
}
