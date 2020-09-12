using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Data.Repository;
using MeAgendaAi.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MeAgendaAi.CrossCutting.DependencyInjection
{
    public class ConfigureRepository
    {
        public static void ConfigureDependenciesService(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<MeAgendaAiContext>(options =>
            {
                options.UseSqlServer("Password=4dm1n!;Persist Security Info=True;User ID=admin;Initial Catalog=meagendaai;Data Source=LAPTOP-CEI7D250\\SQLEXPRESS");
            });

            serviceCollection.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
