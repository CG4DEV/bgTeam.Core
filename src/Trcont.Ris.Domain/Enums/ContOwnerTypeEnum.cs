namespace Trcont.Ris.Domain.Enums
{
    using System.ComponentModel;

    public enum ContOwnerTypeEnum
    {
        [Description("None")]
        None = 0,

        [Description("a8a9371a-e5fb-4572-b81d-1b193727a878")]
        INV = 1,

        /// <summary>
        /// Принадлежат ТрансКонтейнеру
        /// </summary>
        [Description("df2e7381-70fd-4658-9cec-746c6672a999")]
        TK = 2,

        [Description("b42ce38c-c854-4cc5-83fa-61a521c0411d")]
        SOB = 4
    }

    //case 1: ipms.valueIrs = "A8A9371A-E5FB-4572-B81D-1B193727A878"; ipms.valueTitle = "1-Инвентарный"; break;
    //case 2: ipms.valueIrs = "DF2E7381-70FD-4658-9CEC-746C6672A999"; ipms.valueTitle = "2-Приватный оператора"; break;
    //case 3: ipms.valueIrs = "4676B10E-C6EC-4CFF-B6A9-4258D5BB7135"; ipms.valueTitle = "3-Арендованный"; break;
    //case 4: ipms.valueIrs = "B42CE38C-C854-4CC5-83FA-61A521C0411D"; ipms.valueTitle = "4-Приватный не оператора"; break;

}
