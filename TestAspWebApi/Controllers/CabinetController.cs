using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using TestAspWebApi.Models;

namespace TestAspWebApi.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private const string _appName = "DesktopApp.exe";

        public CabinetController(UserManager<ApplicationUser> um)
        {
            _userManager = um;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            CabinetViewModel model = new CabinetViewModel()
            {
                UserName = user.UserName,
                RegisteredAt = user.RegisteredAt
            };

            return View(model);
        }

        public IActionResult Download()
        {
            string path = Path.Combine("Files", _appName);
            string fullPath = Path.GetFullPath(path);
            return PhysicalFile(fullPath, _appName);
        }
    }
}
