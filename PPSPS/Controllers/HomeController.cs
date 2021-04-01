using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PPSPS.Areas.Identity.Data;
using PPSPS.Data;

namespace PPSPS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly AuthDBContext _context;
        private readonly UserManager<PPSPSUser> _userManager;

        public HomeController(ILogger<StudentController> logger, AuthDBContext context, UserManager<PPSPSUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        // GET
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserOverview()
        {
            string id = _userManager.GetUserId(User);
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(c => c.Class)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}