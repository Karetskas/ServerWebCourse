using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories.Interfaces;
using Academits.Karetskas.PhoneBook.DataAccess;
using Academits.Karetskas.PhoneBook.UnitOfWork.Repositories;

namespace Academits.Karetskas.PhoneBook.UnitOfWork.UnitOfWork
{
    public class UnitOfWorkPhoneBook : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private IDbContextTransaction? _transaction;
        private bool _isDisposed;

        public UnitOfWorkPhoneBook(PhoneBookDbContext dbContext, IServiceProvider serviceProvider)
        {
            CheckArgument(dbContext);
            CheckArgument(serviceProvider);

            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }

        private void CheckArgument(object? obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj), $"The argument \"{nameof(obj)}\" is null.");
            }
        }

        public T? GetRepository<T>() where T : class, IRepository
        {
            return _serviceProvider.GetService<T>();
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
