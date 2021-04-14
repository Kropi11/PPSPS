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

        public IActionResult Index()
        {
            return View();
        }
    }
}