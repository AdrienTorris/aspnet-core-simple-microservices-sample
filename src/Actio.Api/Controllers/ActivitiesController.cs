namespace Actio.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
     using  RawRabbit;
     using System;
     using Actio.Common.Commands;
     using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authentication.JwtBearer;

    [Route("[controller]")]
    public class ActivitiesController : Controller
    {
        private readonly IBusClient _busClient;

        public ActivitiesController(IBusClient busClient){
            this._busClient=busClient;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CreateActivity command)
        {
            Console.WriteLine("Post");
            Console.WriteLine(command);

            command.Id=Guid.NewGuid();
            command.CreatedAt = DateTime.Now;
            
            await this._busClient.PublishAsync(command);

            return Accepted($"activities/{command.Id}");
        }

        [HttpGet("")]
        [Authorize(AuthenticationSchemes= JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get() => Content("Secured");
    }
}