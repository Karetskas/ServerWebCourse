using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.UnitOfWorkProject.Model;
using Academits.Karetskas.UnitOfWorkProject.QueryResultClasses;
using Academits.Karetskas.UnitOfWorkProject.Repositories.Interfaces;

namespace Academits.Karetskas.UnitOfWorkProject.Repositories
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

        public List<CustomerInfo> GetExpensesForAllTime()
        {
            var customers = _dbContext.Set<Customer>();

            return customers
                .Select(customer => new CustomerInfo(
                    customer.LastName,
                    customer.FirstName,
                    customer.SecondName,
                    customer.Orders
                        .SelectMany(order => order.OrderItems)
                        .Sum(orderItem => orderItem.Count * orderItem.Product.Price)
                ))
                .ToList();
        }
    }
}
