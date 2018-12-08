using Actio.Services.Activities.Domain.Models;
using Actio.Services.Activities.Domain.Repositories;
using Actio.Services.Activities.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Actio.Services.Activities.Tests.Unit.Services
{
    public class ActivityServiceTests
    {
        [Fact]
        public async Task Activity_service_add_async_should_succeed()
        {
            string category = "test";
            Mock<IActivityRepository> activityRepositoryMock = new Mock<IActivityRepository>();
            Mock<ICategoryRepository> categoryRepositoryMock = new Mock<ICategoryRepository>();
            categoryRepositoryMock
                .Setup(x=>x.GetAsync(category))
                .ReturnsAsync(new Category(category));
                ActivityService activityService = new ActivityService(activityRepositoryMock.Object,
                categoryRepositoryMock.Object);

            Guid id = Guid.NewGuid();
            await activityService.AddAsync(id, Guid.NewGuid(), category, 
            "activity", "description", DateTime.UtcNow);

            categoryRepositoryMock.Verify(x=>x.GetAsync(category), Times.Once);
            activityRepositoryMock.Verify(x=> x.AddAsync(It.IsAny<Activity>()), Times.Once);
        }
    }
}