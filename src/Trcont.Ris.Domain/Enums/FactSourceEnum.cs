namespace Trcont.Ris.Domain.Enums
{
    public enum FactSourceEnum
    {
        /// <summary>
        /// 04. Железнодорожная накладная
        /// </summary>
        [System.ComponentModel.Description("04. Железнодорожная накладная")]
        SourceNaklId = 304686,

        /// <summary>
        /// _15.1. Наряд КЭУ-16 при отправлении
        /// </summary>
        [System.ComponentModel.Description("_15.1. Наряд КЭУ-16 при отправлении")]
        KEO16OutSourceId = 617940,

        /// <summary>
        /// _15.2. Наряд КЭУ-16 по прибытию
        /// </summary>
        [System.ComponentModel.Description("_15.2. Наряд КЭУ-16 по прибытию")]
        KEO16InSourceId = 617937,

        /// <summary>
        /// 10. КЭУ-16
        /// </summary>
        [System.ComponentModel.Description("10. КЭУ-16")]
        KEU16Id = 58282992,

        /// <summary>
        /// _14.1. Товарно-транспортные накладные по отправлению
        /// </summary>
        [System.ComponentModel.Description("_14.1. Товарно-транспортные накладные по отправлению")]
        TTNOutSourceId = 304670,

        /// <summary>
        /// _14.2. Товарно-транспортные накладные по прибытию
        /// </summary>
        [System.ComponentModel.Description("_14.2. Товарно-транспортные накладные по прибытию")]
        TTNInSourceId = 617945,

        /// <summary>
        /// 06. Транспортная накладная
        /// </summary>
        [System.ComponentModel.Description("06. Транспортная накладная")]
        TTNId = 58282848
    }
}
