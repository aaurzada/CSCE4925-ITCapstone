using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Cfg;

namespace SQLSolutions
{
    public class Database
    {
        public static void Configure()
        {
            var config = new Configuration();

            //configure connection string
            config.Configure();
            //add our mappings

            //create session
        }

        public static void OpenSession()
        {
            
        }

        public static void CloseSession()
        {
            
        }
    }
}