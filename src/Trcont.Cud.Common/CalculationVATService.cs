namespace Trcont.Cud.Common
{
    using bgTeam.DataAccess;
    using DapperExtensions.Mapper;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Cud.Domain.Common;

    /// <summary>
    /// Сервис расчета НДС
    /// </summary>
    public interface ICalculationVATService
    {
        Task CalculateAsync(RouteCollection route, int contCount);
    }

    public class CalculationVATService : ICalculationVATService
    {
        private readonly IRepository _repository;

        public CalculationVATService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task CalculateAsync(RouteCollection route, int contCount)
        {
            // собираем все страны, в которых расположены станции
            var points = new List<Guid?>();
            foreach (var item in route)
            {
                points.Add(item.FromCountryGuid);
                points.Add(item.ToCountryGuid);
            }

            var isRussianStation = !points
                .Where(x => x.HasValue)
                .Where(x => x != LH.ctRussia)
                .Any();

            foreach (var item in route)
            {
                if (!(item is RouteTrain))
                {
                    continue;
                }

                var countryFrom = item.FromCountryGuid.Value;
                var countryTo = item.ToCountryGuid.Value;

                foreach (var srv in item.Services)
                {
                    var payType = CalculateVATForService(true, srv.Code, LH.ctRussia, LH.ctRussia, srv.TerritoryGuid, true, false, false, true, true, countryFrom, countryTo, isRussianStation, GetCargo18Flag(null), KPTransType.ttExport);

                    srv.Tariff = srv.Summ;
                    srv.Summ = srv.Summ * contCount;

                    switch (payType)
                    {
                        case PayAccEnum.Pay18:
                            srv.TariffVAT = srv.Tariff + ((srv.Tariff / 100) * 18);
                            srv.SummVAT = srv.Summ + ((srv.Summ / 100) * 18);
                            break;

                        default:
                            srv.TariffVAT = srv.Tariff;
                            srv.SummVAT = srv.Summ;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Вычисление НДС услуг
        /// </summary>
        /// <param name="сontrRelationsType">Тип договорых отношений</param>
        /// <param name="usl">Номер услуги</param>
        /// <param name="cFromGUID">Страна отправления</param>
        /// <param name="cToGUID">Страна прибытия</param>
        /// <param name="terGUID">Территория</param>
        /// <param name="customMode">Таможенный режим</param>
        /// <param name="isResident">Резидент</param>
        /// <param name="nds0">НДС0</param>
        /// <param name="execTK">Исполнитель ТК</param>
        /// <param name="isUslTK">Присутствуют услуги ТК</param>
        /// <param name="ArmFromGUID">Страна отправления звена</param>
        /// <param name="ArmToGUID">Страна назначения звена</param>
        /// <param name="isRussianStation">Присутствуют российские станции</param>
        /// <param name="cargo18">Груз 18%</param>
        /// <param name="kpTransType">Тип перевозки</param>
        /// <returns>НДС</returns>
        public static PayAccEnum CalculateVATForService(
            bool сontrRelationsType,
            string usl,
            Guid cFromGUID,
            Guid? cToGUID,
            Guid? terGUID,
            bool customMode,
            bool isResident,
            bool nds0,
            bool execTK,
            bool isUslTK,
            Guid armCFromGUID,
            Guid armCToGUID,
            bool isRussianStation,
            bool cargo18,
            KPTransType kpTransType)
        {
            // -----------------------------------------------------------
            // 0 - Расходная
            // 1 - Внутренняя
            // 2 - Транзитная
            int uslType = 1;
            if (IfUslCodes(usl, "1") && execTK)
            {
                uslType = сontrRelationsType ? 2 : 0;
            }

            if (IfUslCodes(usl, "0"))
            {
                uslType = 0;
            }

            // 0. -----------------------------------------------------------
            if (!IfUslCodes(usl, "1.2.1") && !сontrRelationsType)
            {
                if (cFromGUID != LH.ctRussia && cToGUID != LH.ctRussia && !isRussianStation)
                {
                    return PayAccEnum.PayNo;
                }

                if (isUslTK && !cargo18 && kpTransType != KPTransType.ttRussian)
                {
                    return PayAccEnum.Pay0;
                }

                return PayAccEnum.Pay18;
            }

            // 1. --------------------------------------------------------
            if (IfUslCodes(usl, "0.1,0.2,0.4,0.5,0.6,0.12,0.13,0.15,1.2,1.5,1.10,1.11"))
            {
                return PayAccEnum.Pay18;
            }

            // 2. --------------------------------------------------------
            if (IfUslCodes(usl, "1.7,1.8,1.12"))
            {
                return PayAccEnum.PayNo;
            }

            // 3. --------------------------------------------------------
            if (IfUslCodes(usl, "1.1"))
            {
                if (!IfUslCodes(usl, "1.1.1.1,1.1.1.3,1.1.1.4,1.1.1.5,1.1.1.6")
                  || kpTransType == KPTransType.ttRussian
                  || kpTransType == KPTransType.ttImport
                  || cargo18
                  || (kpTransType == KPTransType.ttExport && !customMode && cToGUID != LH.ctBalarus && cToGUID != LH.ctKhazahstan)
                  || (kpTransType == KPTransType.ttTransit && !customMode && (cFromGUID == LH.ctBalarus || cFromGUID == LH.ctKhazahstan) && (cToGUID == LH.ctBalarus || cToGUID == LH.ctKhazahstan)))
                {
                    return PayAccEnum.Pay18;
                }

                return PayAccEnum.Pay0;
            }

            // 4. --------------------------------------------------------
            if (IfUslCodes(usl, "1.3"))
            {
                if ((terGUID != null && terGUID != LH.ctRussia) || (terGUID == null && armCFromGUID != LH.ctRussia && armCToGUID != LH.ctRussia))
                {
                    return PayAccEnum.PayNo;
                }

                if (kpTransType == KPTransType.ttRussian || kpTransType == KPTransType.ttImport
                    || !IfUslCodes(usl, "1.3.1.1")
                    || (IfUslCodes(usl, "1.3.1.1") && cargo18)
                    || (IfUslCodes(usl, "1.3.1.1") && kpTransType == KPTransType.ttExport && !customMode))
                {
                    return PayAccEnum.Pay18;
                }

                return PayAccEnum.Pay0;
            }

            // 5. --------------------------------------------------------
            if (IfUslCodes(usl, "1.4"))
            {
                if (kpTransType == KPTransType.ttRussian)
                {
                    return PayAccEnum.Pay18;
                }

                return isResident ? PayAccEnum.Pay0 : PayAccEnum.PayNo;
            }

            // 6. --------------------------------------------------------
            if (IfUslCodes(usl, "1.6"))
            {
                if ((terGUID != null && terGUID != LH.ctRussia) || (terGUID == null && armCFromGUID != LH.ctRussia && armCToGUID != LH.ctRussia))
                {
                    return PayAccEnum.PayNo;
                }

                if (!cargo18 && kpTransType != KPTransType.ttRussian && customMode)
                {
                    return PayAccEnum.Pay0;
                }

                return PayAccEnum.Pay18;
            }

            // 7. --------------------------------------------------------
            if (IfUslCodes(usl, "1.9"))
            {
                if (kpTransType == KPTransType.ttRussian)
                {
                    return PayAccEnum.Pay18;
                }

                if (uslType != 2)
                {
                    if (cargo18 && isResident)
                    {
                        return PayAccEnum.Pay18;
                    }

                    return isResident ? PayAccEnum.Pay0 : PayAccEnum.PayNo;
                }
                else
                {
                    if (!isResident)
                    {
                        return PayAccEnum.PayNo;
                    }

                    return cargo18 ? PayAccEnum.Pay18 : PayAccEnum.Pay0;
                }
            }

            // 8. --------------------------------------------------------
            if (IfUslCodes(usl, "2.1,2.2,2.5,2.7,2.13"))
            {
                if ((terGUID != null && terGUID != LH.ctRussia) || (terGUID == null && armCFromGUID != LH.ctRussia && armCToGUID != LH.ctRussia))
                {
                    return PayAccEnum.PayNo;
                }

                if (nds0 && !cargo18 && kpTransType != KPTransType.ttRussian)
                {
                    return PayAccEnum.Pay0;
                }

                return PayAccEnum.Pay18;
            }

            // 9. --------------------------------------------------------
            if (IfUslCodes(usl, "2.6,2.8,0.14"))
            {
                if ((terGUID != null && terGUID != LH.ctRussia) || (terGUID == null && armCFromGUID != LH.ctRussia && armCToGUID != LH.ctRussia))
                {
                    return PayAccEnum.PayNo;
                }

                if (isUslTK && nds0 && !cargo18 && kpTransType != KPTransType.ttRussian)
                {
                    return PayAccEnum.Pay0;
                }

                return PayAccEnum.Pay18;
            }

            // 10. --------------------------------------------------------
            if (IfUslCodes(usl, "2.3,2.4,2.9,2.10,2.12,2.14,2.15,2.16,2.17,2.18,2.19"))
            {
                if (isUslTK && nds0 && !cargo18 && kpTransType != KPTransType.ttRussian)
                {
                    return PayAccEnum.Pay0;
                }

                return PayAccEnum.Pay18;
            }

            return PayAccEnum.Pay0;
        }

        private static bool IfUslCodes(string usl, string codes)
        {
            usl = string.Join(".", usl.Split(' ')[0].Split('.').Select(c =>
            {
                int rc;
                return int.TryParse(c, out rc) ? rc.ToString(CultureInfo.InvariantCulture) : c;
            }));

            return codes.Split(',').Any(s => usl.IndexOf(s, StringComparison.Ordinal) == 0);
        }

        /// <summary>
        /// Вычисление груза 18%
        /// </summary>
        protected static bool GetCargo18Flag(Guid? etsngGuid)
        {
            return new[]
            {
                LH.etsngHomeCargo, LH.etsngEmptyWagonSpec, LH.etsngEmptyWagonUniRem, LH.etsngEmptyWagonUniReg, LH.etsngEmptyWagonUni
            }.Any(c => c == etsngGuid);
        }

        /// <summary>
        /// Вычисление типа перевозки
        /// </summary>
        /// <param name="countryFromGUID">Страна отправления</param>
        /// <param name="countryToGUID">Страна назначения</param>
        protected static KPTransType GetTransType(Guid countryFromGUID, Guid countryToGUID)
        {
            if (countryFromGUID != LH.ctRussia && countryToGUID == LH.ctRussia)
            {
                return KPTransType.ttImport;
            }

            if (countryFromGUID == LH.ctRussia && countryToGUID != LH.ctRussia)
            {
                return KPTransType.ttExport;
            }

            if (countryFromGUID != LH.ctRussia && countryToGUID != LH.ctRussia)
            {
                return countryFromGUID != countryToGUID ? KPTransType.ttTransit : KPTransType.ttInternal;
            }

            return KPTransType.ttRussian;
        }

        public enum PayAccEnum
        {
            Pay18 = 0,
            Pay0 = 1,
            PayNo = 2,
            PayNULL = 3
        }

        /// <summary>
        /// Тип перевозки
        /// </summary>
        public enum KPTransType
        {
            ttImport = 0,
            ttExport = 1,
            ttTransit = 2,
            ttInternal = 3,
            ttRussian = 4
        }

        public static partial class LH
        {
            public static Guid ctRussia = new Guid("0a504325-c94c-11d4-9e9f-00a0c9108841");
            public static Guid ctBalarus = new Guid("0A504295-C94C-11D4-9E9F-00A0C9108841");
            public static Guid ctKhazahstan = new Guid("0A5042E0-C94C-11D4-9E9F-00A0C9108841");

            //public static int ctRussiaId = 3;
            //public static int ctBalarusId = 26;
            //public static int ctKhazahstanId = 87;

            public static Guid etsngHomeCargo = new Guid("39069A0F-1EA1-43C6-A0A4-154D8A03430C");
            public static Guid etsngEmptyWagonSpec = new Guid("09EE7B36-C94C-11D4-9E9F-00A0C9108841");
            public static Guid etsngEmptyWagonUniRem = new Guid("09EE7B38-C94C-11D4-9E9F-00A0C9108841");
            public static Guid etsngEmptyWagonUniReg = new Guid("09EE7B37-C94C-11D4-9E9F-00A0C9108841");
            public static Guid etsngEmptyWagonUni = new Guid("09EE7B39-C94C-11D4-9E9F-00A0C9108841");
            public static Guid etsngThings = new Guid("098A6FDE-C94C-11D4-9E9F-00A0C9108841");
        }
    }
}
