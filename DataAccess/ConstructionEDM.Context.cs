﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ConstructionCompanyEntities : DbContext
    {
        public ConstructionCompanyEntities()
            : base("name=ConstructionCompanyEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<ContactWay> ContactWays { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<GroupConnection> GroupConnections { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<JobEmployee> JobEmployees { get; set; }
        public virtual DbSet<JobGroup> JobGroups { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Link> Links { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProjectConnection> ProjectConnections { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Satisfaction> Satisfactions { get; set; }
        public virtual DbSet<Slider> Sliders { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
