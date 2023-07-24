using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Academits.Karetskas.UnitOfWorkProject.Repositories;
using Academits.Karetskas.UnitOfWorkProject.Repositories.Interfaces;

namespace Academits.Karetskas.UnitOfWorkProject.UnitOfWork
{
    public class UnitOfWorkGroceryStore : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private IDbContextTransaction? _transaction;
        private bool _isDisposed;

        public UnitOfWorkGroceryStore(DbContext dbContext)
        {
            _dbContext = dbContext is null
                ? throw new ArgumentNullException(nameof(dbContext), $"The argument \"{nameof(dbContext)}\" = {dbContext} is null.")
                : dbContext;
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
            if (_transaction is not null)
            {
                throw new TransactionException("Previous transaction not completed.");
            }

            _transaction = _dbContext.Database.BeginTransaction();
            _isDisposed = false;
        }

        public void CommitTransaction()
        {
            CompleteTransaction(() => _transaction!.Commit());
        }

        public void RollbackTransaction()
        {
            CompleteTransaction(() => _transaction!.Rollback());
        }

        private void CompleteTransaction(Action task)
        {
            if (_transaction is null)
            {
                return;
            }

            task();

            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(_dbContext), $"The \"{nameof(_dbContext)}\" object has already called Dispose.");
            }

            _transaction.Dispose();
            _transaction = null;
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _dbContext.Dispose();
            _isDisposed = true;
        }
    }
}
