using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestAspWebApi.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestAspWebApi.Controllers
{
    [Route("apiv1")]
    [ApiController]
    public class ApiCookieController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApiCookieController(SignInManager<ApplicationUser> sm, UserManager<ApplicationUser> um)
        {
            _signInManager = sm;
            _userManager = um;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return NotFound($"Пользователь {model.UserName} не найден!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
                return Unauthorized();

            return Ok();
        }

        [HttpPost("date")]
        public async Task<IActionResult> Date([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return NotFound();

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
                return Unauthorized();

            return Ok($"Дата регистрации: {user.RegisteredAt}");
        }
    }
}
