namespace Actio.Api.Controllers
{
    using Actio.Common.Commands;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using RawRabbit;
    using System.Threading.Tasks;

    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IBusClient _busClient;
        private ILogger logger;

        public UsersController(IBusClient busClient, ILogger<UsersController> logger)
        {
            this._busClient = busClient;
            this.logger = logger;
        }

        //[HttpPost("Register")]
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CreateUser command)
        {
            if (command == null)
            {
                this.logger.LogTrace("command is null");
            }
            if (string.IsNullOrWhiteSpace(command.Email))
            {
                this.logger.LogTrace("email is IsNullOrWhiteSpace");
            }
            this.logger.LogTrace($"Command: {command.Email} {command.Name} {command.Password}");

            await this._busClient.PublishAsync(command);

            this.logger.LogTrace("Event published");

            return Accepted();
        }
    }
}