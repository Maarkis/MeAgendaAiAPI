using System;
using System.Net.Http;
using MeAgendaAi.Application;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MeAgendaAi.Tests.Shared
{
    public abstract class SetUpIntegration
    {
        protected readonly HttpClient Client;

        protected SetUpIntegration()
        {
            var factory = new WebApplicationFactory<Startup>()
            {
                ClientOptions =
                {
                    AllowAutoRedirect = true,
                    BaseAddress = new Uri("https://localhost:5001")
                }
            };
            Client = factory.CreateClient();
        }
    }
}
