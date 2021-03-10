using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PPSPS.Areas.Identity.Data;
using PPSPS.Data;
using PPSPS.Models;

namespace PPSPS.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly AuthDBContext _context;
        private readonly UserManager<PPSPSUser> _userManager;

        public StudentController(ILogger<StudentController> logger, AuthDBContext context, UserManager<PPSPSUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}