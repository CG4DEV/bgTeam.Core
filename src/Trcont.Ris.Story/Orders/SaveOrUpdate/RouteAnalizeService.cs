namespace Trcont.Ris.Story.Orders.SaveOrUpdate
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Domain.TransPicture;

    public class RouteAnalizeService
    {
        private readonly IRepositoryEntity _repository;

        private readonly Hashtable _cashList;

        public RouteAnalizeService(IRepositoryEntity repository)
        {
            _repository = repository;

            _cashList = new Hashtable();
        }

        internal async Task AnalizeAsync(SaveOrUpdateOrderStoryContext context)
        {
            foreach (var route in context.Routes)
            {
                switch (route.RouteType)
                {
                    case Domain.Enums.RouteTypeEnum.TL:
                        route.FromPointGuid = await GetSationNullByCodeGuidAsync(route.FromPointCnsi, route.FromPointGuid);
                        route.ToPointGuid = await GetSationNullByCodeGuidAsync(route.ToPointCnsi, route.ToPointGuid);

                        if (route.FromPointGuid == null)
                        {
                            // Заполняем для разбора ответа от фсс
                            route.FromPointCode = route.FromPointCnsi;
                            route.ToPointCode = GetStationCodeFromCash(route.ToPointCnsi);

                            route.FromPointTitle = GetPointTitleFromCash(route.FromPointCode, route.FromPointTitle);
                            route.FromPointCnsi = route.ToPointCnsi;
                            route.FromPointGuid = route.ToPointGuid;
                            route.PointNow = route.ToPointGuid;
                            route.TariffZone = await GetRowNullByCodeGuidAsync<Zone>(route.FromPointCnsi, x => x.CnsiCode == route.FromPointCnsi, route.FromPointGuid);
                        }
                        else
                        {
                            route.FromPointTitle = GetPointTitleFromCash(route.FromPointCnsi, route.FromPointTitle);
                        }

                        if (route.ToPointGuid == null)
                        {
                            // Заполняем для разбора ответа от фсс
                            route.FromPointCode = GetStationCodeFromCash(route.FromPointCnsi);
                            route.ToPointCode = route.ToPointCnsi;

                            route.ToPointTitle = GetPointTitleFromCash(route.ToPointCode, route.ToPointTitle);
                            route.ToPointCnsi = route.FromPointCnsi;
                            route.ToPointGuid = route.FromPointGuid;
                            route.PointNow = route.FromPointGuid;
                            route.TariffZone = await GetRowNullByCodeGuidAsync<Zone>(route.ToPointCnsi, x => x.CnsiCode == route.ToPointCnsi, route.ToPointGuid);
                        }
                        else
                        {
                            route.ToPointTitle = GetPointTitleFromCash(route.ToPointCnsi, route.ToPointTitle);
                        }

                        // Заполняем для разбора ответа от фсс
                        route.FromPointId = GetStationIdFromCash(route.FromPointCnsi);
                        route.ToPointId = GetStationIdFromCash(route.ToPointCnsi);
                        //route.FromPointCode = GetStationCodeFromCash(route.FromPointCnsi);
                        //route.ToPointCode = GetStationCodeFromCash(route.ToPointCnsi);
                        break;

                    case Domain.Enums.RouteTypeEnum.RAIL:
                    case Domain.Enums.RouteTypeEnum.VESSEL:
                        route.FromPointGuid = await GetSationByCodeGuidAsync(route.FromPointCnsi, route.FromPointGuid);
                        route.ToPointGuid = await GetSationByCodeGuidAsync(route.ToPointCnsi, route.ToPointGuid);
                        route.PointNow = route.FromPointGuid;

                        route.FromPointTitle = GetPointTitleFromCash(route.FromPointCnsi, route.FromPointTitle);
                        route.ToPointTitle = GetPointTitleFromCash(route.ToPointCnsi, route.ToPointTitle);

                        // Заполняем для разбора ответа от фсс
                        route.FromPointId = GetStationIdFromCash(route.FromPointCnsi);
                        route.ToPointId = GetStationIdFromCash(route.ToPointCnsi);
                        route.FromPointCode = GetStationCodeFromCash(route.FromPointCnsi);
                        route.ToPointCode = GetStationCodeFromCash(route.ToPointCnsi);
                        break;

                    case Domain.Enums.RouteTypeEnum.INTRMDL:
                        route.FromPointGuid = await GetSationNullByCodeGuidAsync(route.FromPointCnsi, route.FromPointGuid);
                        route.ToPointGuid = await GetSationNullByCodeGuidAsync(route.ToPointCnsi, route.ToPointGuid);

                        if (route.FromPointGuid == null)
                        {
                            route.FromPointCnsi = route.ToPointCnsi;
                            route.FromPointGuid = route.ToPointGuid;
                            route.PointNow = route.ToPointGuid;
                        }

                        if (route.ToPointGuid == null)
                        {
                            route.ToPointCnsi = route.FromPointCnsi;
                            route.ToPointGuid = route.FromPointGuid;
                            route.PointNow = route.FromPointGuid;
                        }

                        route.FromPointTitle = GetPointTitleFromCash(route.FromPointCnsi, route.FromPointTitle);
                        route.ToPointTitle = GetPointTitleFromCash(route.ToPointCnsi, route.ToPointTitle);

                        // Заполняем для разбора ответа от фсс
                        route.FromPointId = GetStationIdFromCash(route.FromPointCnsi);
                        route.ToPointId = GetStationIdFromCash(route.ToPointCnsi);
                        route.FromPointCode = GetStationCodeFromCash(route.FromPointCnsi);
                        route.ToPointCode = GetStationCodeFromCash(route.ToPointCnsi);
                        break;

                    case Domain.Enums.RouteTypeEnum.JUNCTION:
                        var stfrom = await GetSationByCodeGuidAsync(route.FromPointCnsi, route.FromPointGuid);
                        var stto = await GetSationByCodeGuidAsync(route.ToPointCnsi, route.ToPointGuid);

                        // Заполняем для разбора ответа от фсс
                        route.FromPointId = GetStationIdFromCash(route.FromPointCnsi);
                        route.ToPointId = GetStationIdFromCash(route.ToPointCnsi);
                        route.FromPointTitle = GetPointTitleFromCash(route.FromPointCnsi, route.FromPointTitle);
                        route.ToPointTitle = GetPointTitleFromCash(route.ToPointCnsi, route.ToPointTitle);
                        route.FromPointCode = GetStationCodeFromCash(route.FromPointCnsi);
                        route.ToPointCode = GetStationCodeFromCash(route.ToPointCnsi);

                        var from = GetStationFromCash(route.FromPointCode);
                        if (from.PointType == (int)PointTypeEnum.PointPortType)
                        {
                            route.FromPointCnsi = route.ToPointCnsi;
                        }

                        var to = GetStationFromCash(route.ToPointCode);
                        if (to.PointType == (int)PointTypeEnum.PointPortType)
                        {
                            route.ToPointCnsi = route.FromPointCnsi;
                        }

                        route.FromPointGuid = await GetSationByCodeGuidAsync(route.FromPointCnsi, route.FromPointGuid);
                        route.ToPointGuid = await GetSationByCodeGuidAsync(route.ToPointCnsi, route.ToPointGuid);
                        route.PointNow = route.FromPointGuid;
                        break;

                    default:
                        throw new ArgumentException("RouteType");
                }
            }

            var railRoute = context.Routes.Where(x => x.RouteType == Domain.Enums.RouteTypeEnum.RAIL);

            context.PlaceFromGuid = railRoute.First().FromPointGuid;
            context.PlaceToGuid = railRoute.Last().ToPointGuid;
        }

        private string GetPointTitleFromCash(string code, string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                return title;
            }

            if (_cashList.ContainsKey(code))
            {
                var savedTitle = (_cashList[code] as vPoints).Title;
                return !string.IsNullOrWhiteSpace(savedTitle) ? savedTitle : code;
            }

            return code;
        }

        #region Получение справочных значение
        private async Task<Guid?> GetRowNullByCodeGuidAsync<T>(string code, Expression<Func<T, bool>> predicate, Guid? defValue = null)
            where T : class, IIrsDictionary
        {
            if (code == null)
            {
                return defValue;
            }

            if (_cashList.ContainsKey(code))
            {
                return _cashList[code] as Guid?;
            }

            var points = await _repository.GetAllAsync<T>(predicate);
            if (points.Any())
            {
                var point = points.First();
                _cashList.Add(code, point.IrsGuid);
                return point.IrsGuid;
            }

            if (defValue != null)
            {
                _cashList.Add(code, defValue);
            }

            return defValue;
        }

        private async Task<Guid?> GetSationGuidByCodeAsync(string code, Guid? defValue = null)
        {
            if (code == null)
            {
                return defValue;
            }

            if (_cashList.ContainsKey(code))
            {
                return (_cashList[code] as vPoints).IrsGuid;
            }

            var points = await _repository.GetAllAsync<vPoints>(x => x.CnsiCode == code);
            if (points.Any())
            {
                var point = points.First();
                _cashList.Add(code, point);
                _cashList.Add($"{code}_STATION", point.CnsiCode);
                return point.IrsGuid;
            }

            if (defValue != null)
            {
                _cashList.Add(code, defValue);
            }

            return defValue;
        }

        private async Task<Guid?> GetSationNullByCodeGuidAsync(string code, Guid? defValue = null)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            var res = await GetSationGuidByCodeAsync(code, defValue);
            if (!res.HasValue)
            {
                // Ищем через маппинг
                var points = await _repository.GetAllAsync<vPointMap>(x => x.SlaveCnsiCode == code);
                if (points.Any())
                {
                    var point = points.First();
                    var points2 = await _repository.GetAllAsync<vPoints>(x => x.CnsiGuid == point.MasterCnsiGuid);
                    if (points2.Any())
                    {
                        var point2 = points2.First();
                        _cashList.Add(code, point2);
                        _cashList.Add(point2.CnsiCode, point2);
                        _cashList.Add($"{code}_STATION", point2.CnsiCode);
                        return point2.IrsGuid;
                    }
                }
            }

            return res;
        }

        private async Task<Guid?> GetSationByCodeGuidAsync(string code, Guid? defValue = null)
        {
            var res = await GetSationNullByCodeGuidAsync(code, defValue);

            if (!res.HasValue)
            {
                throw new ArgumentNullException($"Not find vPoint, code - {code}");
            }

            return res.Value;
        }

        private int GetStationIdFromCash(string code)
        {
            if (_cashList.ContainsKey(code))
            {
                return (_cashList[code] as vPoints).Id;
            }

            throw new Exception($"Not find station by code - {code}");
        }

        private string GetStationCodeFromCash(string code)
        {
            var cd = $"{code}_STATION";
            if (_cashList.ContainsKey(cd))
            {
                return _cashList[cd] as string;
            }

            throw new Exception($"Not find station by code - {cd}");
        }

        private vPoints GetStationFromCash(string code)
        {
            if (_cashList.ContainsKey(code))
            {
                return _cashList[code] as vPoints;
            }

            throw new Exception($"Not find station by code - {code}");
        }
    }
    #endregion
}
