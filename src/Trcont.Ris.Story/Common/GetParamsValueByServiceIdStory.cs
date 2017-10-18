namespace Trcont.Ris.Story.Common
{
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.Ris.DataAccess.Common;
    using Trcont.Ris.DataAccess.Services;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;

    public class GetParamsValueByServiceIdStory : IStory<GetParamsValueByServiceIdStoryContext, IEnumerable<PointServices>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IRepositoryEntity _repositoryEn;

        private Guid[] _ignoreParams;

        public GetParamsValueByServiceIdStory(
            IAppLogger logger,
            IRepository repository,
            IRepositoryEntity repositoryEn)
        {
            _logger = logger;
            _repository = repository;
            _repositoryEn = repositoryEn;

            // Заполняем игнорируемые параметры
            _ignoreParams = new[]
            {
                new Guid("3ccdd8e9-48c1-461f-a2fa-36b401be8bb7"), // Вид отправки

                new Guid("a6b2e88a-c07b-4a47-865b-c5bfee079861"), // Грузы ЕТСНГ
                new Guid("aa91460a-248c-4952-8106-8ac820724bd3"), // Масса брутто контейнера
                new Guid("57fc7e0a-ca2c-4484-80ca-b0cae8fb3224"), // Парк вагона
                new Guid("96cefd54-0f1e-4cf4-8930-dd8ecf3f75ba"), // Парк контейнера
                //new Guid("4fc42115-25b9-4a08-b6a7-d5a7d140ca00"), // Ранг отправки
                new Guid("0000004d-0000-0000-0000-000000000000"), // Типы контейнера
                new Guid("0000004e-0000-0000-0000-000000000000"), // Зоны автодоставки
                new Guid("69cd13d5-4d8e-4fff-9ce7-984aa10b2463"), // Единица измерения
                new Guid("d58757af-47da-4195-b850-cf6dd5549cb6"), // Признак отправления/ прибытия
                new Guid("3d08f9b5-5122-4562-aa1e-1f019287c80e"), // Доп. условие автоперевозки
                new Guid("f003be0d-8054-44bf-a4ec-0779a057b455"), // Тип комплекса
                new Guid("fb0c9f53-2084-4b17-86e1-39022c8dd44a") // Терминал оказания услуги
            };
        }

        public IEnumerable<PointServices> Execute(GetParamsValueByServiceIdStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<PointServices>> ExecuteAsync(GetParamsValueByServiceIdStoryContext context)
        {
            CheckInputParams(context);

            // Определяем услугу по коду
            await SetServicesIdsAsync(context);

            // Определяем местоположение по коду
            await SetPointsIdsAsync(context);

            CheckParams(context);

            var result = new List<PointServices>();
            foreach (var point in context.InputParams)
            {
                var ps = await GetParamsValueForRouteAsync(point);
                result.Add(ps);
            }

            return result;
        }

        private async Task<PointServices> GetParamsValueForRouteAsync(ContextForParamValues point)
        {
            var serviceIds = point.ServiceIds.Select(s => s.Value);

            var ignorePrms = new List<Guid>(_ignoreParams);

            // IS-140 Фильтрация параметров услуг. 09.
            if (!point.BeginPointCnsi.StartsWith("P_") && !point.EndPointCnsi.StartsWith("P_"))
            {
                // Соисполнитель /агент в порту
                ignorePrms.Add(new Guid("00000059-0000-0000-0000-000000000000"));
            }

            var paramsSrv = await new GetParamsForRouteCmd(_repository)
                .ExecuteAsync(new GetParamsForRouteCmdContext() { ServiceIds = serviceIds.Distinct().ToArray(), IgnoreParams = ignorePrms.ToArray() });

            // Получаем значения для параметров из доменного справочника
            foreach (var item in paramsSrv)
            {
                switch (item.ParamGuid.ToString())
                {
                    case "0000004e-0000-0000-0000-000000000000":
                        item.DictionaryName = "Zone";
                        break;

                    case "0ec279bf-7476-4c6a-99c8-9eaac4cebc59":
                    case "f512fdc1-d654-4d44-a412-e2798cb88a54":
                    case "49990e6b-fcb1-4cd6-ac6b-45fd9d723f7f":
                    case "00000059-0000-0000-0000-000000000000":
                        item.DictionaryName = "Client";
                        break;

                    case "0000004a-0000-0000-0000-000000000000":
                    case "0000004b-0000-0000-0000-000000000000":
                    case "0000004c-0000-0000-0000-000000000000":
                        item.DictionaryName = "Point";
                        break;

                    case "69cd13d5-4d8e-4fff-9ce7-984aa10b2463":
                        item.DictionaryName = "Unit";
                        break;

                    default:
                        item.Values = (await _repository.GetAllAsync<DomainDictValues>(
                            "SELECT TOP 50 DomainGuid, IrsGuid, Value FROM DomainsValues WITH(NOLOCK) WHERE DomainGuid = @DomainGuid", new { DomainGuid = item.DomainGuid }))
                            .Select(x =>
                                new ServiceParamValues()
                                {
                                    Name = item.DomainName,
                                    IrsGuid = x.IrsGuid,
                                    Value = x.Value
                                })
                             .OrderBy(x => x.Value)
                             .ToArray();

                        // TODO : Убираем дубли, мега костыль
                        var list = new List<ServiceParamValues>();
                        var groupList = item.Values.GroupBy(x => x.Value);
                        foreach (var row in groupList)
                        {
                            var first = row.First();
                            list.Add(first);
                        }

                        item.Values = list;
                        break;
                }
            }

            return new PointServices()
            {
                InputBeginPointCnsi = point.BeginPointCnsi,
                BeginPointCnsi = point.OutBeginPointCnsi,
                BeginPointGuid = point.OutBeginPointGuid,
                BeginPointTitle = point.OutBeginPointTitle,

                InputEndPointCnsi = point.EndPointCnsi,
                EndPointCnsi = point.OutEndPointCnsi,
                EndPointGuid = point.OutEndPointGuid,
                EndPointTitle = point.OutEndPointTitle,
                ServiceParams = paramsSrv
            };
        }

        private static void CheckParams(GetParamsValueByServiceIdStoryContext context)
        {
            if (context.InputParams.Any(x => x.ServiceCodes != null && x.ServiceCodes.Any() && x.ServiceCodes.Distinct().Count() != x.ServiceIds.Count()))
            {
                throw new ArgumentException("Найдены не все услуги");
            }

            if (context.InputParams.Any(x => !x.ServiceIds.Any()))
            {
                throw new ArgumentException("Найдены не все услуги");
            }

            //if (context.InputParams.Any(x => !x.OutBeginPointGuid.HasValue || !x.OutEndPointGuid.HasValue))
            //{
            //    throw new ArgumentException("Найдены не все местоположения");
            //}
        }

        private static void CheckInputParams(GetParamsValueByServiceIdStoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.InputParams == null || !context.InputParams.Any())
            {
                throw new ArgumentNullException(nameof(context.InputParams));
            }
        }

        private async Task SetPointsIdsAsync(GetParamsValueByServiceIdStoryContext context)
        {
            var pointCodes = new List<string>();

            foreach (var param in context.InputParams)
            {
                if (!string.IsNullOrWhiteSpace(param.BeginPointCnsi))
                {
                    pointCodes.Add(param.BeginPointCnsi);
                }

                if (!string.IsNullOrWhiteSpace(param.EndPointCnsi))
                {
                    pointCodes.Add(param.EndPointCnsi);
                }
            }

            pointCodes = pointCodes.Distinct().ToList();

            if (pointCodes.Any())
            {
                await MapPointsAsync(context, pointCodes);

                //Если остались не заполненные поля, пытаемся смаппить на зону автодоставки
                if (context.InputParams.Any(x => 
                    !string.IsNullOrWhiteSpace(x.BeginPointCnsi) && !x.OutBeginPointGuid.HasValue ||
                    !string.IsNullOrWhiteSpace(x.EndPointCnsi) && !x.OutEndPointGuid.HasValue))
                {
                    await MapZonesAsync(context);
                }
            }
        }

        private async Task MapZonesAsync(GetParamsValueByServiceIdStoryContext context)
        {
            var zoneCodes = new List<string>();
            foreach (var param in context.InputParams)
            {
                if (!string.IsNullOrWhiteSpace(param.BeginPointCnsi) && !param.OutBeginPointGuid.HasValue)
                {
                    zoneCodes.Add(param.BeginPointCnsi);
                }

                if (!string.IsNullOrWhiteSpace(param.EndPointCnsi) && !param.OutEndPointGuid.HasValue)
                {
                    zoneCodes.Add(param.EndPointCnsi);
                }
            }

            zoneCodes = zoneCodes.Distinct().ToList();

            var zones = await _repositoryEn.GetAllAsync<Zone>(x => zoneCodes.Contains(x.CnsiCode));
            foreach (var param in context.InputParams)
            {
                if (!string.IsNullOrWhiteSpace(param.BeginPointCnsi) && !param.OutBeginPointGuid.HasValue)
                {
                    var zone = zones.SingleOrDefault(z => z.CnsiCode == param.BeginPointCnsi);
                    if (zone != null)
                    {
                        param.OutBeginPointGuid = zone.IrsGuid;
                        param.OutBeginPointCnsi = zone.CnsiCode;
                        param.OutBeginPointTitle = zone.Name;
                    }
                }

                if (!string.IsNullOrWhiteSpace(param.EndPointCnsi) && !param.OutEndPointGuid.HasValue)
                {
                    var zone = zones.SingleOrDefault(z => z.CnsiCode == param.EndPointCnsi);
                    if (zone != null)
                    {
                        param.OutEndPointGuid = zone.IrsGuid;
                        param.OutEndPointCnsi = zone.CnsiCode;
                        param.OutEndPointTitle = zone.Name;
                    }
                }
            }
        }

        private async Task MapPointsAsync(GetParamsValueByServiceIdStoryContext context, List<string> pointCodes)
        {
            var pointMaps = await _repositoryEn.GetAllAsync<vPointMap>(x => pointCodes.Contains(x.SlaveCnsiCode));
            var masterCnsiGuids = pointMaps.Select(x => (Guid?)x.MasterCnsiGuid).ToList();
            if (!masterCnsiGuids.Any())
            {
                return;
            }

            var points = await _repositoryEn.GetAllAsync<vPoints>(x => masterCnsiGuids.Contains(x.CnsiGuid));

            // заполняем смапленными значениями пункты
            foreach (var param in context.InputParams)
            {
                if (!string.IsNullOrWhiteSpace(param.BeginPointCnsi))
                {
                    var map = pointMaps.SingleOrDefault(p => p.SlaveCnsiCode == param.BeginPointCnsi);
                    if (map != null)
                    {
                        // TODO : есть значения с одинаковым CnsiCode, пока неясно что делать
                        var point = points.FirstOrDefault(p => p.CnsiGuid == map.MasterCnsiGuid);
                        if (point != null)
                        {
                            param.OutBeginPointCnsi = point.CnsiCode;
                            param.OutBeginPointGuid = point.IrsGuid;
                            param.OutBeginPointTitle = point.Title;
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(param.EndPointCnsi))
                {
                    var map = pointMaps.SingleOrDefault(p => p.SlaveCnsiCode == param.EndPointCnsi);
                    if (map != null)
                    {
                        // TODO : есть значения с одинаковым CnsiCode, пока неясно что делать
                        var point = points.FirstOrDefault(p => p.CnsiGuid == map.MasterCnsiGuid);
                        if (point != null)
                        {
                            param.OutBeginPointCnsi = point.CnsiCode;
                            param.OutEndPointGuid = point.IrsGuid;
                            param.OutEndPointTitle = point.Title;
                        }
                    }
                }
            }
        }

        private async Task SetServicesIdsAsync(GetParamsValueByServiceIdStoryContext context)
        {
            var serviceCodes = new List<string>();
            context.InputParams
                .Where(x => x.ServiceCodes != null && x.ServiceCodes.Any())
                .DoForEach(x => serviceCodes.AddRange(x.ServiceCodes));

            serviceCodes = serviceCodes.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct().ToList();

            if (serviceCodes.Any())
            {
                var services = await _repositoryEn.GetAllAsync<Service>(x => serviceCodes.Contains(x.Code) && x.ServiceGroupId == (int)ServiceGroupEnum.EPU);

                context.InputParams
                    .Where(x => x.ServiceCodes != null && x.ServiceCodes.Any())
                    .DoForEach(x =>
                    {
                        var list = new List<int?>();
                        list.AddRange(services.Where(s => x.ServiceCodes.Select(y => y.Trim()).Contains(s.Code.Trim()))
                            .Select(s => s.Id));
                        x.ServiceIds = list;
                    });
            }
        }
    }
}
