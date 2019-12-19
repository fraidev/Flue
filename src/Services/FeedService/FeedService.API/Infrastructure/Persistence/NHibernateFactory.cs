using System.Diagnostics.CodeAnalysis;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace FeedService.Infrastructure.Persistence
{
    [ExcludeFromCodeCoverage]
    public class NHibernateFactory
    {
        private string ConnectionString { get; }

        public NHibernateFactory(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
        public ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure().Database(MsSqlConfiguration.MsSql2012.ConnectionString(ConnectionString)
                    #if DEBUG
                        .ShowSql()
                    #endif    
                )

                .Mappings(m => m.FluentMappings
                    .AddFromAssemblyOf<Program>()
                    .Conventions.Setup(c =>
                    {
                        c.Add(DefaultLazy.Never());
                    }))

                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                    .Execute(true, false,false))
                .BuildSessionFactory();
        }
    }
}
