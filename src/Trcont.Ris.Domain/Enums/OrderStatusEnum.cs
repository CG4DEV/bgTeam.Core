namespace Trcont.Ris.Domain.Enums
{
    using System.ComponentModel;

    public enum OrderStatusEnum
    {
        [Description("Заявка")]
        Request = 0,

        [Description("Отменен")]
        Canceled = 1,

        [Description("Выполняется")]
        Processing = 2,

        [Description("Архив")]
        Archive = 3
    }
}
