using MeAgendaAi.Data.Context;
using MeAgendaAi.Data.Repository;
using MeAgendaAi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
