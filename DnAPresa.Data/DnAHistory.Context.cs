﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DnAPresa.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TMW_LiveEntities2 : DbContext
    {
        public TMW_LiveEntities2()
            : base("name=TMW_LiveEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<carter_DnAHistory> carter_DnAHistory { get; set; }
    
        public virtual int up_Insert_carter_dnaHistory(string employID, string lastname, string firstname, string midlname, string emplclas, string db, string testsel)
        {
            var employIDParameter = employID != null ?
                new ObjectParameter("EmployID", employID) :
                new ObjectParameter("EmployID", typeof(string));
    
            var lastnameParameter = lastname != null ?
                new ObjectParameter("lastname", lastname) :
                new ObjectParameter("lastname", typeof(string));
    
            var firstnameParameter = firstname != null ?
                new ObjectParameter("firstname", firstname) :
                new ObjectParameter("firstname", typeof(string));
    
            var midlnameParameter = midlname != null ?
                new ObjectParameter("midlname", midlname) :
                new ObjectParameter("midlname", typeof(string));
    
            var emplclasParameter = emplclas != null ?
                new ObjectParameter("emplclas", emplclas) :
                new ObjectParameter("emplclas", typeof(string));
    
            var dbParameter = db != null ?
                new ObjectParameter("db", db) :
                new ObjectParameter("db", typeof(string));
    
            var testselParameter = testsel != null ?
                new ObjectParameter("testsel", testsel) :
                new ObjectParameter("testsel", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("up_Insert_carter_dnaHistory", employIDParameter, lastnameParameter, firstnameParameter, midlnameParameter, emplclasParameter, dbParameter, testselParameter);
        }
    }
}
