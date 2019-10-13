using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace URL_shortener.Factory
{
    public class SessionFactoryBuilder
    {
        //var listOfEntityMap = typeof(M).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(M))).ToList();  
        //var sessionFactory = SessionFactoryBuilder.BuildSessionFactory(dbmsTypeAsString, connectionStringName, listOfEntityMap, withLog, create, update);  
        //.ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
        public static ISessionFactory BuildSessionFactory(bool create = false, bool update = false)
        {
            return Fluently.Configure()
                           .Database(MySQLConfiguration.Standard.ConnectionString("Server=localhost; Database=urlsdb;User ID=root;Password=A12358133b;"))
                           .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Models.UrlMap>())
                           .CurrentSessionContext("call")
                           .BuildSessionFactory();

        }
        /// <summary>  
        /// Build the schema of the database.  
        /// </summary>  
        /// <param name="config">Configuration.</param>  
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
