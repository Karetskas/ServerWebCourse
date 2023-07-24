using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.UnitOfWorkProject.Model;
using Academits.Karetskas.UnitOfWorkProject.Repositories.Interfaces;

namespace Academits.Karetskas.UnitOfWorkProject.Repositories
{
    public class OrderRepository : BaseEfRepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext dbContext) : base(dbContext) { }
    }
}
