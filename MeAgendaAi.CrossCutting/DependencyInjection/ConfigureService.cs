using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MeAgendaAi.CrossCutting.DependencyInjection
{
    public class ConfigureService
    {
        public static void ConfigureDependenciesService (IServiceCollection serviceCollection)
        {
            // Services
            serviceCollection.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IClientService, ClientService>();
            serviceCollection.AddScoped<IEmployeeService, EmployeeService>();
            serviceCollection.AddScoped<ICompanyService, CompanyService>();
            serviceCollection.AddScoped<ISchedulingService, SchedulingService>();
            serviceCollection.AddScoped<ILocationService, LocationService>();
            serviceCollection.AddScoped<IEmployeeWorkHoursService, EmployeeWorkHoursService>();


            serviceCollection.AddCors(options =>
            {
                //options.AddPolicy("MyAllowSpecificOrigins",
                //        builder =>           {
                //            builder.WithOrigins("http://localhost:44316",
                //                                "http://localhost:4200"
                //                                )
                //                                .AllowAnyHeader()
                //                                .AllowAnyMethod()
                //                                .AllowAnyOrigin();
                //        });

                options.AddPolicy("MyAllowSpecificOrigins", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });

            });

        }
    }
}
