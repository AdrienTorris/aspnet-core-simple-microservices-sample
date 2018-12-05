namespace Actio.Services.Activities.Services
{
    using Actio.Common.Exceptions;
    using Actio.Services.Activities.Domain.Models;
    using Actio.Services.Activities.Domain.Repositories;
    using System;
    using System.Threading.Tasks;

    public sealed class ActivityService : IActivityService
    {
        private readonly IActivityRepository activityRepository;
        private readonly ICategoryRepository categoryRepository;

        public ActivityService(IActivityRepository activityRepository,
        ICategoryRepository categoryRepository)
        {
            this.activityRepository = activityRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task AddAsync(Guid id, Guid userId, string category,
            string name, string description, DateTime createdAt)
        {
            var activityCategory = await this.categoryRepository.GetAsync(category);
            if (activityCategory == null)
            {
                throw new ActioException("category_not_found", $"category: '{category}' was not found");
            }

            var activity = new Activity(id, activityCategory, userId, name, description, createdAt);

            await this.activityRepository.AddAsync(activity);
        }
    }
}