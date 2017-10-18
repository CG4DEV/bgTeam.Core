using bgTeam;
using bgTeam.DataAccess;
using System;
using System.Threading.Tasks;
using Trcont.IRS.Domain.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Trcont.IRS.Story.Common
{
    public class GetOrderIdByIrsGuidStory : IStory<GetOrderIdByIrsGuidStoryContext, OrderInfoByGuid>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetOrderIdByIrsGuidStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public OrderInfoByGuid Execute(GetOrderIdByIrsGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<OrderInfoByGuid> ExecuteAsync(GetOrderIdByIrsGuidStoryContext context)
        {
            if (context.ReferenceGuid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(context.ReferenceGuid));
            }

            var OrderInfoByGuid = await LoadOrderIds(context.ReferenceGuid);
            OrderInfoByGuid.KpServices = await LoadKpServices(OrderInfoByGuid.Id);
            OrderInfoByGuid.TeoServices = await LoadTeoServices(OrderInfoByGuid.TeoId);

            if (OrderInfoByGuid.KpServices.Any())
            {
                OrderInfoByGuid.KpServiceParams = await LoadKpServiceParams(OrderInfoByGuid.Id);
            }

            if (OrderInfoByGuid.TeoServices.Any())
            {
                OrderInfoByGuid.TeoServiceParams = await LoadTeoServiceParams(OrderInfoByGuid.TeoId);
            }

            return OrderInfoByGuid;
        }

        private async Task<IEnumerable<TeoServiceParamInfo>> LoadTeoServiceParams(int teoId)
        {
            var sql =
            @" 
                SELECT r.ReferenceId AS TeoId                                                                               
                 ,ooav.ObjAttribValId AS Id                                                                                 
                 ,ooav.AttribGUID                                                                                           
                 ,ooav.ObjectId AS TeoServiceId                                                                             
                 ,ooav.ObjectGUID AS TeoServiceGUID                                                                         
                 ,ooav.AttribValueRus                                                                                       
                 ,ooav.AttribNumValue                                                                                       
                 ,ooav.AttribDateValue                                                                                      
                 ,rr.ParentId AS OrderId                                                                                    
                FROM Reference r WITH(NOLOCK)                                                                               
                  INNER JOIN DetailTransport dt WITH(NOLOCK) ON dt.ReferenceId = r.ReferenceId AND dt.detailtype = 0        
                  INNER JOIN ExecTransport et WITH(NOLOCK) ON et.DetailTransId = dt.DetailTransId AND et.IsProfit IN (1,2)  
                  INNER JOIN OA_ObjectAttribsVals ooav WITH(NOLOCK) ON ooav.ObjectId = et.ExecTransId AND ooav.TableSQL = 30
                  LEFT JOIN Ref2Refs rr ON rr.ChildId = r.ReferenceId                                                       
                WHERE r.RefGroupId = 9 AND r.ReferenceId = @TeoId                                                                                
            ";

            return await _repository.GetAllAsync<TeoServiceParamInfo>(sql, new
            {
                TeoId = teoId
            });
        }

        private async Task<IEnumerable<KpServiceParamInfo>> LoadKpServiceParams(int kpId)
        {
            var sql =
               @" 
                  SELECT rl.RefLTRServiceId AS OrderServiceId                                              
                    ,r.ReferenceId AS OrderId                                                              
                    ,ooav.ObjAttribValId AS Id                                                             
                    ,ooav.AttribGUID                                                                       
                    ,ooav.ObjectGUID AS OrderServiceGUID                                                   
                    ,ooav.AttribValueRus                                                                   
                    ,ooav.AttribNumValue                                                                   
                    ,ooav.AttribDateValue                                                                  
                    ,rr.ChildId AS TeoId                                                                   
                  FROM Reference r WITH(NOLOCK)                                                            
                    INNER JOIN RefLTRServices rl WITH(NOLOCK) ON rl.ReferenceId = r.ReferenceId            
                    INNER JOIN OA_ObjectAttribsVals ooav WITH(NOLOCK) ON ooav.ObjectId = rl.RefLTRServiceId
                    LEFT JOIN Ref2Refs rr ON rr.ParentId = r.ReferenceId                                   
                  WHERE r.RefGroupId = 2030 AND r.ReferenceId = @KpId                                        
            ";

            return await _repository.GetAllAsync<KpServiceParamInfo>(sql, new
            {
                KpId = kpId
            });
        }

        private async Task<IEnumerable<RisTeoServiceInfo>> LoadTeoServices(int teoId)
        {
            var sql =
               @" 
                SELECT DISTINCT                                                                                           
                  et.ExecTransId AS ServiceId,                                                                            
                  dt.ReferenceId AS TeoId,                                                                                
                  rr.ParentId AS OrderId,                                                                                 
                  et.WorkTypeId AS ServiceTypeId,                                                                         
                  et.StationFromId AS FromPointId,                                                                        
                  et.StationToId AS ToPointId,                                                                            
                  et.RegionId AS TerritoryId,                                                                             
                  et.ParentExecTransId AS ParentTeoServiceId,                                                             
                  et.SrcCurrencyId AS CurrencyId,                                                                         
                  et.RealTariff AS Tariff,                                                                                
                  CASE                                                                                                    
                    --VAT 18%                                                                                             
                    WHEN et.PayAccountId = 67082 THEN et.RealTariff * 18 / 118                                            
                    ELSE 0.0                                                                                              
                  END AS TariffVAT,                                                                                       
                  et.SrcFullExecSum AS Summ,                                                                              
                  CASE                                                                                                    
                    --VAT 18%                                                                                             
                    WHEN et.PayAccountId = 67082 THEN et.SrcFullExecSum * 18 / 118                                        
                    ELSE 0.0                                                                                              
                  END AS SummVAT,                                                                                         
                  et.ExecVolumeType AS TariffType,                                                                        
                  et.DogovorId AS ContractId,                                                                             
                  et.ArmIndex,                                                                                            
                  et.RefPriceServiceId AS SourcePriceServiceId,                                                           
                  et.RefPriceDocumentId AS SourceReferenceId,
                  et.ExecVolume AS SrcVolume
                FROM DetailTransport dt                                                                                   
                  INNER JOIN ExecTransport et WITH(NOLOCK) ON et.DetailTransId = dt.DetailTransId AND et.IsProfit IN (1,2)
                  INNER JOIN Ref2Refs rr WITH(NOLOCK) ON dt.ReferenceId = rr.ChildId                                      
                WHERE dt.ReferenceId = @TeoId AND dt.detailtype = 0                                                       
                ORDER BY et.ExecTransId                                                                                   
            ";

            return await _repository.GetAllAsync<RisTeoServiceInfo>(sql, new
            {
                @TeoId = teoId
            });
        }

        private async Task<IEnumerable<RisKPServiceInfo>> LoadKpServices(int kpId)
        {
            var sql =
               @" 
                  SELECT                                                                      
                     rl.RefLTRServiceId AS ServiceId,                                         
                     rl.ReferenceId AS OrderId,                                               
                     rl.WorkTypeId AS ServiceTypeId,                                          
                     rl.PointFromId AS FromPointId,                                           
                     rl.PointToId AS ToPointId,                                               
                     rl.SrcCurrencyId AS CurrencyId,                                          
                     rl.SrcExecRate AS Tariff,                                                
                     CASE                                                                     
                       --VAT 18%                                                              
                       WHEN rl.PayAccountId = 67082 THEN rl.SrcExecRate * 18 / 118            
                       ELSE 0.0                                                               
                     END AS TariffVAT,                                                        
                     rl.SrcExecCost AS Summ,                                                  
                     CASE                                                                     
                       --VAT 18%                                                              
                       WHEN rl.PayAccountId = 67082 THEN rl.SrcExecCost * 18 / 118            
                       ELSE 0.0                                                               
                     END AS SummVAT,                                                          
                     rl.VolumeType AS TariffType,                                             
                     rl.IsActive,                                                             
                     rl.ArmIndex,                                                             
                     rl.SourceReferenceId,                                                    
                     rl.SourcePriceServiceId,
                     rl.SrcVolume
                FROM Reference r WITH(NOLOCK)                                                 
                   INNER JOIN RefLTRServices rl WITH(NOLOCK) ON rl.ReferenceId = r.ReferenceId
                WHERE r.RefGroupId = 2030 AND r.ReferenceId = @KpId
            ";

            return await _repository.GetAllAsync<RisKPServiceInfo>(sql, new
            {
                KpId = kpId
            });
        }

        private async Task<OrderInfoByGuid> LoadOrderIds(Guid referenceGuid)
        {
            var sql =
               @" SELECT
                   r.ReferenceId AS Id,
                   r.ReferenceGUID AS IrsGuid,
                   teo.ReferenceId AS TeoId,
                   teo.ReferenceGUID AS TeoGuid,
                   teo.ReferenceTitle AS Number,
                   teo.CreateDate AS OrderDate
                FROM Reference r WITH(NOLOCK)
                  INNER JOIN Ref2Refs rr ON rr.ParentId = r.ReferenceId
                  INNER JOIN Reference teo ON teo.ReferenceId = rr.ChildId AND teo.RefGroupId = 9
                WHERE r.ReferenceGUID = @ReferenceGuid AND r.RefGroupId = 2030
            ";

            return await _repository.GetAsync<OrderInfoByGuid>(sql, new
            {
                ReferenceGuid = referenceGuid
            });
        }
    }
}
