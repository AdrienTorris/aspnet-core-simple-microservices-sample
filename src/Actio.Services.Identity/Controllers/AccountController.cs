using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actio.Services.Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Actio.Common.Commands;

namespace Actio.Services.Identity.Controllers
{
    [Route("")]
    public class AccountController : Controller
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Login([FromBody]AuthenticateUser command)
            => Json(await this.userService.LoginAsync(command.Email,command.Password));
    }
}