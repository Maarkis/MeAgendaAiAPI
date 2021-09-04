using System;
using System.Net;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Tests.Shared;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace MeAgendaAi.Test.Integration
{
    public class AuthenticationControllerTest : SetUpIntegration
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public AuthenticationControllerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async void ShouldPassUserAndPasswordAndReturnStatusCode200()
        {
            var user = new LoginModel()
            {
                Email = "jeanmarkis85@gmail.com",
                Senha = "123456789"
            };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/Authentication", content);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void ShouldPassInvalidUserAndPasswordAndReturnStatusCodeNotFound()
        {
            var user = new LoginModel()
            {
                Email = "jeanmarkis@gmail.com",
                Senha = "123456789"
            };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var request = await Client.PostAsync("/api/Authentication", content);

            request.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void ShouldPassInvalidUserAndPasswordAndResponseUserNotFound()
        {
            const string requestResponse = "Usuário não cadastrado";
            var user = new LoginModel()
            {
                Email = "jeanmarkis@gmail.com",
                Senha = "123456789"
            };
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var request = await Client.PostAsync("/api/Authentication", content);

            var result = await request.Content.ReadAsStringAsync();

            result.Should().BeEquivalentTo(requestResponse);
        }
    }
}
