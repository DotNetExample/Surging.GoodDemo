using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using MicroService.Core;
using MicroService.Data.Common;
using MicroService.Data.Validation;
using MicroService.IApplication.Order.Dto;
namespace MicroService.IApplication.Order
{
    
    public interface IOrderAppService : IDependency
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="orderInfoRequestDto"></param>
        /// <returns></returns>
        Task<JsonResponse> CreateAsync(OrderInfoRequestDto orderInfoRequestDto);
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="orderInfoRequestDtos"></param>
        /// <returns></returns>
        Task<JsonResponse> BatchCreateAsync(IList<OrderInfoRequestDto> orderInfoRequestDtos);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orderInfoPageRequestDto"></param>
        /// <returns></returns>
        Task<PageData> GetPageListAsync(OrderInfoPageRequestDto orderInfoPageRequestDto);
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="entityQueryRequest"></param>
        /// <returns></returns>
        Task<OrderInfoQueryDto> GetForModifyAsync(EntityQueryRequest entityQueryRequest);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="orderInfoRequestDto"></param>
        /// <returns></returns>
        Task<JsonResponse> ModifyAsync(OrderInfoRequestDto orderInfoRequestDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<JsonResponse> RemoveAsync(params string[] ids);


        Task<DataTable> GetList();
    }
}
