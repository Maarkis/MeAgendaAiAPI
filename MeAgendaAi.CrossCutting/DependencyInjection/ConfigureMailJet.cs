using Mailjet.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeAgendaAi.CrossCutting.DependencyInjection
{
    public class ConfigureMailJet
    {
        public static void ConfigureDependecieMailJet(IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.AddHttpClient<MailjetClient, MailjetClient>(client =>
            {
                client.SetDefaultSettings();

                var mailJetConfiguration = configuration.GetSection("MailJetConfiguration");
                var apiKey = mailJetConfiguration.GetValue<string>("MJ_APIKEY_PUBLIC");
                var apiSecret = mailJetConfiguration.GetValue<string>("MJ_SECRETKEY_PRIVATE");

                client.UseBasicAuthentication(apiKey, apiSecret);
            });
        }
    }
}