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

            modelBuilder.Entity<User>(new UserMap().Configure);
        }

        public DbSet<User> Users { get; set; }
    }
}
