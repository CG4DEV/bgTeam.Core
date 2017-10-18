namespace Trcont.Ris.WebApp.Controllers
{
    using bgTeam;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Trcont.App.Service.Entity;
    using Trcont.Domain.Common;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Domain.Enums;
    using Trcont.Ris.Story.Common;
    using Trcont.Ris.Story.Contracts;
    using Trcont.Ris.Story.Info;
    using Trcont.Ris.Story.Orders;

    public partial class InfoController : Controller
    {
        private readonly IStoryBuilder _storyBuilder;

        public InfoController(IStoryBuilder storyBuilder)
        {
            _storyBuilder = storyBuilder;
        }

        [HttpPost]
        public async Task<Trcont.Domain.Common.SaldoInfoDto> GetSaldoByContractGuid(GetSaldoByContractGuidStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<Trcont.Domain.Common.SaldoInfoDto>();
        }

        [HttpPost]
        public async Task<IEnumerable<ContractDto>> GetContractsByClientGuid(GetContractsByClientGuidStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<ContractDto>>();
        }

        [HttpPost]
        public async Task<ContractInfoDto> GetContractInfoByGuid(GetContractInfoByGuidStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<ContractInfoDto>();
        }

        [HttpPost]
        public async Task<ClientSaldoDto> GetSaldoByClientGuid(GetSaldoByClientGuidStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<ClientSaldoDto>();
        }

        [HttpPost]
        public async Task<IEnumerable<PointServices>> GetParamsValueByService(GetParamsValueByServiceIdStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<PointServices>>();
        }

        [HttpPost]
        public async Task<IEnumerable<DocumentsDto>> GetDocumentsForEntity(GetDocumentsForEntityStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<DocumentsDto>>();
        }

        [HttpGet]
        public async Task<IActionResult> GetDocumentPrint(int docId, DocTypeEnumType docType, FileExtensionEnum fileExtension)
        {
            var fileResult = await _storyBuilder
                .Build(new GetDocumentPrintStoryContext() { DocId = docId, DocType = docType, FileExtension = fileExtension })
                .ReturnAsync<FileResponseDto>();

            if (fileResult.IsError)
            {
                return BadRequest(fileResult.ErrorMessage);
            }

            return File(fileResult.Content, "application/octet-stream", fileResult.FileName);
        }

        [HttpPost]
        public async Task<Documents> CreateContractBillById(CreateContractBillByIdStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<Documents>();
        }

        [HttpGet]
        public async Task<IEnumerable<AccountBillItemDto>> GetContractServceTypes()
        {
            return await _storyBuilder
                .Build(new GetContractServceTypesStoryContext())
                .ReturnAsync<IEnumerable<AccountBillItemDto>>();
        }

        [HttpPost]
        public async Task<IEnumerable<DislocationInfoDto>> GetOrderDislock(GetOrderDislockStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<DislocationInfoDto>>();
        }

        [HttpPost]
        public async Task<IEnumerable<ServiceDto>> GetServiceParams(GetServiceParamsStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<ServiceDto>>();
        }

        [HttpPost]
        public async Task<IEnumerable<OrderFactDto>> GetFactsByOrderId(GetFactsByOrderIdStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<OrderFactDto>>();
        }

        [HttpPost]
        public async Task<IEnumerable<object>> GetDictionary(GetDictionaryStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<IEnumerable<object>>();
        }

        [HttpPost]
        public async Task<AutoZoneDto> GetAutoZoneByCnsiCode(GetAutoZoneByCnsiCodeStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<AutoZoneDto>();
        }
    }
}
