using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<MeAgendaAiContext>
    {

        public MeAgendaAiContext CreateDbContext(string[] args)
        {
            var connectionString = "Password=4dm1n!;Persist Security Info=True;User ID=admin;Initial Catalog=meagendaai;Data Source=LAPTOP-CEI7D250\\SQLEXPRESS";
            var optionsBuilder = new DbContextOptionsBuilder<MeAgendaAiContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new MeAgendaAiContext(optionsBuilder.Options);
        }
    }
}
