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
            CheckDisposed();

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
            CheckDisposed();

            _dbContext.SaveChanges();
        }

        public void BeginTransaction()
        {
            CheckDisposed();

            if (_transaction is not null)
            {
                throw new TransactionException("Transaction has already started.");
            }

            _transaction = _dbContext.Database.BeginTransaction();
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
            CheckDisposed();

            if (_transaction is null)
            {
                throw new TransactionException("Transaction hasn't been started.");
            }

            task();

            _transaction.Dispose();
            _transaction = null;
        }

        private void CheckDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(_dbContext), $"The \"{nameof(_dbContext)}\" object has already called Dispose.");
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (_transaction is not null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }

            _dbContext.Dispose();
            _isDisposed = true;
        }
    }
}
