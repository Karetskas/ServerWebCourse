using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.WorkUnit.Model;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;

namespace Academits.Karetskas.WorkUnit.Repositories
{
    public class OrderRepository : BaseEfRepository<Order>, IOrderRepository
    {
        public OrderRepository(DbContext dbContext) : base(dbContext) { }
    }
}
