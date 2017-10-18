namespace Trcont.Ris.Domain.Enums
{
    using System.ComponentModel;
    public enum ContainerOwnerEnum
    {
        None = 0,

        [Description("Инвентарный")]
        Inventory = 1,

        [Description("Собственность ТК")]
        Trcont = 2,

        None2 = 3,

        [Description("Других собственников")]
        Other = 4,
    }
}
