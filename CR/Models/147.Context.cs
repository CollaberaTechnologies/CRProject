﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CR.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class IHD_DBEntities : DbContext
    {
        public IHD_DBEntities()
            : base("name=IHD_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<IHD_USER_MAIN> IHD_USER_MAIN { get; set; }
        public virtual DbSet<IHD_CR> IHD_CR { get; set; }
        public virtual DbSet<Edu_Background> Edu_Background { get; set; }
        public virtual DbSet<Recent_Employeer> Recent_Employeer { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<USER_LIST_TEST> USER_LIST_TEST { get; set; }
        public virtual DbSet<IHD_Tabusers> IHD_Tabusers { get; set; }
        public virtual DbSet<personal_data> personal_data { get; set; }
        public virtual DbSet<IHD_TABDOCS> IHD_TABDOCS { get; set; }
        public virtual DbSet<Reference> References { get; set; }
    }
}
