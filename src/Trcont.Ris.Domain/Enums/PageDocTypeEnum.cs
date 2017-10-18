namespace Trcont.Ris.Domain.Enums
{
    using System.ComponentModel;

    public enum PageDocTypeEnum
    {
        [Description("Неизвестный")]
        None = 0,

        [Description("Документы")]
        Documents = 1,

        [Description("Счета")]
        Bills = 2,

        [Description("Инструкции")]
        Instractions = 3,

        [Description("Доп. соглашения")]
        Additional = 4
    }
}
