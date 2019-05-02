using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace FeedService.Infrastructure.Persistence
{
    public class NHibernateHelper
    {
        public static ISessionFactory CreateSessionFactory(string connectionString)
        {
            return Fluently.Configure().Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString).ShowSql())

                .Mappings(m => m.FluentMappings

                    .AddFromAssemblyOf<Program>()
                    .Conventions.Setup(c =>
                    {
                        c.Add(DefaultLazy.Never());  // Acabar com os virtual
                        //c.Add(DefaultCascade.All());
                    }))

                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                    .Execute(true, false,false))
                .BuildSessionFactory();
        }
        public static ISessionFactory CreateSessionFactoryInMemory()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.InMemory().ShowSql())

                .Mappings(m => m.FluentMappings

                    .AddFromAssemblyOf<Program>()
                    .Conventions.Setup(c =>
                    {
                        c.Add(DefaultLazy.Never());  // Acabar com os virtual
                        //c.Add(DefaultCascade.All());
                    }))

                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true)
                )

                .BuildSessionFactory();
        }
    }
}
