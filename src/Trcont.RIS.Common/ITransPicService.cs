namespace Trcont.Ris.Common
{
    using System.Collections.Generic;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.TransPicture;

    public interface ITransPicService
    {
        string GetTransPicString(ITransPicOrder order, IEnumerable<FactByRouteDto> facts);
    }
}
