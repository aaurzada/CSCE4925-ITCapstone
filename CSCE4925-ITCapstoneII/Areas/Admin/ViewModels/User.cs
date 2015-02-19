using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SQLSolutions.Models;

//TODO: add the rest of the needed fields from User table
namespace SQLSolutions.Areas.Admin.ViewModels
{
    public class UserIndex
    {
        public IEnumerable<User> Users { get; set; }
    }

    public class UserNew
    {
        [Required, MaxLength(128)]
        public string Username { get; set; }

        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class UserEdit
    {
        [Required, MaxLength(128)]
        public string Username { get; set; }

        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}