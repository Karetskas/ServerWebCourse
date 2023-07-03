using System;
using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.ShopEf.Model;

namespace Academits.Karetskas.ShopEf
{
    public class ShopEfContext : DbContext
    {
        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Customer> Customers { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<OrderItem> OrdersItems { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer("Server=Micron;Database=ShopEf;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(builder =>
            {
                builder.ToTable("category");

                builder.Property(category => category.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(builder =>
            {
                builder.ToTable("product");

                builder.Property(product => product.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(product => product.Price)
                    .IsRequired()
                    .HasPrecision(10, 2)
                    .HasDefaultValue(0.00);

                builder.HasMany(product => product.Categories)
                    .WithMany(category => category.Products)
                    .UsingEntity(j => j.ToTable("categoryProduct"));
            });

            modelBuilder.Entity<Customer>(builder =>
            {
                builder.ToTable("customer");

                builder.Property(customer => customer.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(customer => customer.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                builder.Property(customer => customer.SecondName)
                    .HasMaxLength(50);

                builder.Property(customer => customer.PhoneNumber)
                    .HasMaxLength(50);

                builder.Property(customer => customer.LastName)
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(builder =>
            {
                builder.ToTable("order");

                builder.Property(order => order.Date)
                    .IsRequired()
                    .HasDefaultValue(new DateTime());

                builder.HasOne(order => order.Customer)
                    .WithMany(customer => customer.Orders)
                    .HasForeignKey(order => order.CustomerId);

                builder.HasMany(order => order.Products)
                    .WithMany(product => product.Orders)
                    .UsingEntity<OrderItem>(
                        j => j
                            .HasOne(orderItem => orderItem.Product)
                            .WithMany(product => product.OrdersItems)
                            .HasForeignKey(orderItem => orderItem.ProductId),
                        j => j
                            .HasOne(orderItem => orderItem.Order)
                            .WithMany(order => order.OrdersItems)
                            .HasForeignKey(orderItem => orderItem.OrderId),
                        j =>
                        {
                            j.Property(orderItem => orderItem.Count).HasDefaultValue(0);
                            j.HasKey(orderItem => orderItem.Id);
                            j.ToTable("orderItem");
                        });
            });
        }
    }
}
