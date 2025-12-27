using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TestAspWebApi.Data;
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
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public ApiJwtController(UserManager<ApplicationUser> um, JwtService js, ApplicationDbContext db, IConfiguration c)
        {
            _userManager = um;
            _jwtService = js;
            _db = db;
            _config = c;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginJwtModel ljm)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(ljm.UserName);
            bool checkPassword = await _userManager.CheckPasswordAsync(user, ljm.Password);
            if (user == null || !checkPassword)
                return Unauthorized();

            string accessToken = _jwtService.GenerateToken(user);
            var jwt = _config.GetSection("Jwt");
            int lifetimeDays = int.Parse(jwt["LifetimeDays"]!);

            RefreshToken refreshToken = new RefreshToken
            {
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(lifetimeDays),
            };

            _db.RefreshTokens.Add(refreshToken);
            await _db.SaveChangesAsync();


            return Ok(new { accessToken, refreshToken = refreshToken.Token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            RefreshToken? token = await _db.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                x.Token == refreshToken
                && !x.Revoked
                && x.ExpiresAt > DateTime.UtcNow);

            if (token == null)
                return Unauthorized();

            string newAccessToken = _jwtService.GenerateToken(token.User);
            return Ok(new { accessToken = newAccessToken });
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
