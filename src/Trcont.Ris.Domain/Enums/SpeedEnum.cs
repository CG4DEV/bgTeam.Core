namespace Trcont.Ris.Domain.Enums
{
    using System.ComponentModel;

    public enum SpeedEnum
    {
        [Description("None")]
        None = 0,

        /// <summary>
        /// Большая
        /// </summary>
        [Description("0")]
        Big = 1,

        /// <summary>
        /// Грузовая
        /// </summary>
        [Description("1")]
        Cargo = 2
    }
}
