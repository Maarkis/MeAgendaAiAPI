using Mailjet.Client;
using Mailjet.Client.Resources;
using MeAgendaAi.Domain.Entities.Email;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.Interfaces.Services.Email;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace MeAgendaAi.Service.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        private readonly string APIKEY;
        private readonly string APISECRET;


        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            IConfigurationSection mailJetConfiguration = _configuration.GetSection("MailJetConfiguration");
            APIKEY = mailJetConfiguration.GetValue<string>("MJ_APIKEY_PUBLIC");
            APISECRET = mailJetConfiguration.GetValue<string>("MJ_SECRETKEY_PRIVATE");
        }
        private MailjetClient newMailjetClient(string apiKey, string apiSecrte)
        {
            MailjetClient client = new MailjetClient(APIKEY, APISECRET)
            {
                BaseAdress = "https://api.mailjet.com/"

            };

            return client;
        }
        public async Task<bool> SendEmailConfirmartion(Domain.Entities.User user, EmailConfirmation emailConfirmation)
        {
            try
            {
                MailjetClient client = newMailjetClient(APIKEY, APISECRET);

                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource

                }
                .Property(Send.FromEmail, emailConfirmation.FromEmail)
                .Property(Send.FromName, emailConfirmation.FromName)
                .Property(Send.Subject, emailConfirmation.Subject)
                .Property(Send.Recipients, new JArray {
                    new JObject {
                        {"Email", user.Email},
                        {"Name", user.Name }
                    }
                })
                .Property(Send.MjTemplateID, 2288231)
                .Property(Send.MjTemplateLanguage, "True")
                .Property(Send.Vars, new JObject
                {
                    {"user_name", user.Name},
                    {"confirmation_link", GenerateURL(emailConfirmation.Url + "/confirmar-email", user.UserId.ToString())},

                });

                MailjetResponse resp = await client.PostAsync(request);

                if (resp.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<bool> SendRecoveryPassword(Domain.Entities.User user, EmailRetrievePassword emailRetrieve)
        { 
            try
            {
                MailjetClient client = newMailjetClient(APIKEY, APISECRET);

                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource

                }
                .Property(Send.FromEmail, emailRetrieve.FromEmail)
                .Property(Send.FromName, emailRetrieve.FromName)
                .Property(Send.Subject, emailRetrieve.Subject)
                .Property(Send.Recipients, new JArray {
                    new JObject {
                        {"Email", user.Email},
                        {"Name", user.Name }
                    }
                })
                .Property(Send.MjTemplateID, 2267909)
                .Property(Send.MjTemplateLanguage, "True")
                .Property(Send.Vars, new JObject
                {
                    {"user_name", user.Name},
                    {"link_reset", GenerateURL(emailRetrieve.Url + "/redefinir-senha", user.UserId.ToString(), emailRetrieve.Token) },
                    {"expiration",  emailRetrieve.Expiration}

                });

                MailjetResponse resp = await client.PostAsync(request);

                if (resp.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        private string GenerateURL(string urlPoral, string id,  string token = null)
        {          
            return new Uri(urlPoral + "/" + id + (token != null? "/" + token : "")).ToString();
        }
    }
}
