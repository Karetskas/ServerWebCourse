﻿using Microsoft.EntityFrameworkCore;
using Academits.Karetskas.ShopEf.Model;

namespace Academits.Karetskas.ShopEf
{
    public class ShopEfContext : DbContext
    {
        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Customer> Customers { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        public DbSet<CategoryProduct> CategoriesProducts { get; set; } = null!;

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
                builder.Property(category => category.Name)
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(builder =>
            {
                builder.Property(product => product.Name)
                    .HasMaxLength(50);

                builder.Property(product => product.Price)
                    .HasPrecision(10, 2)
                    .HasDefaultValue(0.00);

                builder.HasMany(product => product.Categories)
                    .WithMany(category => category.Products)
                    .UsingEntity<CategoryProduct>(categoryProductBuilder => categoryProductBuilder.ToTable("CategoryProduct"));
            });

            modelBuilder.Entity<Customer>(builder =>
            {
                builder.Property(customer => customer.FirstName)
                    .HasMaxLength(50);

                builder.Property(customer => customer.LastName)
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
                builder.Property(order => order.Date)
                    .HasColumnType("date");

                builder.HasOne(order => order.Customer)
                    .WithMany(customer => customer.Orders)
                    .HasForeignKey(order => order.CustomerId);

                builder.HasMany(order => order.Products)
                    .WithMany(product => product.Orders)
                    .UsingEntity<OrderItem>(
                        right => right
                            .HasOne(orderItem => orderItem.Product)
                            .WithMany(product => product.OrderItems)
                            .HasForeignKey(orderItem => orderItem.ProductId),
                        left => left
                            .HasOne(orderItem => orderItem.Order)
                            .WithMany(order => order.OrderItems)
                            .HasForeignKey(orderItem => orderItem.OrderId),
                        orderItemBuilder =>
                        {
                            orderItemBuilder.Property(orderItem => orderItem.Count).HasDefaultValue(0);
                            orderItemBuilder.HasKey(orderItem => orderItem.Id);
                        });
            });
        }
    }
}
