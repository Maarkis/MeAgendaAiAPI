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
        public static void ConfigureDependenciesService(IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<MeAgendaAiContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            serviceCollection.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IClientRepository, ClientRepository>();
            serviceCollection.AddScoped<IEmployeeRepository, EmployeeRepository>();
            serviceCollection.AddScoped<ICompanyRepository, CompanyRepository>();
            serviceCollection.AddScoped<IServiceRepository, ServiceRepository>();
            serviceCollection.AddScoped<IServiceEmployeeRepository, ServiceEmployeeRepository>();
            serviceCollection.AddScoped<IPolicyRepository, PolicyRepository>();
            serviceCollection.AddScoped<ISchedulingRepository, SchedulingRepository>();
        }
    }
}
