namespace Trcont.Domain.Common
{
    using System.ComponentModel;

    public enum ServiceTypeEnum
    {
        [Description("Обязательная")]
        Mandatory = 1,

        [Description("Рекомендуемая")]
        Recomended = 2,

        [Description("Дополнительная")]
        Extra = 0,
    }
}
