using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace FeedService.Infrastructure.Persistence
{
    public interface INHibernateFactory
    {
        ISessionFactory CreateSessionFactory();
        ISession GetSession();
    }
    public class NHibernateFactory: INHibernateFactory
    {
        public ISession Session { get; set; }
        public string ConnectionString { get; set; }

        public NHibernateFactory(string connectionString)
        {
            ConnectionString = connectionString;
//            Session = CreateSessionFactory().OpenSession();
//            Session.FlushMode = FlushMode.Auto;
        }
        
        public ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure().Database(MsSqlConfiguration.MsSql2012.ConnectionString(ConnectionString).ShowSql())

                .Mappings(m => m.FluentMappings

                    .AddFromAssemblyOf<Program>()
                    .Conventions.Setup(c =>
                    {
                        c.Add(DefaultLazy.Never()); // Acabar com os virtual
                        //c.Add(DefaultCascade.All());
                    }))

                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                    .Execute(true, false,false))
                .BuildSessionFactory();
        }

        public ISession GetSession()
        {
            return Session;
        }
    }
}
