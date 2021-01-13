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
            modelBuilder.Entity<Services>(new ServiceMap().Configure);
            modelBuilder.Entity<User>(new UserMap().Configure);
            modelBuilder.Entity<Location>(new LocationMap().Configure);
            modelBuilder.Entity<UserRole>(new UserRoleMap().Configure);
            modelBuilder.Entity<EmployeeWorkHours>(new EmployeeWorkHoursMap().Configure);
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ServiceEmployee> ServiceEmployees { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Scheduling> Schedulings { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<EmployeeWorkHours> EmployeeWorkHours { get; set; }
    }
}
