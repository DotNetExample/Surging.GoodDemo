using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using MicroService.Data.Common;
using MicroService.Data.Extensions;
using MicroService.Data.Validation;
using MicroService.IApplication.Order;
using MicroService.IApplication.Order.Dto;
using MicroService.IModules.Order;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using Surging.Core.CPlatform.Utilities;
using Newtonsoft.Json;

namespace MicroService.Modules.Order
{
    [ModuleName("Order")]
   public class OrderService: ProxyServiceBase, IOrderService
    {
        private readonly IOrderAppService _orderAppService;
        private readonly IOrderDetailAppService _orderDetailAppService;
        public OrderService(IOrderAppService orderAppService, IOrderDetailAppService orderDetailAppService)
        {
            _orderAppService = orderAppService;
            _orderDetailAppService = orderDetailAppService;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="orderInfoRequestDto"></param>
        /// <returns></returns>
        public async Task<JsonResponse> Create(OrderInfoRequestDto orderInfoRequestDto)
        {
           
            orderInfoRequestDto.ToLoginUser();
            List<GoodsQueryDto> goodsQuerys = await GetGoodsAsync(orderInfoRequestDto);

            List<OrderDetailRequestDto> orderDetailRequestDtos = new List<OrderDetailRequestDto>();
            orderInfoRequestDto.Id = Guid.NewGuid().ToString();
            foreach (var item in goodsQuerys)
            {
                var good = orderInfoRequestDto.GoodsRequests.Where(g => g.GoodsId == item.Id).SingleOrDefault();
                orderDetailRequestDtos.Add(new OrderDetailRequestDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    GoodsId = item.Id,
                    OrderId = orderInfoRequestDto.Id,
                    Count = good.Count,
                    Price = item.Price,
                    Money = good.Count * item.Price
                });
            }
            orderInfoRequestDto.OrderNumber = DateTime.Now.ToString();
            orderInfoRequestDto.TotalMoney = orderDetailRequestDtos.Select(d => d.Money).Sum();
            orderInfoRequestDto.ExpireTime = DateTime.Now.AddDays(14);

            var resJson = await new ApplicationEnginee().TryTransactionAsync(async () =>
            {
                await _orderAppService.CreateAsync(orderInfoRequestDto);
                await _orderDetailAppService.BatchCreateAsync(orderDetailRequestDtos);
            });
            return resJson;


        }

        private static async Task<List<GoodsQueryDto>> GetGoodsAsync(OrderInfoRequestDto orderInfoRequestDto)
        {
            var serviceProxyProvider = ServiceLocator.GetService<IServiceProxyProvider>();
            Dictionary<string, object> model = new Dictionary<string, object>();
            model.Add("entityQueryRequest", JsonConvert.SerializeObject(new
            {
                Ids = orderInfoRequestDto.GoodsRequests.Select(g => g.GoodsId).ToList(),
            }));
            string path = "api/Goods/GetGoodsByIds";
            string serviceKey = "Goods";

            var goodsProxy = await serviceProxyProvider.Invoke<object>(model, path, serviceKey);
            List<GoodsQueryDto> goodsQuerys = JsonConvert.DeserializeObject<List<GoodsQueryDto>>(goodsProxy.ToString());
            return goodsQuerys;
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="orderInfoBatchRequestDto"></param>
        /// <returns></returns>
        public async Task<JsonResponse> BatchCreate(OrderInfoBatchRequestDto orderInfoBatchRequestDto)
        {
            foreach (var orderInfoRequest in orderInfoBatchRequestDto.OrderInfoRequestList)
            {
                orderInfoRequest.ToLoginUser();
            }
            return await _orderAppService.BatchCreateAsync(orderInfoBatchRequestDto.OrderInfoRequestList);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orderInfoPageRequestDto"></param>
        /// <returns></returns>
        public async Task<PageData> GetPageList(OrderInfoPageRequestDto orderInfoPageRequestDto)
        {
            orderInfoPageRequestDto.ToLoginUser();
            return await _orderAppService.GetPageListAsync(orderInfoPageRequestDto);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="entityQueryRequest"></param>
        /// <returns></returns>
        public async Task<OrderInfoQueryDto> GetForModify(EntityQueryRequest entityQueryRequest)
        {
            entityQueryRequest.ToLoginUser();
            return await _orderAppService.GetForModifyAsync(entityQueryRequest);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="orderInfoRequestDto"></param>
        /// <returns></returns>
        public async Task<JsonResponse> Modify(OrderInfoRequestDto orderInfoRequestDto)
        {
            orderInfoRequestDto.ToLoginUser();
            return await _orderAppService.ModifyAsync(orderInfoRequestDto);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entityRequest"></param>
        /// <returns></returns>
        public async Task<JsonResponse> Remove(EntityRequest entityRequest)
        {
            entityRequest.ToLoginUser();
            return await _orderAppService.RemoveAsync(entityRequest.Ids.ToArray());
        }

        public async Task<DataTable> GetList()
        {
            return await _orderAppService.GetList();
        }
    }
}
