using System;
using System.Configuration;
using System.Linq;
using FeedService.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using NHibernate;

namespace Spellbook.Infrastructure.Persistence
{
    public interface IUnitOfWork
    {
        void Save<TEntity>(TEntity entity) where TEntity : class;
        /*TEntity Merge<TEntity>(TEntity entity) where TEntity : class; */
        void Update(object entity);
        void SaveOrUpdate(object entity);
        void Delete(object entity);
        /* void Delete(params object[] entities);
         void Clear(); */
        void Flush();
        T GetById<T>(Guid id);
        IQueryable<T> Query<T>();
        /* ISQLQuery CreateSqlQuery(string sql);
         IQuery GetNamedQuery(string namedQuery);
         object Get(Type type, object id);
         IList List(Type t);*/
        bool Contains(object entity);
        //        bool IsMapped(Type type);
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private readonly ISession _session;

        public UnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
            _session = NHibernateHelper.CreateSessionFactory(_configuration["ConnectionStrings:Default"]).OpenSession();
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

        public void SaveOrUpdate(object entity)
        {
            _session.SaveOrUpdate(entity);
            _session.Transaction.Commit();
        }

        public void Delete(object entity)
        {
            _session.Delete(entity);
            _session.Transaction.Commit();
        }

        public void Flush()
        {
            _session.Flush();
        }

        public T GetById<T>(Guid id)
        {
            return _session.Get<T>(id);
        }

        public IQueryable<T> Query<T>()
        {
            return _session.Query<T>();
        }

        public bool Contains(object entity)
        {
            return _session.Contains(entity);
        }
    }
}