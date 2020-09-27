using MeAgendaAi.Data.Mapping;
using MeAgendaAi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Data.Context
{
    public class MeAgendaAiContext : DbContext
    {
        public MeAgendaAiContext (DbContextOptions<MeAgendaAiContext> options) : base(options)
        {

        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>(new ClientMap().Configure);
            modelBuilder.Entity<Company>(new CompanyMap().Configure);
            modelBuilder.Entity<Employee>(new EmployeeMap().Configure);
            modelBuilder.Entity<ServiceEmployee>(new ServiceEmployeeMap().Configure);
            modelBuilder.Entity<Policy>(new PolicyMap().Configure);
            modelBuilder.Entity<Scheduling>(new SchedulingMap().Configure);
            modelBuilder.Entity<Service>(new ServiceMap().Configure);
            modelBuilder.Entity<User>(new UserMap().Configure);
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ServiceEmployee> EmployeeServices { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Scheduling> Schedulings { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
