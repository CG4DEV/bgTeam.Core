namespace Trcont.Ris.Domain.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public enum RouteTypeEnum
    {
        None = 0,

        /// <summary>
        /// Авто
        /// </summary>
        TL = 1,

        /// <summary>
        /// Терминальное плечо, либо п/п
        /// </summary>
        INTRMDL = 2,

        /// <summary>
        ///  ЖД
        /// </summary>
        RAIL = 3,

        /// <summary>
        /// Море
        /// </summary>
        VESSEL = 4,

        /// <summary>
        /// Перевал
        /// </summary>
        JUNCTION = 5,
    }
}
