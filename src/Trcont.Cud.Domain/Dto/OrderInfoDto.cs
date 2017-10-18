namespace Trcont.Cud.Domain.Dto
{
    using bgTeam.Extensions;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Trcont.Cud.Domain.Common;
    using Trcont.Cud.Domain.Enum;

    public class OrderInfoDto : ITransPicOrder
    {
        public Guid ReferenceGuid { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Название документа с указанием маршрута
        /// </summary>
        public string DocumentTitle { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public OrderStatusEnum Status { get; set; }

        /// <summary>
        /// Наименование статуса
        /// </summary>
        public string StatusTitle => Status.GetDescription();

        /// <summary>
        /// Валюта оценки стоимости
        /// </summary>
        public Guid CurrencyGuid { get; set; }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public Guid ClientGuid { get; set; }

        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public Guid ContractGuid { get; set; }

        /// <summary>
        /// Тип договорных отношений
        /// </summary>
        public ContrRelationsTypeEnum ContrRelationsType { get; set; }

        public string ContrRelationsTypeTitle => ContrRelationsType.GetDescription();

        /// <summary>
        /// Тип отправки
        /// </summary>
        public ContrOutCategoryEnum OutCategory { get; set; }

        /// <summary>
        /// Признак отправки
        /// </summary>
        public Guid SendingGuid { get; set; }

        /// <summary>
        /// Тип контейнера
        /// </summary>
        public Guid TrainTypeGuid { get; set; }

        /// <summary>
        /// Количество контейнеров
        /// </summary>
        public int ContainerQuantity { get; set; }

        /// <summary>
        /// Принадлежность контейнера
        /// </summary>
        public ContainerOwnerEnum ContOwner { get; set; }

        /// <summary>
        /// Принадлежность вагона
        /// </summary>
        public ContainerOwnerEnum WagonPark { get; set; }

        /// <summary>
        /// Скорость
        /// </summary>
        public OrderSpeedEnum Speed { get; set; }

        /// <summary>
        /// Таможенный режим
        /// </summary>
        public CustomTypeEnum CustomType { get; set; }

        /// <summary>
        /// Период начала исполнения заявки
        /// </summary>
        public DateTime PeriodBeginDate { get; set; }

        /// <summary>
        /// Период окончания исполнения заявки
        /// </summary>
        public DateTime PeriodEndDate { get; set; }
        /// <summary>
        /// Пункт отправления
        /// </summary>
        public Guid PlaceFromGuid { get; set; }

        /// <summary>
        /// Страна отправления
        /// </summary>
        public Guid CountryFromGuid { get; set; }

        /// <summary>
        /// Пункт назначения
        /// </summary>
        public Guid PlaceToGuid { get; set; }

        /// <summary>
        /// Страна назначения
        /// </summary>
        public Guid CountryToGuid { get; set; }

        /// <summary>
        /// ЕТСНГ
        /// </summary>
        public Guid ETSNGGuid { get; set; }

        /// <summary>
        /// ГНГ
        /// </summary>
        public Guid GNGGuid { get; set; }

        /// <summary>
        /// Вес груза (т) в ваг./конт
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// Общая сумма НДС
        /// </summary>
        public decimal Summ { get; set; }

        public bool IsTeo { get; set; }

        /// <summary>
        /// Вес брутто груза контейнера
        /// </summary>
        public float WeightBrutto { get; set; }

        /// <summary>
        /// Предполагаемый день прибытия
        /// </summary>
        public DateTime? ArrivalDate { get; set; }

        /// <summary>
        /// День отправления
        /// </summary>
        public DateTime? SendDate { get; set; }

        public string TransPic { get; set; }

        // Поля для строковых значений

        public string PlaceFromStr { get; set; }

        public string PlaceToStr { get; set; }

        public string CountryFromStr { get; set; }

        public string CountryToStr { get; set; }

        public string CurrencyStr { get; set; }

        public string ETSNGStr { get; set; }

        public string GNGStr { get; set; }

        public string SendingStr { get; set; }

        public string TrainTypeStr { get; set; }

        public Guid? PlaceFromCNSIGuid { get; set; }

        public Guid? PlaceToCNSIGuid { get; set; }

        public Guid? CountryFromCNSIGuid { get; set; }

        public Guid? CountryToCNSIGuid { get; set; }

        public Guid? CurrencyCNSIGuid { get; set; }

        public Guid? ETSNGCNSIGuid { get; set; }

        public Guid? GNGCNSIGuid { get; set; }

        public Guid? SendingCNSIGuid { get; set; }

        public Guid? TrainTypeCNSIGuid { get; set; }

        public PointTypeEnum PlaceFromPointType { get; set; }

        public PointTypeEnum PlaceToPointType { get; set; }

        public IEnumerable<IRoute> Routes { get; set; }
    }

    public enum OrderStatusEnum
    {
        [Description("Нет")]
        None = 0,

        [Description("Запрос ставки")]
        RateRequest = 1,

        [Description("Рассчитано")]
        Calculated = 2,

        [Description("Отклонено")]
        Rejected = 3,

        [Description("Рассчитано, но не проверено")]
        CalculatedNotVerified = 4,

        [Description("Расчет завершен")]
        CalculatedCompleted = 5,

        [Description("Редактирование")]
        TeoEdit = 100,

        [Description("На подписи")]
        TeoSignature = 101,

        [Description("На согласовании")]
        TeoUponAgreement = 102,

        [Description("Согласовано")]
        TeoAgreed = 103,

        [Description("Отклонено")]
        TeoDisapproved = 104,

        [Description("Частично согласовано")]
        TeoUponAgreementPart = 105,
    }

    public enum ContrOutCategoryEnum
    {
        [Description("Вагонная")]
        Coach = 0,

        [Description("Контейнерная")]
        Container = 1,
    }

    public enum ContainerOwnerEnum
    {
        None = 0,

        [Description("Инвентарный")]
        Inventory = 1,

        [Description("Собственность ТК")]
        Trcont = 2,

        None2 = 3,

        [Description("Других собственников")]
        Other = 4,
    }

    public enum OrderSpeedEnum
    {
        [Description("Большая")]
        Large = 0,

        [Description("Грузовая")]
        Freight = 1,

        [Description("Большая")]
        BigLarge = 2,
    }

    public enum CustomTypeEnum
    {
        [Description("Перевозка не под таможенным режимом")]
        NotUnderCustom = 0,

        [Description("Перевозка под таможенным режимом")]
        UnderCustom = 1,
    }

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
