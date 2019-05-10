using System;
using System.Collections.Generic;
using System.Text;
using MicroService.Entity.Order;
using MicroService.IRespository.Order;
using MicroService.EntityFramwork;
namespace MicroService.Respository.Order
{
    public class OrderRespository : RespositoryBase<OrderInfo>, IOrderRespository
    {
        public OrderRespository(IUnitOfWorkDbContext dbDbContext) : base(dbDbContext)
        {

        }
    }
}
