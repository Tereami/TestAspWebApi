using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestAspWebApi.Data;
using TestAspWebApi.Models;

namespace TestAspWebApi.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private const string _appName = "DesktopApp.exe";
        private readonly ApplicationDbContext _db;

        public CabinetController(UserManager<ApplicationUser> um, ApplicationDbContext db)
        {
            _userManager = um;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found");

            List<RefreshToken> tokens = await _db.RefreshTokens
                .Where(x => x.UserId == user.Id)
                .ToListAsync();

            List<UserSessionViewModel> sessions = tokens.Select(
                t => new UserSessionViewModel
                {
                    Id = t.Id,
                    ExpiresTime = t.ExpiresAt,
                    LoginTime = t.CreatedAt,
                    Active = !t.Revoked,
                }).ToList();


            CabinetViewModel model = new CabinetViewModel()
            {
                UserName = user.UserName!,
                RegisteredAt = user.RegisteredAt,
                Sessions = sessions,
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RevokeAllSessions()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found");
            List<RefreshToken> tokens = await _db.RefreshTokens
                .Where(x => x.UserId == user.Id)
                .ToListAsync();
            foreach (RefreshToken token in tokens)
                token.Revoked = true;

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Download()
        {
            string path = Path.Combine("Files", _appName);
            string fullPath = Path.GetFullPath(path);
            return PhysicalFile(fullPath, _appName);
        }
    }
}
