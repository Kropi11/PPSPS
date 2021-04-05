using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            PopulateClassesDropDownList();
            PopulateSubjectDropDownList();
            PopulateYearsOfStudiesDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaskCreate([Bind("TaskName,Description,DateEntered,DateDeadline,ClassId,SubjectId,YearsOfStudiesId,File")] PPSPSTask task)
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

            PopulateClassesDropDownList(task.ClassId);
            PopulateSubjectDropDownList(task.SubjectId);
            PopulateYearsOfStudiesDropDownList(task.YearsOfStudiesId);
            return View(task);
        }

        public async Task<IActionResult> TasksOverview()
        {
            var tasks = _context.Tasks
                .Include(c => c.Class)
                .Include(s => s.Subject)
                .Include(y => y.YearsOfStudies)
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

            PopulateClassesDropDownList(task.ClassId);
            PopulateSubjectDropDownList(task.SubjectId);
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
                t => t.TaskName, t => t.Description, t => t.DateEntered, t => t.DateDeadline, t => t.ClassId, t => t.SubjectId, t => t.YearsOfStudiesId))
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

            PopulateClassesDropDownList(taskToUpdate.YearsOfStudiesId);
            PopulateSubjectDropDownList(taskToUpdate.YearsOfStudiesId);
            PopulateYearsOfStudiesDropDownList(taskToUpdate.YearsOfStudiesId);
            return View(taskToUpdate);
        }

        public async Task<IActionResult> AssignmentsOverview()
        {
            var assignment = _context.Assignments
                .Include(u => u.User)
                .Include(t => t.Task)

                .Where(t => t.Task.TeacherId == User.Identity.GetUserId<string>())
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
        private void PopulateClassesDropDownList(object selectedClass = null)
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
    }
}