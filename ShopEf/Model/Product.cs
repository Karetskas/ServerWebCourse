using System.Collections.Generic;

namespace Academits.Karetskas.ShopEf.Model
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
