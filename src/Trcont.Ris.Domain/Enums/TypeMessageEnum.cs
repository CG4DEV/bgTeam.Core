namespace Trcont.Ris.Domain.Enums
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Руские названия из за того что так приходит от ФОРТ
    /// </summary>
    public enum TypeMessageEnum
    {
        [Description("107383")]
        None = 0,

        [Description("13")]
        Импорт,

        [Description("27061")]
        Экспорт,

        [Description("27167")]
        Транзит,

        [Description("27060")]
        Внутрироссийская,

        [Description("38767959")]
        Внутригосударственная,

        [Description("58491295")]
        Межгосударственная,
    }
}
