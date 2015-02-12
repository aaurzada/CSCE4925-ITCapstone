using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NHibernate.Bytecode.CodeDom;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

//Mapping for C# entity to MySQL table
namespace SQLSolutions.Models
{
    public class User
    {
        //user idNum {primary key
        public virtual int IdNum { get; set; }
        //user euid for log in purposes...must be a required field
        [Required(ErrorMessage="Please enter your euid", AllowEmptyStrings=false)]
        public virtual String Euid { get; set; }
        //first name
        public virtual String FirstName { get; set; }
        //last name
        public virtual String LastName { get; set; }
        //email
        public virtual String Email { get; set; }
    }

    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Table("user");

            Id(x => x.IdNum, x => x.Generator(Generators.Assigned));

            Property(x => x.IdNum, x => x.NotNullable(true));
            Property(x => x.Euid, x => x.NotNullable(true));
            Property(x => x.FirstName, x => x.NotNullable(true));
            Property(x => x.LastName, x => x.NotNullable(true));
            Property(x => x.Email, x => x.NotNullable(true));

        }
    }
}