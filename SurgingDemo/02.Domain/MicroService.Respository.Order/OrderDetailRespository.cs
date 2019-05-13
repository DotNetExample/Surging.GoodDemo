
using System;
using System.Collections.Generic;
using System.Text;
using MicroService.Entity.Order;
using MicroService.IRespository.Order;
using MicroService.EntityFramwork;
namespace MicroService.Respository.Order
{
    public class OrderDetailRespository : RespositoryBase<OrderDetail>, IOrderDetailRespository
    {
        public OrderDetailRespository()
        {
        }
    }
}
