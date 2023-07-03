namespace Academits.Karetskas.ShopEf.Model
{
    public class CategoryProduct
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = new Category();

        public int ProductId { get; set; }

        public virtual Product Product { get; set; } = new Product();
    }
}
