namespace Trcont.Ris.DataAccess.Order
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using Trcont.Ris.Domain.Dto;
    using System.Linq;

    public class GetOrdersByContractIdCmd : ICommand<GetOrdersByContractIdCmdContext, IEnumerable<OrderDto>>
    {
        private static readonly string[] _orderByFields = new string[]
        {
            "ContractNumber",
            "ContainerTypeId",
            "ContainerTypeTitle",
            "PlaceFromId",
            "PlaceFromTitle",
            "PlaceFromCnsiCode",
            "PlaceFromCnsiGuid",
            "CountryFromId",
            "CountryFromTitle",
            "CountryFromCode",
            "PlaceToId",
            "PlaceToTitle",
            "PlaceToCnsiCode",
            "PlaceToCnsiGuid",
            "CountryToId",
            "CountryToTitle",
            "CountryToCode",
            "EtsngId",
            "EtsngTitle",
            "EtsngCode",
            "GngId",
            "GngTitle",
            "GngCode",
            "Id",
            "IrsGuid",
            "TeoId",
            "TeoGuid",
            "Number",
            "ClientId",
            "ClientName",
            "ClientGuid",
            "ContractId",
            "OrderDate",
            "TrainTypeId",
            "DocumentTitle",
            "StatusId",
            "StatusTitle",
            "ContrRelationsTypeId",
            "PeriodBeginDate",
            "PeriodBeginOffset",
            "PeriodEndDate",
            "PeriodEndOffset",
            "Weight",
            "WeightBrutto",
            "CurrencyId",
            "CurrencyTitle",
            "CurrencyCode",
            "OutCategory",
            "SendTypeId",
            "ContainerQuantity",
            "ContOwner",
            "WagonPark",
            "Speed",
            "CustomType",
            "Summ",
            "SendDate",
            "ArrivalDate",
            "LoadDate"
        };

        private readonly IRepository _repository;

        #region QueryStrings
        private readonly string _defaultSelectBlock = @"SELECT 
    o.Id,
    o.IrsGuid,
    o.TeoId,
    o.TeoGuid,
    o.Number,
    o.ClientId,
    o.ContractId,
    o.OrderDate,
    o.TrainTypeId,
    o.DocumentTitle,
    o.StatusId,
    o.ContrRelationsTypeId,
    o.PeriodBeginDate,
    o.PeriodBeginOffset,
    o.PeriodEndDate,
    o.PeriodEndOffset,
    o.EtsngId,
    o.GngId,
    o.Weight,
    o.WeightBrutto,
    o.CurrencyId,
    o.OutCategory,
    o.SendTypeId,
    o.ContainerQuantity,
    o.ContOwner,
    o.WagonPark,
    o.Speed,
    o.CustomType,
    o.PlaceFromId,
    o.CountryFromId,
    o.PlaceToId,
    o.CountryToId,
    o.CreateDate,
    o.TimeStamp,
    o.TransTypeId,
    tt.Name AS TransTypeTitle,
    ct.RegNumber AS ContractNumber,
    ct1.Id AS ContainerTypeId,
    ct1.Title AS ContainerTypeTitle, 
    p1.Title AS PlaceFromTitle,
    p1.CnsiCode AS PlaceFromCnsiCode,
    p1.CnsiGuid AS PlaceFromCnsiGuid, 
    p2.Title AS PlaceToTitle,
    p2.CnsiCode AS PlaceToCnsiCode,
    p2.CnsiGuid AS PlaceToCnsiGuid, 
    e.Title AS EtsngTitle,
    e.Code AS EtsngCode,
    cr1.Title AS CountryFromTitle,
    cr1.Account AS CountryFromCode,
    cr2.Title AS CountryToTitle,
    cr2.Account AS CountryToCode,
    g.Title AS GngTitle,
    g.Code AS GngCode,
    cl.ClientName,
    cl.ClientGUID,
    c.Title AS CurrencyTitle,
    c.Account AS CurrencyCode,
    os.Name AS StatusTitle,
    CASE
    WHEN o.TeoId IS NULL
        THEN (SELECT SUM(kps.Summ) FROM KPService kps WHERE kps.OrderId = o.Id AND kps.IsActive = 1) 
    ELSE
        (SELECT SUM(os.Summ) FROM OrdersService os WHERE os.OrderId = o.Id AND os.ParentTeoServiceId IS NULL) 
    END as Summ,
    fct.LoadDate AS LoadDate,
    COALESCE(fct.ArrivalDate, fct.PlanArrivalDate) AS ArrivalDate,
    fct.TransDate AS SendDate";
        private readonly string _defaultFromBlock = @"FROM Orders o WITH (NOLOCK)
INNER JOIN Contract ct ON o.ContractId = ct.Id
INNER JOIN OrdersStatus os ON o.StatusId = os.Id
LEFT JOIN vClients cl ON o.ClientId = cl.Id
LEFT JOIN ContainerType ct1 ON o.TrainTypeId = ct1.Id
LEFT JOIN vPoints p1 ON o.PlaceFromId = p1.Id
LEFT JOIN vPoints p2 ON o.PlaceToId = p2.Id
LEFT JOIN Country cr1 ON o.CountryFromId = cr1.Id
LEFT JOIN Country cr2 ON o.CountryToId = cr2.Id
LEFT JOIN Etsng e ON o.EtsngId = e.Id
LEFT JOIN GNG g ON o.GngId = g.Id
LEFT JOIN Currency c ON o.CurrencyId = c.Id
LEFT JOIN TransType tt ON tt.Id = o.TransTypeId
LEFT JOIN 
    (
    SELECT fct1.OrderId, 
        MAX(fct1.LoadDate) AS LoadDate, 
        MAX(fct1.PlanArrivalDate) AS PlanArrivalDate,
        MAX(fct1.TransDate) AS TransDate,
        MAX(fct1.ArrivalDate) AS ArrivalDate
    FROM OrdersFact fct1 
    WHERE fct1.FactSourceId = 304686
    GROUP BY fct1.OrderId) fct ON fct.OrderId = o.TeoId";
        private readonly string _defaultWhereBlock = @"WHERE ct.IrsGuid = @ContractGuid";
        private readonly string _defaultOrderByBlock = @"ORDER BY o.OrderDate DESC";
        private readonly string _pagingBlock = @"OFFSET @First ROWS FETCH NEXT @PageSize ROWS ONLY";
        #endregion

        private StringBuilder _selectBlock;
        private StringBuilder _fromBlock;
        private StringBuilder _whereBlock;
        private StringBuilder _orderByBlock;

        public GetOrdersByContractIdCmd(IRepository repository)
        {
            _repository = repository.CheckNull(nameof(repository));
            Init();
        }

        public static string[] OrderByFields => _orderByFields;

        public string SelectBlock => _selectBlock.ToString();

        public string FromBlock => _fromBlock.ToString();

        public string WhereBlock => _whereBlock.ToString();

        public string OrderByBlock => _orderByBlock.Length == 0 ? _defaultOrderByBlock : _orderByBlock.ToString();

        public string FullQuery
        {
            get
            {
                return $"{SelectBlock}\r\n{FromBlock}\r\n{WhereBlock}\r\n{OrderByBlock}\r\n{_pagingBlock}";
            }
        }

        public static void CheckOrderBy(IEnumerable<string> orderFields)
        {
            if (orderFields != null && orderFields.Any())
            {
                foreach (var field in orderFields)
                {
                    if (!OrderByFields.Any(x => x.Equals(field)))
                    {
                        throw new ArgumentException($"Несуществующее поле для сортировки: {field}");
                    }
                }
            }
        }

        public IEnumerable<OrderDto> Execute(GetOrdersByContractIdCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public Task<IEnumerable<OrderDto>> ExecuteAsync(GetOrdersByContractIdCmdContext context)
        {
            Init();

            if (context.PageOffset < 0
                || context.PageSize < 1)
            {
                throw new ArgumentException($"Ошибка параметров постраничного вывода: значение {nameof(context.PageOffset)} должено быть больше или равно 0," +
                    $" значение {nameof(context.PageSize)} должено быть больше или равно 1");
            }

            UpdateBlocks(context);
            var queryParams = GetQueryParams(context);

            return _repository.GetAllAsync<OrderDto>(FullQuery, queryParams);
        }

        public Dictionary<string, object> GetQueryParams(GetOrdersByContractIdCmdContext context)
        {
            Dictionary<string, object> prms = new Dictionary<string, object>();

            if (context.WhereBlockParams is IDictionary<string, object>)
            {
                var dict = context.WhereBlockParams as IDictionary<string, object>;
                prms = new Dictionary<string, object>(dict);
            }
            else if (!(context.WhereBlockParams is System.Collections.IEnumerable) && !(context.WhereBlockParams is string))
            {
                PropertyInfo[] properties = context.WhereBlockParams.GetType().GetProperties();

                foreach (PropertyInfo prop in properties)
                {
                    prms.Add(prop.Name, prop.GetValue(context.WhereBlockParams));
                }
            }

            if (!prms.ContainsKey("ContractGuid"))
            {
                prms.Add("ContractGuid", context.ContractGuid);
            }

            if (!prms.ContainsKey("First"))
            {
                prms.Add("First", context.PageOffset);
            }

            if (!prms.ContainsKey("PageSize"))
            {
                prms.Add("PageSize", context.PageSize);
            }

            return prms;
        }

        private void UpdateBlocks(GetOrdersByContractIdCmdContext context)
        {
            if (!string.IsNullOrWhiteSpace(context.AddToWhereBlock))
            {
                _whereBlock.AppendLine();
                _whereBlock.Append($"AND ({context.AddToWhereBlock})");
            }

            if (!string.IsNullOrWhiteSpace(context.AddToOrderByBlock))
            {
                if (context.AddToOrderByBlock.TrimStart(' ', '\r', '\n', '\t').StartsWith("ORDER BY", StringComparison.InvariantCultureIgnoreCase))
                {
                    _orderByBlock.AppendLine(context.AddToOrderByBlock);
                }
                else
                {
                    _orderByBlock.AppendFormat("ORDER BY {0}\r\n", context.AddToOrderByBlock);
                }
            }
            else
            {
                _orderByBlock.AppendLine(_defaultOrderByBlock);
            }
        }

        private void Init()
        {
            _selectBlock = new StringBuilder(_defaultSelectBlock);
            _fromBlock = new StringBuilder(_defaultFromBlock);
            _whereBlock = new StringBuilder(_defaultWhereBlock);
            _orderByBlock = new StringBuilder();
        }
    }
}
