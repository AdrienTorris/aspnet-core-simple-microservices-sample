namespace Actio.Api.Tests.Unit.Controllers
{
    using Actio.Api.Controllers;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public class HomeControllerTests
    {
        [Fact]
        public void Home_controller_get_should_return_string_content()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            var result = controller.Get();

            // Assert
            var contentResult = result as ContentResult;
            contentResult.Should().NotBeNull();
            contentResult.Content.Should().BeEquivalentTo("Hi!");
        }   
    }
}