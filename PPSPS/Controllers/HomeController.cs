using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Identity;
using PPSPS.Areas.Identity.Data;
using PPSPS.Data;

namespace PPSPS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly AuthDBContext _context;

        public HomeController(ILogger<StudentController> logger, AuthDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrator") || User.IsInRole("Directorate"))
            {
                return RedirectToAction("Index", "Administration");
            }

            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("Index", "Teacher");
            }

            if (User.IsInRole("Student"))
            {
                return RedirectToAction("Index", "Student");
            }

            return View();
        }
    }
}