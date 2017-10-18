namespace Trcont.Ris.Domain.Enums
{
    using System.ComponentModel;

    public enum WagonParkTypeEnum
    {
        [Description("None")]
        None = 0,

        [Description("19ba6c88-83ba-4a71-b116-367b19e9b8ac")]
        INV = 1,

        /// <summary>
        /// Принадлежат ТрансКонтейнеру
        /// </summary>
        [Description("137387f1-9d39-45df-8dd7-a63a0929bb23")]
        TK = 2,
    }
}
