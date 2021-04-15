using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PPSPS.Areas.Identity.Data;
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

        public async Task<IActionResult> UsersOverview()
        {
            var users = _context.Users
                .Include(c => c.Class)
                .OrderBy(u => u.LastName)
                .AsNoTracking();

            return View(await users.ToListAsync());
        }

        public async Task<IActionResult> UserOverview(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(c => c.Class)
                .Include(g => g.Group)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> UserEdit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            PopulateClassesWithIdDropDownList(user.ClassId);
            PopulateGroupDropDownList(user.GroupId);
            return View(user);
        }

        [HttpPost, ActionName("UserEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit_Post(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userToUpdate = await _context.Users.FirstOrDefaultAsync(s => s.Id == id);
            if (await TryUpdateModelAsync<PPSPSUser>(
                userToUpdate,
                "",
                s => s.FirstName, s => s.LastName, s => s.Email, s => s.EmailConfirmed, c => c.ClassId, g => g.GroupId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(UsersOverview));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                                 "Zkuste to znovu později a pokud problém přetrvává, " +
                                                 "obraťte se na správce systému.");
                }
            }

            PopulateClassesWithIdDropDownList(userToUpdate.ClassId);
            return View(userToUpdate);
        }

        public IActionResult TaskCreate()
        {
            PopulateClassesWithoutIdDropDownList();
            PopulateSubjectDropDownList();
            PopulateGroupDropDownList();
            PopulateYearsOfStudiesDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaskCreate([Bind("TaskName,Description,DateEntered,DateDeadline,ClassId,SubjectId,GroupId,YearsOfStudiesId,File")] PPSPSTask task)
        {
            task.Id = Guid.NewGuid().ToString();
            task.TeacherId = User.Identity.GetUserId<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(task);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(TasksOverview));
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                             "Zkuste to znovu později, pokud problém přetrvává, " +
                                             "zkontaktujte svého správce systému.");
            }

            PopulateClassesWithoutIdDropDownList(task.ClassId);
            PopulateSubjectDropDownList(task.SubjectId);
            PopulateGroupDropDownList(task.GroupId);
            PopulateYearsOfStudiesDropDownList(task.YearsOfStudiesId);
            return View(task);
        }

        public async Task<IActionResult> TasksOverview()
        {
            var tasks = _context.Tasks
                .Include(s => s.Subject)
                .Include(g => g.Group)
                .Include(y => y.YearsOfStudies)
                    .Where(t => t.TeacherId == User.Identity.GetUserId<string>())
                .OrderByDescending(t => t.DateEntered)
                .AsNoTracking();

            return View(await tasks.ToListAsync());
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

            PopulateClassesWithoutIdDropDownList(task.ClassId);
            PopulateSubjectDropDownList(task.SubjectId);
            PopulateGroupDropDownList(task.GroupId);
            PopulateYearsOfStudiesDropDownList(task.YearsOfStudiesId);
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
                t => t.TaskName, t => t.Description, t => t.DateEntered, t => t.DateDeadline, t => t.ClassId, t => t.SubjectId, t => t.YearsOfStudiesId, t => t.File, t => t.GroupId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(TasksOverview));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                                 "Zkuste to znovu později a pokud problém přetrvává, " +
                                                 "zkontaktujte svého správce systému.");
                }
            }

            PopulateClassesWithoutIdDropDownList(taskToUpdate.ClassId);
            PopulateSubjectDropDownList(taskToUpdate.SubjectId);
            PopulateGroupDropDownList(taskToUpdate.GroupId);
            PopulateYearsOfStudiesDropDownList(taskToUpdate.YearsOfStudiesId);
            return View(taskToUpdate);
        }

        public async Task<IActionResult> TaskDelete(string? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .AsNoTracking()
                .Include(u => u.Teacher)
                .Include(s => s.Subject)
                .Include(y => y.YearsOfStudies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Smazání se nezdařilo. Zkuste to znovu později a pokud problém přetrvává, " +
                    "obraťte se na správce systému.";;
            }

            return View(task);
        }

        [HttpPost, ActionName("TaskDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaskDelete(string? id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return RedirectToAction(nameof(TasksOverview));
            }

            try
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TasksOverview));
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction(nameof(TaskDelete), new { id = id, saveChangesError = true });
            }
        }

        public async Task<IActionResult> AssignmentsOverview(string? id)
        {
            var assignment = _context.Assignments
                .Include(u => u.User)
                .Include(t => t.Task)
                    .ThenInclude(g => g.Group)
                    .Where(a => a.TaskId == id)
                .OrderBy(u => u.User.LastName)
                .AsNoTracking();
            return View(await assignment.ToListAsync());
        }

        public async Task<IActionResult>AssignmentOverview(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

             var assignment = await _context.Assignments
                .Include(u => u.User)
                    .ThenInclude(u => u.Class)
                .Include(u => u.User)
                    .ThenInclude(g => g.Group)
                .Include(t => t.Task)
                    .ThenInclude(g => g.Group)
                .Include(t => t.Task)
                    .ThenInclude(t => t.Teacher)
                .Include(t => t.Task)
                    .ThenInclude(s => s.Subject)
                .Include(t => t.Task)
                    .ThenInclude(y => y.YearsOfStudies)

                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        [HttpPost, ActionName("AssignmentOverview")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignmentOverview_Post(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentToUpdate = await _context.Assignments.FirstOrDefaultAsync(a => a.Id == id);
            if (await TryUpdateModelAsync<PPSPSAssignment>(
                assignmentToUpdate,
                "",
                a => a.Grade, a => a.Note))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(AssignmentOverview));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                                 "Zkuste to znovu později a pokud problém přetrvává, " +
                                                 "zkontaktujte svého správce systému.");
                }
            }

            return View(assignmentToUpdate);
        }

        private void PopulateClassesWithoutIdDropDownList(object selectedClass = null)
        {
            var classesQuery = from c in _context.Classes
                orderby c.ClassName
                select c;
            ViewBag.ClassId =
                new SelectList(classesQuery.AsNoTracking(), "ClassName", "ClassName", selectedClass);
        }

        private void PopulateClassesWithIdDropDownList(object selectedClass = null)
        {
            var classesQuery = from c in _context.Classes
                orderby c.ClassName
                select c;
            ViewBag.ClassId =
                new SelectList(classesQuery.AsNoTracking(), "Id", "ClassName", selectedClass);
        }

        private void PopulateSubjectDropDownList(object selectedSubject = null)
        {
            var subjectsQuery = from s in _context.Subjects
                orderby s.SubjectAbbreviation
                select s;
            ViewBag.SubjectId =
                new SelectList(subjectsQuery.AsNoTracking(), "Id", "SubjectAbbreviation", selectedSubject);
        }

        private void PopulateYearsOfStudiesDropDownList(object selectedYearsOfStudies = null)
        {
            var YearsQuery = from y in _context.YearsOfStudies
                orderby y.FirstSemester, y.SecondSemester
                select y;
            ViewBag.YearsOfStudiesId =
                new SelectList(YearsQuery.AsNoTracking(), "Id", "Years", selectedYearsOfStudies);
        }

        private void PopulateGroupDropDownList(object selectedGroup = null)
        {
            var GroupQuery = from g in _context.Groups
                orderby g.GroupAbbreviation
                select g;
            ViewBag.GroupId =
                new SelectList(GroupQuery.AsNoTracking(), "Id", "GroupAbbreviation", selectedGroup);
        }
    }
}