using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode.Conformist;

namespace SQLSolutions.Models
{
    public class RolesUser
    {
        public virtual int UserId { get; set; }
        public virtual int RoleId { get; set; }
    }

    public class RolesUserMap : ClassMapping<RolesUser>
    {
        public RolesUserMap()
        {
            Table("roles_user");

            Property(x => x.UserId, x =>
            {
                //override column name to match table
                x.Column("user_id");
                x.NotNullable(true);
            });
            Property(x => x.RoleId, x =>
            {
                //override column name to match table
                x.Column("role_id");
                x.NotNullable(true);
            });
        }
    }
}