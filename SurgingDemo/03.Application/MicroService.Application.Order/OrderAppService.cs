﻿using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroService.Data.Ext;
using MicroService.IApplication.Order;
using MicroService.IRespository.Order;
using MicroService.Entity.Order;
using MicroService.IApplication.Order.Dto;
using MicroService.Core.Data;
using MicroService.Data.Validation;
using MicroService.Application.Order.Validators;
using MicroService.Data.Extensions;
using MicroService.Data.Common;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace MicroService.Application.Order
{
  
    public class OrderAppService : ApplicationEnginee, IOrderAppService
    {
        public IOrderRespository _orderRespository;
        private readonly IMapper _mapper;

        public OrderAppService(IOrderRespository orderRespository, 
          IMapper mapper)
        {
            _orderRespository = orderRespository;
            _mapper = mapper;
        }
        /// <summary>
        ///异步验证
        /// </summary>
        private async Task DoValidationAsync(OrderInfo orderInfo, string validatorType)
        {
            var orderInfoValidator = new OrderInfoValidator();
            var validatorReresult = await orderInfoValidator.DoValidateAsync(orderInfo, validatorType);
            if (!validatorReresult.IsValid)
            {
                throw new DomainException(validatorReresult);
            }
        }

        /// <summary>
        ///异步验证
        /// </summary>
        private async Task DoValidationAsync(IEnumerable<OrderInfo> orderInfos, string validatorType)
        {
            var orderInfoValidator = new OrderInfoValidator();
            var domainException = new DomainException();
            foreach (var orderInfo in orderInfos)
            {
                var validatorReresult = await orderInfoValidator.DoValidateAsync(orderInfo, validatorType);
                if (!validatorReresult.IsValid)
                {
                    domainException.AddErrors(validatorReresult);
                }

            }

            if (domainException.ValidationErrors.ErrorItems.Any()) throw domainException;
        }
        /// <summary>
          /// 新增
          /// </summary>
          /// <param name="orderInfoRequestDto"></param>
          /// <returns></returns>
        public async Task<JsonResponse> CreateAsync(OrderInfoRequestDto orderInfoRequestDto)
        {
         
          
            var resJson = await TryTransactionAsync(async () =>
              {
                  var orderInfo = _mapper.Map<OrderInfoRequestDto, OrderInfo>(orderInfoRequestDto);
                  await DoValidationAsync(orderInfo, ValidatorTypeConstants.Create);
                  await _orderRespository.InsertAsync(orderInfo);
                  
                 
              });
            return resJson;
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="orderInfoRequestDtos"></param>
        /// <returns></returns>
        public async Task<JsonResponse> BatchCreateAsync(IList<OrderInfoRequestDto> orderInfoRequestDtos)
        {
            var resJson = await TryTransactionAsync(async () =>
            {
                var entities = orderInfoRequestDtos.MapToList<OrderInfoRequestDto, OrderInfo>();
                await DoValidationAsync(entities, ValidatorTypeConstants.Create);
                await _orderRespository.BatchInsertAsync(entities);
                
            });
            return resJson;
        }

      
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orderInfoPageRequestDto"></param>
        /// <returns></returns>

        public async Task<PageData> GetPageListAsync( OrderInfoPageRequestDto orderInfoPageRequestDto)
        {
            var pageData = new PageData(orderInfoPageRequestDto.PageIndex, orderInfoPageRequestDto.PageSize);
            var list = await _orderRespository.Entities(e=>e.IsDelete==false).ToPaginated(pageData).ToListAsync();

            return pageData;

        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="entityQueryRequest"></param>
        /// <returns></returns>
        public async Task<OrderInfoQueryDto> GetForModifyAsync(EntityQueryRequest entityQueryRequest)
        {
            var entity = await _orderRespository.Entities(e => e.Id == entityQueryRequest.Id).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.MapEntity<OrderInfo, OrderInfoQueryDto>();//_mapper.Map<OrderInfo, OrderInfoQueryDto>(entity);
            }
            return null;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="orderInfoRequestDto"></param>
        /// <returns></returns>
        public async Task<JsonResponse> ModifyAsync(OrderInfoRequestDto orderInfoRequestDto)
        {
           
            var resJson = await TryTransactionAsync(async () =>
            {
                var orderInfo = _mapper.Map<OrderInfoRequestDto, OrderInfo>(orderInfoRequestDto);
                await DoValidationAsync(orderInfo, ValidatorTypeConstants.Modify);
                await _orderRespository.UpdateAsync(orderInfo);
            });
            return resJson;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<JsonResponse> RemoveAsync(params string[] ids)
        {
            var resJson = await TryTransactionAsync(async () =>
            {
                await _orderRespository.UpdateAsync(ids,  async (e)=> {

                    await Task.Run(() =>
                   {
                       e.IsDelete = true;
                   });
                });
            });
           return resJson;
        }

        public async Task<DataTable> GetList()
        {
            var dic = new Dictionary<string, object>() { };
            dic.Add("isdelete", 1);
            return await _orderRespository.SqlQueryDataTable("select * from OrderInfos where IsDelete=@isdelete",dic
               );
        }
    }
}
