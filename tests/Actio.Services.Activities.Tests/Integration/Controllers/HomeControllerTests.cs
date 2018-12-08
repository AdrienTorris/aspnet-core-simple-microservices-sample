using Microsoft.AspNetCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;

namespace Actio.Services.Activities.Tests.Integration.Controllers
{
    public class HomeControllerTests
    {
        private readonly TestServer server;
        private readonly HttpClient httpClient;

        public HomeControllerTests()
        {
            this.server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>());

            this.httpClient = this.server.CreateClient();
        }

        [Fact]
        public async Task Home_controller_get_should_return_string_content()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync("/");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            content.Should().BeEquivalentTo("Hello from Actio.Services.Activities API!");
        }
    }
}