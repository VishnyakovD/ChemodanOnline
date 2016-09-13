using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Shop.db
{
    public class NHibernateHelper
    {
        public static ISessionFactory _sessionFactory;
        private static Configuration configuration = null;
        public static string connstring = null;

        public static void GenerateDB(bool script)
        {
            Configuration cfg = Configure();
            if (script)
                (new SchemaExport(cfg)).Execute(true, false, false);
            else
                (new SchemaExport(cfg)).Execute(false, true, false);
        }

        public static Configuration Configure()
        {
            configuration = new Configuration();
            configuration.Configure();

            if (!String.IsNullOrEmpty(connstring))
                configuration.SetProperty(NHibernate.Cfg.Environment.ConnectionString, connstring);

            configuration.AddAssembly(typeof(NHibernateHelper).Assembly);


            return configuration;
        }

        public static string ConnectionString
        {
            get
            {
                if (configuration == null)
                    Configure();
                return configuration.GetProperty(NHibernate.Cfg.Environment.ConnectionString);
            }
            set
            {
                if (configuration == null)
                    connstring = value;
            }

        }

        public static bool IsConfigured
        {
            get
            {
                return (configuration != null);
            }
        }

        public static ISessionFactory SessionFactory
        {

            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = Configure();
                    _sessionFactory = configuration.BuildSessionFactory();
                }

                return _sessionFactory;

            }

        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

    }
}
