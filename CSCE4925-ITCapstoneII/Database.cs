using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using SQLSolutions.Models;

namespace SQLSolutions
{
    public class Database
    {
        private const string SessionKey = "SQLSolutions.Database.SessionKey";
        
        private static ISessionFactory _sessionFactory;

        public static ISession Session
        {
            get { return (ISession) HttpContext.Current.Items[SessionKey]; }
        }

        public static void Configure()
        {
            var config = new Configuration();

            //configure connection string
            config.Configure();

            //add mappings
            var Mapper = new ModelMapper();
            Mapper.AddMapping<UserMap>();
            Mapper.AddMapping<BookMap>();
            Mapper.AddMapping<TransactionMap>();

            config.AddMapping(Mapper.CompileMappingForAllExplicitlyAddedEntities());

            //create session factory
            _sessionFactory = config.BuildSessionFactory();
        }

        public static void OpenSession()
        {
            HttpContext.Current.Items[SessionKey] = _sessionFactory.OpenSession();
        }

        public static void CloseSession()
        {
            var session = HttpContext.Current.Items[SessionKey] as ISession;
            if (session != null)
                session.Close();

            HttpContext.Current.Items.Remove(SessionKey);
        }
    }
}