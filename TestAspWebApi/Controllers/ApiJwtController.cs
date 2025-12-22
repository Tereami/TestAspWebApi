using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TestAspWebApi.Models;
using TestAspWebApi.Services;

namespace TestAspWebApi.Controllers
{
    [Route("apiv2")]
    [ApiController]
    public class ApiJwtController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;

        public ApiJwtController(UserManager<ApplicationUser> um, JwtService js)
        {
            _userManager = um;
            _jwtService = js;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginJwtModel ljm)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(ljm.UserName);
            if (user == null)
                return Unauthorized();

            bool chackPassword = await _userManager.CheckPasswordAsync(user, ljm.Password);
            if (!chackPassword)
                return Unauthorized();

            string token = _jwtService.GenerateToken(user);

            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("date")]
        public async Task<IActionResult> Date()
        {
            var userName = User.Identity!.Name!;
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return Unauthorized();

            DateTime registered = user.RegisteredAt;
            return Ok(registered);
        }
    }
}
