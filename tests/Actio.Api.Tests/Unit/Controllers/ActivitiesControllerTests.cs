namespace Actio.Api.Tests.Unit.Controllers
{
    using Actio.Api.Controllers;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;
    using Moq;
    using RawRabbit;
    using Actio.Api.Repositories;
    using System;
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;
    using Actio.Common.Commands;
    using System.Threading.Tasks;

    public class ActivitiesControllerTests
    {
        [Fact]
        public async Task Activities_controller_post_should_return_accepted()
        {
            var busClientMock = new Mock<IBusClient>();
            var activityRepositoryMock = new Mock<IActivityRepository>();
            var controller = new ActivitiesController(busClientMock.Object,
            activityRepositoryMock.Object);
            Guid userId = Guid.NewGuid();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(
                        new Claim[]{ new Claim(ClaimTypes.Name, userId.ToString())}
                    , "test" ))
                }
            };

            var command = new CreateActivity{
                Id = Guid.NewGuid(),
                UserId = userId
            };

            var result = await controller.Post(command);

            var contentResult = result as AcceptedResult;
            contentResult.Should().NotBeNull();
            contentResult.Location.Should().BeEquivalentTo($"Activities/{command.Id}");
        }
    }
}