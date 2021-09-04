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
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=MeAgendaAi;Trusted_Connection=True;";
            var optionsBuilder = new DbContextOptionsBuilder<MeAgendaAiContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new MeAgendaAiContext(optionsBuilder.Options);
        }
    }
}