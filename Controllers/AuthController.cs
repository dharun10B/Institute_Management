using Institute_Management.Models;
using Institute_Management.Models.api;
using Institute_Management.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly JwtServices _jwtServices;
        public AuthController(JwtServices jwtServices)
        {
            _jwtServices = jwtServices;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginRespond>> Login(Loginmodules request)
        {
            var result = await _jwtServices.Authenticate(request);
            if (result is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View();
            }
            return View(result);
        }

        [AllowAnonymous]
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(Loginmodules request)
        {
            // TO DO: implement register logic here
            return View();
        }
    }
}