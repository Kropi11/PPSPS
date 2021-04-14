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
                    .ThenInclude(s => s.Subject)
                    .Where(u => u.UserId == id)
                .OrderBy(t => t.Task.DateEntered)
                .AsNoTracking();
            return View(await assignment.ToListAsync());
        }

        public async Task<IActionResult> AssignmentsOverview()
        {
            var users = _context.Users
                .FindAsync(User.Identity.GetUserId<string>());

            var classes = _context.Classes
                .FirstOrDefaultAsync(m => m.Id == users.Result.ClassId);

            var tasks = _context.Tasks
                .Include(s => s.Subject)
                .Where(t => t.ClassId == classes.Result.ClassName)
                .Where(d => d.DateDeadline >= DateTime.Now)
                .OrderBy(t => t.DateEntered)
                .AsNoTracking();
            return View(await tasks.ToListAsync());
        }

        public async Task<IActionResult> TaskOverview(string? id)
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

        public async Task<IActionResult> AssignmentOverview(string? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignmentOverview(string? id, [Bind("Id, UserId, TaskId")] PPSPSAssignment assignment)
        {
            assignment.Id = Guid.NewGuid().ToString();
            assignment.UserId = User.Identity.GetUserId<string>();
            assignment.TaskId = id;
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(assignment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(SubmittedTasksOverview));
                }
            }
            catch (DbUpdateException  ex)
            {
                ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                             "Zkuste to znovu později a pokud problém přetrvává, " +
                                             "obraťte se na správce systému.");
            }

            return View();
        }
    }
}