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
            modelBuilder.Entity<EmployeeService>(new EmployeeServiceMap().Configure);
            modelBuilder.Entity<Policy>(new PolicyMap().Configure);
            modelBuilder.Entity<Scheduling>(new SchedulingMap().Configure);
            modelBuilder.Entity<Service>(new ServiceMap().Configure);
            modelBuilder.Entity<User>(new UserMap().Configure);
        }

        public DbSet<User> Users { get; set; }
    }
}
