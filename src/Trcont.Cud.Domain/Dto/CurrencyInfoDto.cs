namespace Trcont.Cud.Domain.Dto
{
    using System;

    public class CurrencyInfoDto
    {
        public Guid ReferenceGuid { get; set; }

        public int IntCurrencyCode
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CurrencyCode))
                {
                    return 0;
                }

                return int.Parse(CurrencyCode);
            }
        }

        public string CurrencyCode { get; set; }

        public string CurrencyTitle { get; set; }
    }
}
