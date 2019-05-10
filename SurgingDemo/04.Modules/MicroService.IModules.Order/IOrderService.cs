using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using MicroService.Data.Common;
using MicroService.Data.Validation;
using MicroService.IApplication.Order.Dto;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;


namespace MicroService.IModules.Order
{
    [ServiceBundle("api/{Service}")]
    public interface IOrderService: IServiceKey
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="orderInfoRequestDto"></param>
        /// <returns></returns>
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<JsonResponse> Create(OrderInfoRequestDto orderInfoRequestDto);
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="orderInfoBatchRequestDto"></param>
        /// <returns></returns>
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<JsonResponse> BatchCreate(OrderInfoBatchRequestDto orderInfoBatchRequestDto);

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orderInfoPageRequestDto"></param>
        /// <returns></returns>
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<PageData> GetPageList(OrderInfoPageRequestDto orderInfoPageRequestDto);
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="entityQueryRequest"></param>
        /// <returns></returns>
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<OrderInfoQueryDto> GetForModify(EntityQueryRequest entityQueryRequest);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="orderInfoRequestDto"></param>
        /// <returns></returns>
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<JsonResponse> Modify(OrderInfoRequestDto orderInfoRequestDto);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entityRequest"></param>
        /// <returns></returns>
        [Authorization(AuthType = AuthorizationType.JWT)]
        Task<JsonResponse> Remove(EntityRequest entityRequest);


        Task<DataTable> GetList();
    }
}
