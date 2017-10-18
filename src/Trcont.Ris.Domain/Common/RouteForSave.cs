namespace Trcont.Ris.Domain.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Enums;

    public class RouteForSave //: IRoute
    {
        public Guid? FromPointGuid { get; set; }

        //public Guid? FromPointGuidCNSI { get; set; }

        public string FromPointTitle { get; set; }

        public string FromPointCode { get; set; }


        public Guid? ToPointGuid { get; set; }

        //public Guid? ToPointGuidCNSI { get; set; }

        public string ToPointTitle { get; set; }

        public string ToPointCode { get; set; }

        public Guid? PlaceRenderGuid { get; set; }


        public string FromPointCnsi { get; set; }

        public string ToPointCnsi { get; set; }

        public int FromPointId { get; set; }

        public int ToPointId { get; set; }


        public string PlaceRenderCnsi { get; set; }

        public string PointNowCnsi { get; set; }



        public int PayAccountId { get; set; }

        public IList<RouteService> Services { get; set; }

        public RouteTypeEnum RouteType { get; set; }

        // Параметры звена


        ///// <summary>
        ///// Пункт оказания
        ///// </summary>
        //[RouteServiceParams(1, "0000004a-0000-0000-0000-000000000000")]
        //public Guid? PointFrom { get; set; }

        ///// <summary>
        ///// Пункт оказания
        ///// </summary>
        //[RouteServiceParams(2, "0000004b-0000-0000-0000-000000000000")]
        //public Guid? PointTo { get; set; }

        /// <summary>
        /// Пункт оказания
        /// </summary>
        [RouteServiceParams(3, "0000004c-0000-0000-0000-000000000000")]
        public Guid? PointNow { get; set; }

        /// <summary>
        /// Тарифная зона
        /// </summary>
        [RouteServiceParams(5, "0000004e-0000-0000-0000-000000000000")]
        public Guid? TariffZone { get; set; }

        ///// <summary>
        ///// Зона назначения
        ///// </summary>
        //[RouteServiceParams(6, "0000004f-0000-0000-0000-000000000000")]
        //public Guid? DestinationZone { get; set; }

        /// <summary>
        /// Грузоотправитель
        /// </summary>
        [RouteServiceParams(13, "0ec279bf-7476-4c6a-99c8-9eaac4cebc59")]
        public Guid? SenderGuid { get; set; }

        //public Guid? SenderGuidCNSI { get; set; }

        /// <summary>
        /// Грузополучатель
        /// </summary>
        [RouteServiceParams(14, "f512fdc1-d654-4d44-a412-e2798cb88a54")]
        public Guid? ReceiverGuid { get; set; }

        //public Guid? ReceiverGuidCNSI { get; set; }

        /// <summary>
        /// Адрес склада
        /// </summary>
        [RouteServiceParams(15, "0000005b-0000-0000-0000-000000000000")]
        public string AddressWarehouse { get; set; }

        /// <summary>
        /// Соисполнитель/агент в порту
        /// </summary>
        [RouteServiceParams(17, "00000059-0000-0000-0000-000000000000")]
        public Guid? AgentPortFrom { get; set; }

        //public Guid? AgentPortFromCNSI { get; set; }

        ///// <summary>
        ///// Агент в порту выгрузки
        ///// </summary>
        //[RouteServiceParams(18, "0000005a-0000-0000-0000-000000000000")]
        //public Guid? AgentPortTo { get; set; }

        ///// <summary>
        ///// Парк вагона
        ///// </summary>
        //[RouteServiceParams(19, "57fc7e0a-ca2c-4484-80ca-b0cae8fb3224")]
        //public Guid? WagonPark { get; set; }

        /// <summary>
        /// Ранг отправки
        /// </summary>
        [RouteServiceParams(20, "4fc42115-25b9-4a08-b6a7-d5a7d140ca00")]
        public Guid? DeliveryGrade { get; set; }

        // public Guid? AgentPortToCNSI { get; set; }

        /// <summary>
        /// Доп. условия автоперевозки по отправлению
        /// </summary>
        [RouteServiceParams(27, "3d08f9b5-5122-4562-aa1e-1f019287c80e")]
        public Guid? ConditionsTruckingFrom { get; set; }

        ///// <summary>
        ///// Доп. условия автоперевозки по назначению
        ///// </summary>
        //[RouteServiceParams(28, "0a7d3e83-ea96-4c01-87b8-e6f96f434a84")]
        //public Guid? ConditionsTruckingTo { get; set; }

        /// <summary>
        /// Тип перевозимого груза
        /// </summary>
        [RouteServiceParams(29, "3f08484a-4697-420f-86aa-7fa610a19514")]
        public Guid? TypeCargoTransported { get; set; }

        ///// <summary>
        ///// Таможенный контроль
        ///// </summary>
        //[RouteServiceParams(30, "b2990f14-088c-41ae-8880-2a9d104f6aca")]
        //public Guid? CustomsControl { get; set; }

        ///// <summary>
        ///// Парк контейнера
        ///// </summary>
        //[RouteServiceParams(64, "96cefd54-0f1e-4cf4-8930-dd8ecf3f75ba")]
        //public Guid? ContOwner { get; set; }

        /// <summary>
        /// Способ подкл к электросети
        /// </summary>
        [RouteServiceParams(66, "3fd3ff32-5d0c-44c2-9e6e-975f1db73ae5")]
        public Guid? ElectricalСonnection { get; set; }

        ///// <summary>
        ///// Масса брутто
        ///// </summary>
        //[RouteServiceParams(67, "aa91460a-248c-4952-8106-8ac820724bd3")]
        //public Guid? GrossWeight { get; set; }

        ///// <summary>
        ///// Доп. условия
        ///// </summary>
        //[RouteServiceParams(68, "80139855-e11a-4028-b453-84195abffa8f")]
        //public Guid? AdditionalConditions { get; set; }

        /// <summary>
        /// Доп. условие терминального обслуживания по отправлению
        /// </summary>
        [RouteServiceParams(69, "84d57240-fcf9-4fd2-99e5-6329ab6bf1b3")]
        public Guid? ConditionsTerminalServiceFrom { get; set; }

        ///// <summary>
        ///// Доп. условие терминального обслуживания по назначению
        ///// </summary>
        //[RouteServiceParams(70, "0070fa5e-f08f-4d22-bebf-3db7ffe895a5")]
        //public Guid? ConditionsTerminalServiceTo { get; set; }

        /// <summary>
        /// Состояние контейнера
        /// </summary>
        [RouteServiceParams(71, "ee5f614d-dc16-4235-9bee-984702f68ce3")]
        public Guid? ContainerStatus { get; set; }

        ///// <summary>
        ///// Род груза
        ///// </summary>
        //[RouteServiceParams(72, "ace63236-01dc-4bac-b889-15527fd62776")]
        //public Guid? TypeCargo { get; set; }

        /// <summary>
        /// Территория
        /// </summary>
        [RouteServiceParams(73, "fa738a7d-6c14-4013-957a-5c1ad228f4a8")]
        public Guid? Territory { get; set; }

        ///// <summary>
        ///// Вид сообщения
        ///// </summary>
        //[RouteServiceParams(74, "f243b9e8-fc7e-45a4-a2eb-4f393fc5369e")]
        //public Guid? TypeMessage { get; set; }

        ///// <summary>
        ///// Способ доставки
        ///// </summary>
        //[RouteServiceParams(75, "e16d11e1-cafb-4c95-998e-54b539d2e8b5")]
        //public Guid? DeliveryMethod { get; set; }

        /// <summary>
        /// Тип ЗПУ
        /// </summary>
        [RouteServiceParams(76, "8b17f463-6266-47fe-a354-6398c54e284a")]
        public Guid? TypeZPU { get; set; }

        ///// <summary>
        ///// Депо
        ///// </summary>
        //[RouteServiceParams(77, "59a9b548-bdff-4d35-a945-10193e024960")]
        //public Guid? Depot { get; set; }

        ///// <summary>
        ///// Контейнерный поезд
        ///// </summary>
        //[RouteServiceParams(79, "f60cc8eb-79ae-47b1-97d6-50d92d9d571b")]
        //public Guid? ContainerTrain { get; set; }

        ///// <summary>
        ///// Тип груза, степень требуемой охраны груза
        ///// </summary>
        //[RouteServiceParams(80, "80ff1b29-c720-48ae-9c7b-91df3b213efd")]
        //public Guid? SecurityCargo { get; set; }

        ///// <summary>
        ///// Ед. Измерения, типы ставок услуг
        ///// </summary>
        //[RouteServiceParams(81, "69cd13d5-4d8e-4fff-9ce7-984aa10b2463")]
        //public Guid? TypeServices { get; set; }

        ///// <summary>
        ///// Тип утепления
        ///// </summary>
        //[RouteServiceParams(82, "63b35be2-e4d3-4bc2-8260-b6caf82145b3")]
        //public Guid? TypeInsulation { get; set; }

        ///// <summary>
        ///// Вариант подъема
        ///// </summary>
        //[RouteServiceParams(83, "992ffba3-6734-4c7a-be21-2cd9722a7055")]
        //public Guid? LiftOption { get; set; }

        /// <summary>
        /// Вид графического документа
        /// </summary>
        [RouteServiceParams(84, "610ee59b-34ad-47fe-b7ba-9008979c3caa")]
        public Guid? TypeGraphicDocument { get; set; }

        ///// <summary>
        ///// Код груза ЕТСНГ
        ///// </summary>
        //[RouteServiceParams(85, "a6b2e88a-c07b-4a47-865b-c5bfee079861")]
        //public Guid? Etsng { get; set; }

        ///// <summary>
        ///// Вид отправки
        ///// </summary>
        //[RouteServiceParams(123, "712cf5c2-a4a5-4e99-9324-166922cb0340")]
        //public Guid? TypeDispatch { get; set; }

        /// <summary>
        /// Опасность
        /// </summary>
        [RouteServiceParams(124, "327068bc-ba22-49fd-9dd5-71b1b7ac2d2f")]
        public Guid? Danger { get; set; }

        /// <summary>
        /// Тип работ
        /// </summary>
        [RouteServiceParams(126, "e2bd8c53-9978-4e87-86e7-ddaa2138c4f5")]
        public Guid? TypeWork { get; set; }

        ///// <summary>
        ///// Признак отправления / прибытия
        ///// </summary>
        //[RouteServiceParams(127, "d58757af-47da-4195-b850-cf6dd5549cb6")]
        //public Guid? SignDepartureArrival { get; set; }

        /// <summary>
        /// Тип комплекса
        /// </summary>
        [RouteServiceParams(128, "f003be0d-8054-44bf-a4ec-0779a057b455")]
        public Guid? TypeComplex { get; set; }

        ///// <summary>
        ///// Терминал оказания услуги
        ///// </summary>
        //[RouteServiceParams(129, "fb0c9f53-2084-4b17-86e1-39022c8dd44a")]
        //public Guid? ServiceTerminal { get; set; }

        /// <summary>
        /// Доп. условие взвешивания
        /// </summary>
        [RouteServiceParams(132, "e20db8a2-6d43-4e05-a558-da524b1a68e3")]
        public Guid? ExtrasWeighingCondition { get; set; }

        /// <summary>
        /// Тип работ дооборудования
        /// </summary>
        [RouteServiceParams(133, "5006a99c-da18-40e3-b600-afa7f81b7f25")]
        public Guid? TypeWorkAddEquipment { get; set; }

        /// <summary>
        /// Дополнительное условие
        /// </summary>
        [RouteServiceParams(140, "6b7ab332-8fdd-4f44-9af3-260a3b75ce53")]
        public Guid? AddCondition { get; set; }

        /// <summary>
        /// Количество часов
        /// </summary>
        [RouteServiceParams(141, "69e1312c-46ed-4467-9885-f8b1a6749e26")]
        public Guid? HoursCount { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RouteServiceParamsAttribute : Attribute
    {
        public int Id { get; set; }

        public Guid AttGuid { get; set; }

        public RouteServiceParamsAttribute(int id, string attGuid)
        {
            Id = id;
            AttGuid = new Guid(attGuid);
        }
    }

    public class RouteServiceParamList
    {
        public Dictionary<int, RouteServiceParamItem> RouteServiceParam { get; set; }

        public RouteServiceParamList()
        {
            RouteServiceParam = new Dictionary<int, RouteServiceParamItem>();

            var paramList = typeof(RouteForSave).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var item in paramList)
            {
                if (Attribute.IsDefined(item, typeof(RouteServiceParamsAttribute)))
                {
                    var attr = item.GetCustomAttribute<RouteServiceParamsAttribute>();

                    RouteServiceParam.Add(attr.Id, new RouteServiceParamItem(attr.Id, attr.AttGuid, item));
                }
            }
        }
    }

    public class RouteServiceParamItem
    {
        public int Id { get; set; }

        public Guid AttGuid { get; set; }

        public PropertyInfo Property { get; set; }

        public RouteServiceParamItem(int id, Guid attGuid, PropertyInfo property)
        {
            Id = id;
            AttGuid = attGuid;
            Property = property;
        }
    }
}
