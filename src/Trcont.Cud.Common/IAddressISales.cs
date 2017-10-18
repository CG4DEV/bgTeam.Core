namespace Trcont.Cud.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using Trcont.Cud.Domain.Entity;

    public interface IAddressISales
    {
        AddressISalesInfo GetStructure(string address);

        string GetAddress(AddressISalesInfo info);

        string GetAddress(string regionGuid, int? cityType, string cityTitle, int? townType, string townTitle, int? streetType, string streetTitle);
    }

    public class AddressISales : IAddressISales
    {
        private readonly Dictionary<string, string> _regions;

        public AddressISales(IRepositoryEntity repository)
        {
            var regions = Task.Run(async () => { return await repository.GetAllAsync<Region>(); }).Result;

            _regions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in regions)
            {
                _regions.Add(item.ExternalCode, item.Title.Trim());
            }
        }

        public AddressISalesInfo GetStructure(string address)
        {
            string[] mass = address.Split('@');

            var info = new AddressISalesInfo();

            info.RegionGuid = mass[0];
            info.CityType = mass[1].ToInt();
            info.CityTitle = mass[2].ToStr();
            info.TownType = mass[3].ToInt();
            info.TownTitle = mass[4].ToStr();
            info.StreetType = mass[5].ToInt();
            info.StreetTitle = mass[6].ToStr();

            return info;
        }

        public string GetAddress(AddressISalesInfo info)
        {
            return GetAddress(info.RegionGuid, info.CityType, info.CityTitle, info.TownType, info.TownTitle, info.StreetType, info.StreetTitle);
        }

        public string GetAddress(string regionGuid, int? cityType, string cityTitle, int? townType, string townTitle, int? streetType, string streetTitle)
        {
            var str = new StringBuilder();

            if (cityType.HasValue)
            {
                str.Append($"{GetTitleForCityType(cityType.Value)} {cityTitle.ToLower().FirstLetterToUpper()}, ");
            }

            if (townType.HasValue)
            {
                str.Append($"{GetTitleForTownType(townType.Value)} {townTitle.ToLower().FirstLetterToUpper()}, ");
            }

            if (streetType.HasValue)
            {
                str.Append($"{GetTitleForStreetType(streetType.Value)} {streetTitle.ToLower().FirstLetterToUpper()}, ");
            }

            str.Append(_regions[regionGuid]);

            return str.ToString();
        }

        private static string GetTitleForCityType(int cityType, bool shortName = true)
        {
            switch (cityType)
            {
                case 0: return shortName ? "а.о." : "автономный округ"; 
                case 1: return shortName ? "р." : "район";
                case 2: return shortName ? "г." : "город"; 
                case 3: return shortName ? "п." : "поселок городского типа";
                default: return string.Empty;
            }
        }

        private object GetTitleForTownType(int townType, bool shortName = true)
        {
            switch (townType)
            {
                case 0: return shortName ? "р." : "район";
                case 1: return shortName ? "г." : "город";
                case 2: return shortName ? "п." : "поселок городского типа";
                case 3: return shortName ? "п." : "рабочий поселок";
                case 4: return shortName ? "п." : "курортный поселок";
                case 5: return shortName ? "к." : "кишлак";
                case 6: return shortName ? "п.с." : "поселковый совет";
                case 7: return shortName ? "сл." : "сельсовет";
                case 8: return shortName ? "с." : "сомон";
                case 9: return shortName ? "в." : "волость";
                case 10: return shortName ? "д.п.с." : "дачный поселковый совет";
                case 11: return shortName ? "п.с.т." : "поселок сельского типа";
                case 12: return shortName ? "н.п." : "населенный пункт";
                case 13: return shortName ? "п." : "поселок при станции";
                case 14: return shortName ? "ст." : "ж.д. станция";
                case 15: return shortName ? "с." : "село";
                case 16: return shortName ? "м." : "местечко";
                case 17: return shortName ? "д." : "деревня";
                case 18: return shortName ? "сл." : "слобода";
                case 19: return shortName ? "ст." : "станция";
                case 20: return shortName ? "станиц." : "станица";
                case 21: return shortName ? "х." : "хутор";
                case 22: return shortName ? "улус" : "улус";
                case 23: return shortName ? "рзд." : "разъезд";
                case 24: return shortName ? "клх." : "колхоз";
                case 25: return shortName ? "свх." : "совхоз";
                case 26: return shortName ? "зим." : "зимовье";
                default: return string.Empty;
            }
        }

        private object GetTitleForStreetType(int streetType, bool shortName = true)
        {
            switch (streetType)
            {
                case 0: return shortName ? "ул." : "улица";
                case 1: return shortName ? "ш." : "шоссе";
                case 2: return shortName ? "пр." : "проезд";
                case 3: return shortName ? "пер." : "переулок";
                case 4: return shortName ? "просп." : "проспект";
                case 5: return shortName ? "лин." : "линия";
                case 6: return shortName ? "наб." : "набережная";
                case 7: return shortName ? "туп." : "тупик";
                case 8: return shortName ? "алл." : "аллея";
                case 9: return shortName ? "бульв." : "бульвар";
                case 10: return shortName ? "вал" : "вал";
                case 11: return shortName ? "дор." : "дорога";
                case 12: return shortName ? "дор." : "дорожка";
                case 13: return shortName ? "пл." : "площадь";
                default: return string.Empty;
            }
        }
    }

    public class AddressISalesInfo
    {
        public string RegionGuid { get; set; }

        public int? CityType { get; set; }

        public string CityTitle { get; set; }

        public int? TownType { get; set; }

        public string TownTitle { get; set; }

        public int? StreetType { get; set; }

        public string StreetTitle { get; set; }
    }
}
