using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace URL_shortener.Factory
{
    public class SessionFactoryBuilder
    {
        public static ISessionFactory BuildSessionFactory(bool create = false, bool update = false)
        {
            return Fluently.Configure()
                           .Database(MySQLConfiguration.Standard.ConnectionString("Server=localhost; Database=urlsdb;User ID=root;Password=A12358133b;"))
                           .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Models.UrlMap>())
                           .CurrentSessionContext("call")
                           .BuildSessionFactory();

        }
        private static void BuildSchema(Configuration config, bool create = false, bool update = false)
        {
            if (create)
            {
                new SchemaExport(config).Create(false, true);
            }
            else
            {
                new SchemaUpdate(config).Execute(false, update);
            }
        }
    }
}
