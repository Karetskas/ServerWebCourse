using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.WorkUnit.Model;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;

namespace Academits.Karetskas.WorkUnit.Repositories
{
    public class OrderItemRepository : BaseEfRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(DbContext dbContext) : base(dbContext) { }
    }
}
