namespace Trcont.Ris.Domain.Enums
{
    using System.ComponentModel;

    public enum CustomTypeEnum
    {
        [Description("Перевозка не под таможенным режимом")]
        NotUnderCustom = 0,

        [Description("Перевозка под таможенным режимом")]
        UnderCustom = 1,
    }
}
