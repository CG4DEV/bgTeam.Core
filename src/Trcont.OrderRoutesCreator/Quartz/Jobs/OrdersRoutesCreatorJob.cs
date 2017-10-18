namespace Trcont.OrderRoutesCreator.Quartz.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using Dapper;
    using DapperExtensions.Mapper;
    using Quartz;
    using Trcont.RIS.Common;
    using Trcont.Ris.Domain.Dto;
    using global::Quartz;

    public class OrdersRoutesCreatorJob : IJob
    {
        private const int COMMAND_TIMEOUT = 300;

        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;

            string name = dataMap.GetString("Name");
            var logger = (IAppLogger)dataMap["Logger"];
            var repository = (IRepository)dataMap["Repository"];
            var sql = dataMap.GetString("Sql");
            int pool = dataMap.GetIntValue("Pool");
            var connFactory = (IConnectionFactory)dataMap["ConnectionFactory"];
            var routesCreator = (IOrderRoutesCreatorService)dataMap["RoutesCreator"];

            logger.Info($"Start job for {name}");
            try
            {
                await ProcessAsync(logger, name, (int?)dataMap["DateChangeOffset"], sql, repository, pool, routesCreator, connFactory);
            }
            catch (Exception ex)
            {
                logger.Info($"Error in job for {name}: {ex.ToString()}");
            }

            logger.Info($"End job for {name}");
        }

        private async Task ProcessAsync(IAppLogger logger, string name, int? dateChangeOffset, string sql,
            IRepository repository, int pool, IOrderRoutesCreatorService routesCreator, IConnectionFactory connFactory)
        {
            int? dateOffset = GetDateOffset(dateChangeOffset);
            int i = 0;
            var param = new SqlParams
            {
                DateChange = dateOffset.HasValue ? DateTime.Now.AddDays(dateOffset.Value) : new DateTime(1900, 01, 01),
                PageSize = pool,
                Skip = i * pool
            };

            var preparedSql = GetPoolPagingSql(sql, param);

            while (i == 0 || pool > 0)
            {
                try
                {
                    var teoIds = await repository.GetAllAsync<int>(preparedSql, param);

                    if (!teoIds.Any())
                    {
                        logger.Info($"Start job for entity type {name}. No messages");
                        logger.Info($"End job for entity type {name}.");
                        return;
                    }

                    var orders = await LoadOrdersAsync(teoIds, repository);

                    if (orders.Count() < 1000)
                    {
                        foreach (var order in orders)
                        {
                            await ProcessOrderAsync(order, connFactory, routesCreator, logger);
                        }
                    }
                    else
                    {
                        Parallel.ForEach(orders, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, async order =>
                        {
                            await ProcessOrderAsync(order, connFactory, routesCreator, logger);
                        });
                    }
                    param.Skip = ++i * pool;
                }
                catch (Exception exp)
                {
                    logger.Error(exp);
                    throw;
                }
            }
        }

        private async Task ProcessOrderAsync(RouteServiceOrderDto order, IConnectionFactory factory, IOrderRoutesCreatorService routesCreator, IAppLogger logger)
        {
            var orderRoutes = routesCreator.GetOrderRoutes(order);

            if (orderRoutes.NullOrEmpty())
            {
                logger.Info($"Process order with Id {order.Id}, TeoId {order.TeoId}. Nothing to save");
                return;
            }

            var sql = @"INSERT INTO dbo.OrdersRoutes (OrderId,TeoId,FromPointId,FromPointTitle,FromPointCode,ToPointId,ToPointTitle,ToPointCode,ArmIndex,RouteType)
                        VALUES(@OrderId,@TeoId,@FromPointId,@FromPointTitle,@FromPointCode,@ToPointId,@ToPointTitle,@ToPointCode,@ArmIndex,@RouteType)";
            using (var connection = await factory.CreateAsync())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var route in orderRoutes)
                        {
                            await connection.ExecuteAsync(sql, new
                            {
                                TeoId = route.TeoId,
                                OrderId = route.OrderId,
                                FromPointId = route.FromPointId,
                                FromPointTitle = route.FromPointTitle,
                                FromPointCode = route.FromPointCode,
                                ToPointId = route.ToPointId,
                                ToPointTitle = route.ToPointTitle,
                                ToPointCode = route.ToPointCode,
                                ArmIndex = route.ArmIndex,
                                RouteType = route.RouteType,
                            }, transaction: transaction, commandTimeout: COMMAND_TIMEOUT);
                            
                        }
                        transaction.Commit();
                        logger.Info($"Process order with Id {order.Id}, TeoId {order.TeoId}. Routes sucsessful saved");
                    }
                    catch(Exception ex)
                    {
                        logger.Error($"Process order with Id {order.Id}, TeoId {order.TeoId} error. Excepion {ex.ToString()}");
                        transaction.Rollback();
                        logger.Error(ex);
                        throw;
                    }
                }
            }
        }

        private async Task<IEnumerable<RouteServiceOrderDto>> LoadOrdersAsync(IEnumerable<int> teoIds, IRepository repository)
        {
            var sql = @"
            SELECT
                o.Id,
                o.IrsGuid,
                o.TeoId,
                o.TeoGuid,
                o.PlaceFromId,
                o.PlaceToId
            FROM Orders o WITH (NOLOCK)
            WHERE o.TeoId IN (SELECT Id FROM @TeoIds)
            ";
            var orders = await repository.GetAllAsync<RouteServiceOrderDto>(sql, new { TeoIds = new IntDbType(teoIds) });

            var sql2 = @"
                SELECT
                    os.ServiceId,
                    os.OrderId,
                    os.TeoId,
                    os.ServiceTypeId,
                    os.FromPointId,
                    fPoint.CnsiCode AS FromPointCode,
                    fPoint.Title AS FromPointTitle,
                    fPoint.PointType AS FromPointType,
                    os.ToPointId,
                    tPoint.CnsiCode AS ToPointCode,
                    tPoint.Title AS ToPointTitle,
                    tPoint.PointType AS ToPointType,
                    os.ArmIndex
                FROM OrdersService os
                    LEFT JOIN vPoints fPoint WITH(NOLOCK) ON os.FromPointId = fPoint.Id
                    LEFT JOIN vPoints tPoint WITH(NOLOCK) ON os.ToPointId = tPoint.Id
                WHERE os.TeoId IN (SELECT Id FROM @TeoIds)
                ORDER BY os.ArmIndex ASC";
            var services = await repository.GetAllAsync<RouteServiceOrderServiceDto>(sql2, new { TeoIds = new IntDbType(teoIds) });

            if (orders.Count() < 1000)
            {
                foreach (var order in orders)
                {
                    order.Services = services.Where(x => x.TeoId == order.TeoId).ToArray();
                }
            }
            else
            {
                Parallel.ForEach(orders, new ParallelOptions() { MaxDegreeOfParallelism = 50 }, async order =>
                {
                    order.Services = services.Where(x => x.TeoId == order.TeoId).ToArray();
                });
            }

            return orders;
        }

        private static int? GetDateOffset(int? inputOffset)
        {
            int? offset = null;
            if (inputOffset.HasValue)
            {
                offset = inputOffset.Value < 0 ? inputOffset.Value : -inputOffset.Value;
            }

            return offset;
        }

        private static string GetPoolPagingSql(string sql, SqlParams prms)
        {
            string offset = $"\r\nOFFSET @Skip ROWS FETCH FIRST @PageSize ROWS ONLY";
            string orderBy = "\r\nORDER BY 1";

            if (prms.PageSize > 0)
            {
                return Regex.IsMatch(sql, @"ORDER BY(\s|\w|\.)+$", RegexOptions.IgnoreCase) ? string.Concat(sql, offset) : string.Concat(sql, orderBy, offset);
            }
            else
            {
                return sql;
            }
        }
    }

    internal class SqlParams
    {
        public DateTime DateChange { get; set; }

        public int Skip { get; set; }

        public int PageSize { get; set; }
    }
}
