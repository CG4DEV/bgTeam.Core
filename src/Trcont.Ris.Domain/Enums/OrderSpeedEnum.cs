namespace Trcont.Ris.Domain.Enums
{
    using System.ComponentModel;
    public enum OrderSpeedEnum
    {
        [Description("Большая")]
        Large = 0,

        [Description("Грузовая")]
        Freight = 1,

        [Description("Большая")]
        BigLarge = 2,
    }
}
