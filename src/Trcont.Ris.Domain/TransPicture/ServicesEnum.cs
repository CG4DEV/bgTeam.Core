namespace Trcont.Ris.Domain.TransPicture
{
    using System.ComponentModel;

    public enum ServicesEnum
    {
        // old service begin

        /// <summary>
        /// 2.08.03. Комплексная транспортно-экспедиционная услуга по организации перевозки грузов в ПСЖВС
        /// </summary>
        [Description("1fee47c6-5448-4e6d-830b-66aea4663509")]
        Service283,

        /// <summary>
        /// 1.03.01.01. Перевозка грузов во всех видах железнодорожных сообщений (провозная плата по странам СНГ, третьим странам, ЖДЯ, ЯЖДК, Крымской ж.д.)
        /// </summary>
        [Description("7b24839f-a881-42f8-b5eb-ee5ab545ece5")]
        Service131,

        // old service end

        /// <summary>
        /// 1.02.01. Организация перевозки контейнеров/грузов железнодорожным транспортом
        /// </summary>
        [Description("68413a87-4ed6-4e59-bef8-1b426dc64dad")]
        TrainService,

        /// <summary>
        /// 1.02.02. Организация перевозки контейнеров/грузов морским (речным) транспортом
        /// </summary>
        [Description("e3ccfbbc-1b1b-490a-8bb1-422504e7a1aa")]
        ShippingService,

        /// <summary>
        /// 1.02.03. Организация перевозки контейнеров/грузов автомобильным транспортом
        /// </summary>
        [Description("f2a28f09-fb14-4d66-9011-bac47fcea97c")]
        AutoService
    }
}
