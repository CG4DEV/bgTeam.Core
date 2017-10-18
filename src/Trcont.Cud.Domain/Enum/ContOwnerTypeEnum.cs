namespace Trcont.Cud.Domain.Enum
{
    using System.ComponentModel;

    public enum ContOwnerTypeEnum
    {
        [Description("None")]
        None = 0,

        [Description("INV")]
        Inv = 1,

        /// <summary>
        /// Принадлежат ТрансКонтейнеру
        /// </summary>
        [Description("DF2E7381-70FD-4658-9CEC-746C6672A999")]
        TK = 2,

        [Description("SOB")]
        Sob = 4
    }
}
