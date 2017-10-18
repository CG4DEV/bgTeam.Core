namespace Trcont.Cud.Common
{
    using Trcont.Cud.Domain;
    using Trcont.Cud.Domain.Entity.ExtInfo;

    public interface ITransPicService
    {
        string GetTransPicString(ITransPicOrder order, DislocationStation disloc);
    }
}
