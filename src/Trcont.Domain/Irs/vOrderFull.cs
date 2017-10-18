namespace Trcont.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class vOrderFull
    {
        /// <summary>
        /// ИД заказа
        /// </summary>
        public int ReferenceId { get; set; }

        /// <summary>
        /// Номер заказа
        /// </summary>
        public string ReferenceTitle { get; set; }

        /// <summary>
        /// Код статуса
        /// </summary>
        public int TransState { get; set; }

        /// <summary>
        /// Наименование статуса
        /// </summary>
        public string TransStateTitle { get; set; }

        /// <summary>
        /// ИД договора
        /// </summary>
        public int ClientDogovorId { get; set; }

        /// <summary>
        /// ИД клиента
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// ЕКК клиента
        /// </summary>
        public string CNSIClientEKK { get; set; }

        /// <summary>
        /// ГУИД клиента
        /// </summary>
        public Guid CNSIClientGUID { get; set; }

        /// <summary>
        /// Дата формирования заказа
        /// </summary>
        public DateTime TransDate { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public string ClientDogovorRegNumber { get; set; }

        /// <summary>
        /// ИНН Клиента
        /// </summary>
        public string ClientCodeINN { get; set; }

        /// <summary>
        /// Полное наименование Клиента
        /// </summary>
        public string ClientFullName { get; set; }

        /// <summary>
        /// Краткое наименование Клиента
        /// </summary>
        public string ClientShortName { get; set; }

        public string TransAccount { get; set; }

        /// <summary>
        /// Код станции отправления
        /// </summary>
        public string StationFromCode { get; set; }

        /// <summary>
        /// Наименование станции отправления
        /// </summary>
        public string StationFromTitle { get; set; }

        /// <summary>
        /// Код ЦНСИ станции отправления
        /// </summary>
        public string CNSIStationFromCode { get; set; }

        /// <summary>
        /// Код станции назначения
        /// </summary>
        public string StationToCode { get; set; }

        /// <summary>
        /// Наименование станции назначения
        /// </summary>
        public string StationToTitle { get; set; }

        /// <summary>
        /// Код ЦНСИ станции назначения
        /// </summary>
        public string CNSIStationToCode { get; set; }

        public string StationOutCode { get; set; }

        public string StationOutTitle { get; set; }

        public string CNSIStationOutCode { get; set; }

        public string StationInCode { get; set; }

        public string StationInTitle { get; set; }

        public string CNSIStationInCode { get; set; }

        /// <summary>
        /// Код страны отправления
        /// </summary>
        public string CountryFromCode { get; set; }

        /// <summary>
        /// Наименоание страны отправления
        /// </summary>
        public string CountryFromTitle { get; set; }

        /// <summary>
        /// Код страны назначения
        /// </summary>
        public string CountryToCode { get; set; }

        /// <summary>
        /// Наименоание страны назначения
        /// </summary>
        public string CountryToTitle { get; set; }

        /// <summary>
        /// ГНГ код груза
        /// </summary>
        public string GNGCode { get; set; }

        /// <summary>
        /// ГНГ наименование груза
        /// </summary>
        public string GNGTitle { get; set; }

        /// <summary>
        /// ЕТСНГ код груза
        /// </summary>
        public string ETSNGCode { get; set; }

        /// <summary>
        /// ЕТСНГ наименование груза
        /// </summary>
        public string ETSNGTitle { get; set; }

        /// <summary>
        /// Код типа вагона
        /// </summary>
        public string WagonTypeCode { get; set; }

        /// <summary>
        /// Код типа вагона 2
        /// </summary>
        public string WagonTypeCode2 { get; set; }

        /// <summary>
        /// Наименование типа вагона
        /// </summary>
        public string WagonTypeTitle { get; set; }

        /// <summary>
        /// Дата отгрузки
        /// </summary>
        public DateTime ShipmentStart { get; set; }

        public DateTime DateAvailableBy { get; set; }

        public int WagonPark { get; set; }

        public string WagonParkTitle { get; set; }

        public int KontPark { get; set; }

        public string KontParkTitle { get; set; }

        public int OutCategory { get; set; }

        public string OutCategoryTitle { get; set; }

        public int? WagonQnt { get; set; }

        /// <summary>
        /// Вес груза
        /// </summary>
        public decimal LoadWeight { get; set; }

        /// <summary>
        /// Кол-во контейнеров
        /// </summary>
        public int KontQnt { get; set; }

        public int KontQntDfe { get; set; }

        public string ServiceInDestFlag { get; set; }

        public int OrderType { get; set; }

        public string OrderTypeTitle { get; set; }

        public string SourceRegionId { get; set; }

        public string DestRegionId { get; set; }

        public string Notes { get; set; }

        public int SalesChannel { get; set; }

        public int DfeRate { get; set; }

        public int ReservedDfe { get; set; }

        public int ReservedContainer { get; set; }
    }
}