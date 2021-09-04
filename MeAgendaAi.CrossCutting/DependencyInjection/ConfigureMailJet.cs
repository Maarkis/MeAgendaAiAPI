using Mailjet.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeAgendaAi.CrossCutting.DependencyInjection
{


    public class ConfigureMailJet
    {
        public static void ConfigureDependecieMailJet(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddHttpClient<MailjetClient, MailjetClient>(client =>
            {
                client.SetDefaultSettings();

                IConfigurationSection mailJetConfiguration = configuration.GetSection("MailJetConfiguration");
                string apiKey = mailJetConfiguration.GetValue<string>("MJ_APIKEY_PUBLIC");
                string apiSecret = mailJetConfiguration.GetValue<string>("MJ_SECRETKEY_PRIVATE");

                client.UseBasicAuthentication(apiKey, apiSecret);
            });
        }
    }
}

