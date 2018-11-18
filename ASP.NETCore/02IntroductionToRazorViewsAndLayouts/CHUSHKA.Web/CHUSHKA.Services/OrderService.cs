namespace CHUSHKA.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Data;
    using Contracts;

    public class OrderService : IOrderService
    {
        private readonly ChushkaDbContext dbContext;
        private readonly IMapper mapper;

        public OrderService(ChushkaDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public ICollection<T> All<T>()
        {
            var orders = this.dbContext.Orders
                .ToArray()
                .Select(o => mapper.Map<T>(o))
                .ToArray();

            return orders;
        }
    }
}
