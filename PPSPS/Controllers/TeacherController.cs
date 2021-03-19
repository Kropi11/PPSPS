using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PPSPS.Data;
using PPSPS.Models;

namespace PPSPS.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly ILogger<AdministrationController> _logger;
        private readonly AuthDBContext _context;

        public TeacherController(ILogger<AdministrationController> logger, AuthDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET
        public IActionResult TaskCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaskCreate([Bind("TaskName,Description,DateEntered,DateDeadline,ClassId,SubjectId,File")] PPSPSTask task)
        {
            task.Id = Guid.NewGuid().ToString();
            task.TeacherId = User.Identity.GetUserId<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(task);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(TaskCreate));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                             "Zkuste to znovu později, pokud problém přetrvává, " +
                                             "zkontaktujte svého správce systému.");
            }

            return View(task);
        }

        public async Task<IActionResult> TasksOverview()
        {
            var users = _context.Tasks
                .Include(c => c.Class)
                .Include(s => s.Subject)
                .AsNoTracking();
            return View(await users.ToListAsync());
        }

        public async Task<IActionResult> TaskEdit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost, ActionName("TaskEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaskEdit_Post(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskToUpdate = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (await TryUpdateModelAsync<PPSPSTask>(
                taskToUpdate,
                "",
                t => t.TaskName, t => t.Description, t => t.DateEntered, t => t.DateDeadline, t => t.ClassId, t => t.SubjectId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(TasksOverview));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                                 "Zkuste to znovu později a pokud problém přetrvává, " +
                                                 "zkontaktujte svého správce systému.");
                }
            }

            return View(taskToUpdate);
        }

        public async Task<IActionResult> AssignmentsOverview()
        {
            var assignment = _context.Assignments
                .Include(u => u.User)
                .Include(t => t.Task)
                .AsNoTracking();
            return View(await assignment.ToListAsync());
        }

        public async Task<IActionResult> AssignmentOverview(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments
                .Include(u => u.User)
                .Include(t => t.Task)

                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }
    }
}