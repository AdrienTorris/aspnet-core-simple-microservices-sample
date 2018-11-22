namespace Actio.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult GetAction() => Content("Hi!");
    }
}