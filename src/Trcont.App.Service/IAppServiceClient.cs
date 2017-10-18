namespace Trcont.App.Service
{
    using System;
    using System.Threading.Tasks;
    using Trcont.App.Service.Entity;
    using Trcont.Domain.Entity;

    public interface IAppServiceClient
    {
        Task<string> SaveOrUpdateIrsOrderAsync(KpDocuments order);

        Task<Guid?> SaveOrUpdateOrderAsync(OrderInfo order);

        Task<DocumentDto> GetBaseDocumentDocContentAsync(Guid docGuid, Guid? landGuid);

        Task<DocumentDto> GetBaseDocumentPdfContentAsync(Guid docGuid, Guid? landGuid);

        Task<DocumentDto> GetContractDocumentDocContentAsync(Guid docGuid, Guid? landGuid);

        Task<DocumentDto> GetContractDocumentPdfContentAsync(Guid docGuid, Guid? landGuid);

        Task<DocumentDto> GetKpDocumentDocContent(Guid docGuid, Guid? landGuid);

        Task<DocumentDto> GetKpDocumentPdfContent(Guid docGuid, Guid? landGuid);

        Task<DocumentDto> GetTeoDocumentDocContentAsync(Guid docGuid, Guid? landGuid);

        Task<DocumentDto> GetTeoDocumentPdfContentAsync(Guid docGuid, Guid? landGuid);

        Task<DocumentDto> GetBillTeoDocumentDocContentAsync(Guid docGuid, Guid? landGuid);

        Task<DocumentDto> GetBillTeoDocumentPdfContentAsync(Guid docGuid, Guid? landGuid);

        Task<Guid> CreateOrderBillAsync(Guid teoGuid, Guid currencyGuid, Guid userGuid);

        Task<Guid> CreateContractAccountBillAsync(Guid contractGuid, int[] index, decimal[] billSumm, Guid userGuid, bool isClient);
    }
}
