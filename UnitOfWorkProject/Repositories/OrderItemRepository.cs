using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.UnitOfWorkProject.Model;
using Academits.Karetskas.UnitOfWorkProject.Repositories.Interfaces;

namespace Academits.Karetskas.UnitOfWorkProject.Repositories
{
    public class OrderItemRepository : BaseEfRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(DbContext dbContext) : base(dbContext) { }
    }
}
