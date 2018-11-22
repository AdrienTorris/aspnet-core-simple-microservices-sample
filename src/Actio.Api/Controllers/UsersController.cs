namespace Actio.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
     using  RawRabbit;
     using System;
     using Actio.Common.Commands;
     using System.Threading.Tasks;

    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IBusClient _busClient;

        public UsersController(IBusClient busClient){
            this._busClient=busClient;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Post([FromBody]CreateUser command)
        {
            await this._busClient.PublishAsync(command);

            return Accepted();
        }
    }
}