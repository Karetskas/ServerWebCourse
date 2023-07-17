using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.WorkUnit.Model;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;

namespace Academits.Karetskas.WorkUnit.Repositories
{
    public class CustomerRepository : BaseEfRepository<Customer>, ICustomerRepository
    {
        private readonly DbContext _dbContext;

        public CustomerRepository(DbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext is null
                ? throw new ArgumentNullException(nameof(dbContext), $"The argument \"{nameof(dbContext)}\" is null.")
                : dbContext;
        }

        public IQueryable<Tuple<string, string, string?, decimal>> GetExpensesForAllTime()
        {
            var orders = _dbContext.Set<Order>().AsQueryable();

            return orders
                .GroupBy(order => order.CustomerId)
                .Select(ordersGroup => Tuple.Create(ordersGroup.Single().Customer.LastName,
                    ordersGroup.Single().Customer.FirstName,
                    ordersGroup.Single().Customer.SecondName,
                    ordersGroup.SelectMany(orderItem => orderItem.OrderItems)
                        .Sum(orderItem => orderItem.Count * orderItem.Product.Price)
                    ));
        }
    }
}
