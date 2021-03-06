﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

//Mapping between C# Book class and MySQL "book" table 
namespace SQLSolutions.Models
{
    public class Book
    {
        // ? - Make integers nullable, to remove 0 from diplaying in the testbox
        [RegularExpression(@"^[0-9\s]{4}$", ErrorMessage = "Please enter valid 4 digit Asset Number")]
        [Display(Name = "Asset Number:")]
        public virtual int AssetNum { get; set; }
         [Display(Name = "Isbn:")]
        public virtual string Isbn { get; set; }
         [Display(Name = "Title:")]
        public virtual string Title { get; set; }
         [Display(Name = "Author:")]
        public virtual string Author { get; set; }
         [Display(Name = "Course/Section:")]
        public virtual string CourseSection { get; set; }
         [Display(Name = "Year:")]
        public virtual int ? Year { get; set; }
         [Display(Name = "Edition:")]
        public virtual int ? Edition { get; set; }
        public virtual bool IsRequired { get; set; }
        public virtual bool InStock { get; set; }
    }

    public class BookMap : ClassMapping<Book>
    {
        public BookMap()
        {
            Table("book");

            Id(x => x.AssetNum, x => x.Generator(Generators.Assigned));


            Property(x => x.Isbn, x => x.NotNullable(true));
            Property(x => x.Title, x => x.NotNullable(true));
            Property(x => x.Author, x => x.NotNullable(true));
            Property(x => x.CourseSection, x => x.NotNullable(true));
            Property(x => x.Year, x => x.NotNullable(true));
            Property(x => x.Edition, x => x.NotNullable(true));
            Property(x => x.IsRequired, x => x.NotNullable(true));
            Property(x => x.InStock, x => x.NotNullable(true));


        }
    }
}