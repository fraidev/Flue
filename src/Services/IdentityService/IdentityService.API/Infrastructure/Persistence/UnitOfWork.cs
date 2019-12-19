using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NHibernate;

namespace IdentityService.Infrastructure.Persistence
{
    public interface IUnitOfWork
    {
        void Save<TEntity>(TEntity entity) where TEntity : class;
        void Update(object entity);
        void Delete(object entity);
        T GetById<T>(Guid id);
        IQueryable<T> Query<T>();
    }

    [ExcludeFromCodeCoverage]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;

        public UnitOfWork(ISession session)
        {
            _session = session;
            _session.BeginTransaction();
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class
        {
            _session.Save(entity);
            _session.Transaction.Commit();
        }

        public void Update(object entity)
        {
            _session.Update(entity);
            _session.Transaction.Commit();
        }

        public void Delete(object entity)
        {
            _session.Delete(entity);
            _session.Transaction.Commit();
        }

        public T GetById<T>(Guid id)
        {
            return _session.Get<T>(id);
        }

        public IQueryable<T> Query<T>()
        {
            return _session.Query<T>();
        }

    }
}