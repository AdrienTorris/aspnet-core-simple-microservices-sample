namespace Actio.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
     using  RawRabbit;
     using System;
     using Actio.Common.Commands;
     using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Actio.Api.Repositories;
    using System.Linq;

    [Route("[controller]")]
    [Authorize(AuthenticationSchemes= JwtBearerDefaults.AuthenticationScheme)]
    public class ActivitiesController : Controller
    {
        private readonly IBusClient _busClient;
        private readonly IActivityRepository activityRepository; // It's a sample. In a more realistic scenario, it should be an application service here.

        public ActivitiesController(IBusClient busClient,IActivityRepository activityRepository){
            this._busClient=busClient;
            this.activityRepository=activityRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CreateActivity command)
        {
            Console.WriteLine("Post");
            Console.WriteLine(command);

            command.Id=Guid.NewGuid();
            command.CreatedAt = DateTime.Now;
            command.UserId = Guid.Parse(User.Identity.Name);
            
            await this._busClient.PublishAsync(command);

            return Accepted($"activities/{command.Id}");
        }

        [HttpGet("")]
        public async Task<IActionResult> Get() 
        {
            var activities = await this.activityRepository.BrowseAsync(Guid.Parse(User.Identity.Name));

            return  Json(activities.Select(x=>new{x.Id, x.Name,x.Category,x.CreatedAt}));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id) 
        {
            var activity = await this.activityRepository.GetAsync(id);
            if(activity==null){
                return NotFound();
            }
            if(activity.UserId!=Guid.Parse(User.Identity.Name))
            {
                return Unauthorized();
            }

            return  Json(activity);
        }
    }
}