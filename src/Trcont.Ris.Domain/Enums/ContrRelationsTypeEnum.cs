namespace Trcont.Ris.Domain.Enums
{
    using System.ComponentModel;

    public enum ContrRelationsTypeEnum
    {
        [Description("Комплекс")]
        Complex = 0,

        [Description("Комплекс по плечам")]
        ComplexByArms = 1,

        [Description("Доп. услуги")]
        AdditionalServices = 2,
    }
}
