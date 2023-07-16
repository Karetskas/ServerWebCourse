﻿using System;
using Academits.Karetskas.WorkUnit.Repositories;
using Academits.Karetskas.WorkUnit.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Academits.Karetskas.WorkUnit.UnitOfWork
{
    public class UnitOfWorkGroceryStore: IUnitOfWork
    {
        private DbContext _dbContext;
        private IDbContextTransaction? _transaction;

        public UnitOfWorkGroceryStore(DbContext context)
        {
            _dbContext = context is null 
                ? throw new ArgumentNullException(nameof(context), $"The argument \"{nameof(context)}\" = {context} is null.")
                : context;
        }

        public T GetRepository<T>() where T : class, IRepository
        {
            if (typeof(T) == typeof(IProductRepository))
            {
                return (new ProductRepository(_dbContext) as T)!;
            }
            
            if (typeof(T) == typeof(IOrderRepository))
            {
                return (new OrderRepository(_dbContext) as T)!;
            }

            if (typeof(T) == typeof(IOrderItemRepository))
            {
                return (new OrderItemRepository(_dbContext) as T)!;
            }
            
            if (typeof(T) == typeof(ICustomerRepository))
            {
                return (new CustomerRepository(_dbContext) as T)!;
            }

            if (typeof(T) == typeof(ICategoryRepository))
            {
                return (new CategoryRepository(_dbContext) as T)!;
            }

            if (typeof(T) == typeof(ICategoryProductRepository))
            {
                return (new CategoryProductRepository(_dbContext) as T)!;
            }

            throw new Exception($"Unknown repository type: \"{typeof(T)}\"");
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void BeginTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction is null)
            {
                return;
            }

            _transaction.Commit();
            _transaction.Dispose();
        }

        public void RollbackTransaction()
        {
            if (_transaction is null)
            {
                return;
            }

            _transaction.Rollback();
            _transaction.Dispose();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
