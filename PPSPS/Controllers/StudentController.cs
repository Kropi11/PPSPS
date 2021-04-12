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
using Microsoft.AspNet.Identity;
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

        public StudentController(ILogger<StudentController> logger, AuthDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public async Task<IActionResult> SubmittedTasksOverview()
        {
            string id = User.Identity.GetUserId<string>();
            var assignment = _context.Assignments
                .Include(t => t.Task)
                    .Where(u => u.UserId == id)
                .OrderBy(t => t.Task.DateEntered)
                .AsNoTracking();
            return View(await assignment.ToListAsync());
        }

        public async Task<IActionResult> AssignmentsOverview()
        {
            var tasks = _context.Tasks
                    .Where(t => t.ClassId == "B4.I")
                    .Where(y => y.YearsOfStudiesId == "5605981e-bc24-4a43-96fe-df13f688e194")
                .OrderBy(t => t.DateEntered)
                .AsNoTracking();
            return View(await tasks.ToListAsync());
        }

        public async Task<IActionResult> AssignmentOverview(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(u => u.Teacher)
                .Include(c => c.Class)
                .Include(s => s.Subject)
                .Include(a => a.Assignment)
                .Include(y => y.YearsOfStudies)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        public async Task<IActionResult> TaskOverview(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(u => u.Teacher)
                .Include(s => s.Subject)
                .Include(y => y.YearsOfStudies)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }
    }
}