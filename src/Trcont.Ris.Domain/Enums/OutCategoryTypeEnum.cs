namespace Trcont.Ris.Domain.Enums
{
    using System.ComponentModel;

    public enum OutCategoryTypeEnum
    {
        [Description("None")]
        None = 0,

        /// <summary>
        /// Вагонная
        /// </summary>
        [Description("0")]
        Wagon = 1000,

        /// <summary>
        /// Контейнерная
        /// </summary>
        [Description("1")]
        Container = 3000
    }
}
