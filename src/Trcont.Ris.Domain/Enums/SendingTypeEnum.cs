namespace Trcont.Ris.Domain.Enums
{
    using System.ComponentModel;

    public enum SendingTypeEnum
    {
        [Description("None")]
        None = 0,

        /// <summary>
        /// 01.Одиночные контейнеры (а также спец. контейнеры мелкой отправкой)
        /// </summary>
        [Description("391b2ff2-20a3-4939-bc69-39f7e1d9fff6")]
        Type1 = 6,
        
        /// <summary>
        /// 04.Порожние контейнеры (а также порожние спец. контейнеры мелкой отправкой)
        /// </summary>
        [Description("0d45609a-ef92-4104-aa68-09efc8adb292")]
        Type4 = 102,

        ///// <summary>
        ///// 02.Сборная контейнерная
        ///// </summary>
        //[Description("c0ca75e1-ddb7-40dd-a1f8-4f6b64b8bbe2")]
        //Type2 = 2,

        /// <summary>
        /// 03.Комплектами на вагон (а также спец. контейнеры повагонной отправкой)
        /// </summary>
        [Description("80e0bd9b-e9ac-49e0-907c-9e9606d46ec7")]
        Type3 = 15,

        /// <summary>
        /// 05.Порожние контейнеры комплектами на вагон (а также порожние спец. контейнеры повагонной отправкой)
        /// </summary>
        [Description("858e271c-ecea-4a5b-a0b1-9b9825c08390")]
        Type5 = 103,

        ///// <summary>
        ///// 06.В универсальных вагонах
        ///// </summary>
        //[Description("46470ebc-0e07-4d0f-8443-0adaaf61fca8")]
        //Type6 = 6,

        ///// <summary>
        ///// 07.В специализированных вагонах
        ///// </summary>
        //[Description("5cf819e5-a2de-4c64-9048-daf1871a0444")]
        //Type7 = 7,

        ///// <summary>
        ///// 08.В изотермическом подвижном составе
        ///// </summary>
        //[Description("83a588e2-9552-45cb-816b-88fa1eb39965")]
        //Type8 = 8,

        ///// <summary>
        ///// 09.В цистернах
        ///// </summary>
        //[Description("e39f18cc-c828-4d38-90f1-825ec419120f")]
        //Type9 = 9,

        ///// <summary>
        ///// 10.На сцепах платформ и транспортёрах (а также негабаритные грузы)
        ///// </summary>
        //[Description("fa80f307-dbde-4f56-b4e4-4e511f916047")]
        //Type10 = 10,

        ///// <summary>
        ///// 11.Сборная вагонная
        ///// </summary>
        //[Description("01b416ac-285d-41ba-8925-7bef4c485861")]
        //Type11 = 11,

        ///// <summary>
        ///// 12.В багажных и пассажирских вагонах в составе грузовых поездов
        ///// </summary>
        //[Description("733a8319-74ea-4941-b6e6-0ef28fdb7409")]
        //Type12 = 12,

        ///// <summary>
        ///// 13.Порожние вагоны под погрузку/из-под выгрузки
        ///// </summary>
        //[Description("2063925e-c86e-4218-be54-ab8654af568a")]
        //Type13 = 13,

        ///// <summary>
        ///// 14.Подсыл (возврат) транспортера не на ж/д собственницу
        ///// </summary>
        //[Description("2732a3df-e108-4b44-ac9d-7ff8cc6c469e")]
        //Type14 = 14,

        ///// <summary>
        ///// 15.Вагоны, локомотивы и др. передвижное оборудование (в/из ремонта), перевозимое как груз на своих осях
        ///// </summary>
        //[Description("a02fdc4e-d0b9-42c6-a897-6f6996d87576")]
        //Type15 = 15,

        ///// <summary>
        ///// 16.Спец. передвижные формирования, не входящие в систему РЖД, осуществляющие строительство на РЖД
        ///// </summary>
        //[Description("24ab917d-a552-49d5-80a5-5cc95c8607e3")]
        //Type16 = 16,

        ///// <summary>
        ///// 17.Спец передвижные формирования ГАЖК и Ассоциации "Узбектрансстрой"
        ///// </summary>
        //[Description("e5e1b586-a52b-4d81-beb6-3ce08b97c1b7")]
        //Type17 = 17,

        ///// <summary>
        ///// 18.Спец. передвижные формирования, не входящие в систему БЧ, осуществляющие строительство на БЧ
        ///// </summary>
        //[Description("23dac98e-11bb-4fe5-8bbf-7d0a3c4793b1")]
        //Type18 = 18,

        ///// <summary>
        ///// 19.Пробег локомотивов в "холодном" состоянии
        ///// </summary>
        //[Description("d659f991-6367-46b1-bf4c-786d1f215948")]
        //Type19 = 19,

        ///// <summary>
        ///// 20.Пробег отдельных локомотивов
        ///// </summary>
        //[Description("710fea1a-175f-40a5-b172-8ac4485f79f8")]
        //Type20 = 20,

        ///// <summary>
        ///// 21.Порожние вагоны с локомотивом
        ///// </summary>
        //[Description("be7cd85f-5177-4cdb-88d3-fb3360635cfc")]
        //Type21 = 21,

        ///// <summary>
        ///// В сборном вагоне
        ///// </summary>
        //[Description("10ee55f9-4c24-4b2c-8771-eb564fe182fc")]
        //Type22 = 22,

        ///// <summary>
        ///// Вагон прикрытия (повагонной отправкой)
        ///// </summary>
        //[Description("07d3c7e9-c3f9-45aa-a9d3-97b81afe5997")]
        //Type23 = 23,

        ///// <summary>
        ///// Гробы с покойниками и урны с пеплом (повагонной отправкой)
        ///// </summary>
        //[Description("06a1abbe-ba62-4525-856e-65264acc65b6")]
        //Type24 = 24,

        ///// <summary>
        ///// Гружёные вагоны с локомотивом
        ///// </summary>
        //[Description("40c13fa5-a710-4a58-89e3-ccad8a04e3bf")]
        //Type25 = 25,

        ///// <summary>
        ///// Груженые контрейлеры
        ///// </summary>
        //[Description("1ddb93bc-d5ca-490d-ad37-969124d013ba")]
        //Type26 = 26,

        ///// <summary>
        ///// Перевозка контрейлеров
        ///// </summary>
        //[Description("416ab84c-da7e-425e-a83f-849c1cd8c571")]
        //Type27 = 27,

        ///// <summary>
        ///// Порожние контрейлеры
        ///// </summary>
        //[Description("6bf7b94f-730e-4363-9e30-5a8a1ef685a8")]
        //Type28 = 28,

        ///// <summary>
        ///// Пробег локомотива в одиночном следовании
        ///// </summary>
        //[Description("680f51ba-9245-40ae-8610-115492efb40a")]
        //Type29 = 29,

        ///// <summary>
        ///// Проезд проводников
        ///// </summary>
        //[Description("5b92b4c1-908a-4112-ac2a-ec0109285edd")]
        //Type30 = 30,

        ///// <summary>
        ///// Урны с пеплом (мелкой отправкой)
        ///// </summary>
        //[Description("70a3f535-c94d-4915-9f50-22e96246bc09")]
        //Type31 = 31
    }
}
