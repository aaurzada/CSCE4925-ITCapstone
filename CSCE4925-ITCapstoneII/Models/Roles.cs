using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SQLSolutions.Models
{
    public class Roles
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class RolesMap : ClassMapping<Roles>
    {
        public RolesMap() 
        {
            Table("roles");

            Id(x => x.Id, x => x.Generator(Generators.Assigned));

            Property(x => x.Id, x => x.NotNullable(true));
            Property(x => x.Name, x => x.NotNullable(true));
        }
    }
}