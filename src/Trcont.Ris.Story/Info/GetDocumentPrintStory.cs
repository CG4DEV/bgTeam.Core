namespace Trcont.Ris.Story.Info
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using Trcont.App.Service;
    using Trcont.App.Service.Entity;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Enums;

    public class GetDocumentPrintStory : IStory<GetDocumentPrintStoryContext, FileResponseDto>
    {
        private readonly IAppServiceClient _client;
        private readonly IRepository _repository;
        private readonly IMapperBase _mapper;

        public GetDocumentPrintStory(IAppServiceClient client, IRepository repository, IMapperBase mapper)
        {
            _client = client;
            _repository = repository;
            _mapper = mapper;
        }

        public FileResponseDto Execute(GetDocumentPrintStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<FileResponseDto> ExecuteAsync(GetDocumentPrintStoryContext context)
        {
            if (context.DocId == 0)
            {
                throw new ArgumentException(nameof(context.DocId));
            }

            var doc = await GetLoadDocAsync(context.DocId, context.DocType);
            if (doc == null)
            {
                throw new ArgumentException($"Документ с идентификатором {context.DocId} не найден");
            }

            var response = await GetDocumentAsync(doc.IrsGuid, context);
            var result = new FileResponseDto();

            if (response == null)
            {
                result.IsError = true;
                result.ErrorMessage = "Response is null";
                return result;
            }

            _mapper.Map(response, result);
            if (response.IsError)
            {
                return result;
            }

            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            doc.Code = r.Replace(doc.Code, "_");

            result.FileName = doc.Code + "." + response.FileExtension;
            return result;
        }

        /// <summary>
        /// Мы не пересылаем в качестве документов сущности КП, заказы и контракты, поэтому делаем их обработку здесь
        /// </summary>
        /// <returns></returns>
        private async Task<DocumentMinDto> GetLoadDocAsync(int docId, DocTypeEnumType docType)
        {
            switch (docType)
            {
                case DocTypeEnumType.Contract:
                    {
                        return await LoadContractAsync(docId);
                    }

                case DocTypeEnumType.Kp:
                    {
                        return await LoadOrderAsync(docId, true);
                    }

                case DocTypeEnumType.Teo:
                    {
                        return await LoadOrderAsync(docId, false);
                    }

                default:
                    {
                        return await LoadDocAsync(docId);
                    }
            }
        }

        private async Task<DocumentMinDto> LoadOrderAsync(int docId, bool isKp)
        {
            string keyColumn;
            string searchColumn;
            if (isKp)
            {
                keyColumn = "Id";
                searchColumn = "IrsGuid";
            }
            else
            {
                keyColumn = "TeoId";
                searchColumn = "TeoGuid";
            }

            var sql = $@"
                SELECT 
                     doc.{searchColumn} AS IrsGuid,
                     doc.DocumentTitle AS Code
                FROM Orders doc WITH (NOLOCK)
                WHERE doc.{keyColumn} = @DocId";

            return await _repository.GetAsync<DocumentMinDto>(sql, new { DocId = docId });
        }

        private async Task<DocumentMinDto> LoadContractAsync(int docId)
        {
            var sql = @"
                SELECT 
                     doc.IrsGuid,
                     doc.Name AS Code
                FROM Contract doc WITH (NOLOCK)
                WHERE doc.Id = @DocId";

            return await _repository.GetAsync<DocumentMinDto>(sql, new { DocId = docId });
        }

        private async Task<DocumentMinDto> LoadDocAsync(int docId)
        {
            var sql = @"
                SELECT 
                     doc.IrsGuid,
                     doc.Code
                FROM Documents doc WITH (NOLOCK)
                WHERE doc.Id = @DocId";

            return await _repository.GetAsync<DocumentMinDto>(sql, new { DocId = docId });
        }

        private async Task<DocumentDto> GetDocumentAsync(Guid docGuid, GetDocumentPrintStoryContext context)
        {
            if (context.FileExtension == FileExtensionEnum.Doc)
            {
                switch (context.DocType)
                {
                    case DocTypeEnumType.Base:
                        {
                            return await _client.GetBaseDocumentDocContentAsync(docGuid, context.LangGuid);
                        }

                    case DocTypeEnumType.Contract:
                        {
                            return await _client.GetContractDocumentDocContentAsync(docGuid, context.LangGuid);
                        }

                    case DocTypeEnumType.Kp:
                        {
                            return await _client.GetKpDocumentDocContent(docGuid, context.LangGuid);
                        }

                    case DocTypeEnumType.Teo:
                        {
                            return await _client.GetTeoDocumentDocContentAsync(docGuid, context.LangGuid);
                        }

                    case DocTypeEnumType.BillTeo:
                        {
                            return await _client.GetBillTeoDocumentDocContentAsync(docGuid, context.LangGuid);
                        }

                    default:
                        {
                            throw new ArgumentException("Передан неизвестный тип документа");
                        }
                }
            }
            else if (context.FileExtension == FileExtensionEnum.Pdf)
            {
                switch (context.DocType)
                {
                    case DocTypeEnumType.Base:
                        {
                            return await _client.GetBaseDocumentPdfContentAsync(docGuid, context.LangGuid);
                        }

                    case DocTypeEnumType.Contract:
                        {
                            return await _client.GetContractDocumentPdfContentAsync(docGuid, context.LangGuid);
                        }

                    case DocTypeEnumType.Kp:
                        {
                            return await _client.GetKpDocumentPdfContent(docGuid, context.LangGuid);
                        }

                    case DocTypeEnumType.Teo:
                        {
                            return await _client.GetTeoDocumentPdfContentAsync(docGuid, context.LangGuid);
                        }

                    case DocTypeEnumType.BillTeo:
                        {
                            return await _client.GetBillTeoDocumentPdfContentAsync(docGuid, context.LangGuid);
                        }

                    default:
                        {
                            throw new ArgumentException("Передан неизвестный тип документа");
                        }
                }
            }

            throw new ArgumentException("Передан неизвестный тип файла");
        }
    }
}
