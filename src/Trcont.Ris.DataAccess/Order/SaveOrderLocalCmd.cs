namespace Trcont.Ris.DataAccess.Order
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using Dapper;
    using DapperExtensions.Attributes;
    using DapperExtensions.Mapper;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Domain.Enums;

    public class SaveOrderLocalCmd : ICommand<SaveOrderLocalCmdContext, bool>
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IRepositoryEntity _repositoryEn;
        private readonly IRepository _repository;
        private readonly Hashtable _cashList;
        private readonly IMapperBase _mapper;

        public SaveOrderLocalCmd(
            IConnectionFactory connectionFactory,
            IRepositoryEntity repositoryEn,
            IRepository repository,
            IMapperBase mapper,
            Hashtable cashList)
        {
            _connectionFactory = connectionFactory;
            _repositoryEn = repositoryEn;
            _repository = repository;
            _mapper = mapper;
            _cashList = cashList;
        }

        public SaveOrderLocalCmd(
            IConnectionFactory connectionFactory,
            IRepositoryEntity repositoryEn,
            IRepository repository,
            IMapperBase mapper)
                : this(connectionFactory, repositoryEn, repository, mapper, new Hashtable())
        {
        }

        public bool Execute(SaveOrderLocalCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<bool> ExecuteAsync(SaveOrderLocalCmdContext context)
        {
            context.Order.CheckNull(nameof(context.Order));
            context.OrderInfo.CheckNull(nameof(context.OrderInfo));

            var localOrder = _mapper.Map(context.Order, new Order());
            localOrder = _mapper.Map(context.OrderInfo, localOrder);

            localOrder.ClientId = await GetEnityIdByIrsGuidAsync(context.Order.ClientGuid, "vClients", "ClientGUID");
            localOrder.ContractId = await GetEnityIdByIrsGuidAsync(context.Order.ContractGuid, "Contract");
            localOrder.CountryFromId = await GetEnityIdByIrsGuidAsync(context.Order.CountryFromGuid, "Country");
            localOrder.CountryToId = await GetEnityIdByIrsGuidAsync(context.Order.CountryToGuid, "Country");
            localOrder.EtsngId = await GetEnityIdByIrsGuidAsync(context.Order.EtsngGuid, "Etsng");
            localOrder.GngId = await GetEnityIdByIrsGuidAsync(context.Order.GngGuid, "GNG");
            localOrder.PlaceFromId = await GetEnityIdByIrsGuidAsync(context.Order.StationFromGuid, "vPoints");
            localOrder.PlaceToId = await GetEnityIdByIrsGuidAsync(context.Order.StationToGuid, "vPoints");
            localOrder.TrainTypeId = await GetEnityIdByIrsGuidAsync(context.Order.TrainTypeGuid, "ContainerType");
            localOrder.SendTypeId = GetEnumValueByDescription<SendingTypeEnum>(context.Order.SendingGuid.ToString());
            localOrder.DocumentTitle = await GetDocumentTitleAsync(context.OrderRoutes);

            //localOrder.CustomType = ???
            localOrder.CreateDate = DateTime.Now;
            localOrder.TimeStamp = DateTime.Now;
            localOrder.OrderDate = DateTime.Now;

            bool isSuccess = false;
            using (var connection = await _connectionFactory.CreateAsync())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await _repositoryEn.InsertAcync(localOrder, connection, transaction);
                        await SaveOrUpdateCollectionAsync(context.OrderInfo.KpServices, connection, transaction);
                        await SaveOrUpdateCollectionAsync(context.OrderInfo.KpServiceParams, connection, transaction);
                        await SaveOrUpdateCollectionAsync(context.OrderInfo.TeoServices, connection, transaction);
                        await SaveOrUpdateCollectionAsync(context.OrderInfo.TeoServiceParams, connection, transaction);
                        await SaveOrUpdateRoutes(context.OrderRoutes, context.OrderInfo, connection, transaction);

                        foreach (var item in context.OrderCargo)
                        {
                            item.OrderId = localOrder.Id.Value;

                            await _repositoryEn.InsertAcync(item, connection, transaction);
                        }


                        transaction.Commit();
                        isSuccess = true;
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }

            return isSuccess;
        }

        private async Task<string> GetDocumentTitleAsync(IEnumerable<RouteForSave> routes)
        {
            var from = routes.FirstOrDefault();
            var to = routes.LastOrDefault();

            var pointIds = (new[] { from?.FromPointId, to?.ToPointId })
                .Where(x => x.HasValue)
                .Select(x => x.Value)
                .Distinct()
                .ToArray();

            var countries = await LoadCountriesAsync(pointIds);

            var strCnFrom = string.Empty;
            var strPtFrom = string.Empty;
            if (from != null)
            {
                var fromCn = countries.FirstOrDefault(x => x.PointId == from.FromPointId);
                if (fromCn != null)
                {
                    strCnFrom = !string.IsNullOrEmpty(fromCn.CountryTitle) ? $"{fromCn.CountryTitle}, " : null;
                    strPtFrom = !string.IsNullOrEmpty(fromCn.PointTitle) ? $"{fromCn.PointTitle}, " : from.FromPointTitle;
                }
            }

            var strCnTo = string.Empty;
            var strPtTo = string.Empty;
            if (to != null)
            {
                var toCn = countries.FirstOrDefault(x => x.PointId == to.ToPointId);
                if (toCn != null)
                {
                    strCnTo = !string.IsNullOrEmpty(toCn.CountryTitle) ? $"{toCn.CountryTitle}, " : null;
                    strPtTo = !string.IsNullOrEmpty(toCn.PointTitle) ? $"{toCn.PointTitle}, " : to.ToPointTitle;
                }
            }

            return $"{strCnFrom}{strPtFrom} - {strCnTo}{strPtTo}";
        }

        private async Task<IEnumerable<PointCountryDto>> LoadCountriesAsync(int[] pointIds)
        {
            if (pointIds.NullOrEmpty())
            {
                return Enumerable.Empty<PointCountryDto>();
            }

            var sql = @"
            SELECT
              p.Id AS PointId,
              p.Title AS PointTitle,
              c.Title AS CountryTitle
            FROM vPoints p WITH(NOLOCK)
              LEFT JOIN Country c WITH(NOLOCK) ON p.CountryId = c.Id
            WHERE p.Id IN (SELECT Id FROM @PointIds)";
            return await _repository.GetAllAsync<PointCountryDto>(sql, new { PointIds = new IntDbType(pointIds) });
        }

        private async Task SaveOrUpdateRoutes(IEnumerable<RouteForSave> orderRoutes, OrderInfoByGuid orderInfo, IDbConnection connection, IDbTransaction transaction)
        {
            if (orderRoutes.NullOrEmpty())
            {
                return;
            }

            var sql = @"INSERT INTO dbo.OrdersRoutes (OrderId,TeoId,FromPointId,FromPointTitle,FromPointCode,ToPointId,ToPointTitle,ToPointCode,ArmIndex,RouteType)
                        VALUES(@OrderId,@TeoId,@FromPointId,@FromPointTitle,@FromPointCode,@ToPointId,@ToPointTitle,@ToPointCode,@ArmIndex,@RouteType)";

            var index = 0;
            foreach (var route in orderRoutes)
            {
                await connection.ExecuteAsync(sql, new
                {
                    OrderId = orderInfo.Id,
                    TeoId = orderInfo.TeoId,
                    FromPointId = route.FromPointCode.StartsWith("AZ_")
                        ? (int?)null
                        : route.FromPointId,
                    FromPointTitle = route.FromPointTitle,
                    FromPointCode = route.FromPointCode.StartsWith("AZ_") || route.FromPointCode.StartsWith("R_")
                        ? route.FromPointCode
                        : route.FromPointCnsi,
                    ToPointId = route.ToPointCode.StartsWith("AZ_")
                        ? (int?)null
                        : route.ToPointId,
                    ToPointTitle = route.ToPointTitle,
                    ToPointCode = route.ToPointCode.StartsWith("AZ_") || route.ToPointCode.StartsWith("R_")
                        ? route.ToPointCode
                        : route.ToPointCnsi,
                    ArmIndex = index,
                    RouteType = route.RouteType
                }, transaction: transaction);
                index++;
            }
        }

        private async Task SaveOrUpdateCollectionAsync<T>(IEnumerable<T> collection, IDbConnection connection, IDbTransaction transaction)
            where T : class
        {
            var type = typeof(T);
            TableNameAttribute tableAttribute = (TableNameAttribute)Attribute.GetCustomAttribute(type, typeof(TableNameAttribute));
            var properties = type.GetProperties().Where(prop => prop.IsDefined(typeof(PrymaryKeyAttribute), false));
            if (!properties.Any())
            {
                throw new Exception($"Не найдено ключевое поле в классе {type.Name}");
            }

            if (properties.Count() > 1)
            {
                throw new Exception($"Найдено больше 1 ключевых полей в классе {type.Name}");
            }

            var keyPropery = properties.First();

            if (!collection.NullOrEmpty())
            {
                foreach (var item in collection)
                {
                    var isExists = await GetIsItemExistsAsync(tableAttribute.Name, keyPropery.Name, keyPropery.GetValue(item), connection, transaction);
                    if (isExists.HasValue && isExists.Value)
                    {
                        await _repositoryEn.UpdateAcync<T>(item, connection, transaction);
                    }
                    else
                    {
                        await _repositoryEn.InsertAcync<T>(item, connection, transaction);
                    }
                }
            }
        }

        private async Task<bool?> GetIsItemExistsAsync(string tableName, string keyField, object id, IDbConnection connection, IDbTransaction transaction)
        {
            var sql = $@"SELECT 
                            CASE
                              WHEN {keyField} IS NOT NULL THEN 1
                              ELSE 0
                            END
                         FROM {tableName}
                         WHERE {keyField} = @Id";
            return connection.ExecuteScalar<bool?>(sql, new { Id = id }, transaction);
        }

        private async Task<int> GetEnityIdByIrsGuidAsync(Guid? irsGuid, string tableName, string guidColumnName = "IrsGuid")
        {
            if (irsGuid.HasValue)
            {
                var id = _cashList[irsGuid.Value.ToString()] as int?;
                if (id.HasValue)
                {
                    return id.Value;
                }

                id = await LoadEnityIdByIrsGuidAsync(irsGuid.Value, tableName, guidColumnName);
                if (id.HasValue)
                {
                    _cashList.Add(irsGuid.Value.ToString(), id.Value);
                    return id.Value;
                }
            }

            return 0;
        }

        private int GetEnumValueByDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new Exception("Тип должен быть enum");
            }

            var enumValue = Enum.GetValues(type)
                .Cast<Enum>()
                .SingleOrDefault(x => x.GetDescription() == description);

            if (enumValue == null)
            {
                throw new Exception($"Не удалось смаппить значение \"{description}\" на тип {type.Name}");
            }

            object underlyingValue = Convert.ChangeType(enumValue, Enum.GetUnderlyingType(enumValue.GetType()));
            return (int)underlyingValue;
        }

        private async Task<int?> LoadEnityIdByIrsGuidAsync(Guid irsGuid, string tableName, string guidColumnName)
        {
            var sql = $"SELECT top 1 Id, {guidColumnName} AS IrsGuid FROM {tableName} WHERE {guidColumnName} = @IrsGuid";
            return await _repository.GetAsync<int?>(sql, new { IrsGuid = irsGuid });
        }
    }
}
