#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PPSPS.Areas.Identity.Data;
using PPSPS.Data;
using PPSPS.Models;

namespace PPSPS.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        private readonly ILogger<AdministrationController> _logger;
        private readonly AuthDBContext _context;

        public AdministrationController(ILogger<AdministrationController> logger, AuthDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> UsersOverview(string sortOrder)
        {

            var users = _context.Users
                .Include(c => c.Class)
                .AsNoTracking();

            ViewData["LastNameSortParm"] = String.IsNullOrEmpty(sortOrder);
            users = users.OrderBy(u => u.LastName);

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

            PopulateClassesDropDownList(user.ClassId);
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
                s => s.FirstName, s => s.LastName, s => s.Email, s => s.EmailConfirmed, c => c.ClassId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(UsersOverview));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                                 "Zkuste to znovu později a pokud problém přetrvává, " +
                                                 "obraťte se na správce systému.");
                }
            }

            PopulateClassesDropDownList(userToUpdate.ClassId);
            return View(userToUpdate);
        }

        public async Task<IActionResult> UserDelete(string? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Smazání se nezdařilo. Zkuste to znovu později a pokud problém přetrvává, " +
                    "obraťte se na správce systému.";
            }

            return View(user);
        }

        [HttpPost, ActionName("UserDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserDelete_Post(string? id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return RedirectToAction(nameof(UsersOverview));
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(UsersOverview));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(UserDelete), new { id = id, saveChangesError = true });
            }
        }

        public async Task<IActionResult> TasksOverview()
        {
            var users = _context.Tasks
                .Include(c => c.Class)
                .Include(s => s.Subject)
                .Include(y => y.YearsOfStudies)
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
                t => t.TaskName, t => t.Description, t => t.DateEntered, t => t.DateDeadline, t => t.ClassId, t => t.SubjectId, t => t.YearsOfStudiesId, t => t.File))
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

            PopulateClassesDropDownList(taskToUpdate.ClassId);
            PopulateSubjectDropDownList(taskToUpdate.SubjectId);
            PopulateYearsOfStudiesDropDownList(taskToUpdate.YearsOfStudiesId);
            return View(taskToUpdate);
        }

        public async Task<IActionResult> ClassesOverview()
        {
            var classes = _context.Classes
                //.Include(u => u.ClassTeacher)
                .AsNoTracking();
            return View(await classes.ToListAsync());
        }

        public IActionResult ClassCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClassCreate([Bind("ClassName, ClassTeacher")] PPSPSClass classes)
        {
            classes.Id = Guid.NewGuid().ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(classes);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ClassCreate));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                             "Zkuste to znovu později a pokud problém přetrvává, " +
                                             "obraťte se na správce systému.");
            }

            return View(classes);
        }
        public async Task<IActionResult> ClassEdit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes.FindAsync(id);
            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        [HttpPost, ActionName("ClassEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClassEdit_Post(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classToUpdate = await _context.Classes.FirstOrDefaultAsync(c => c.Id == id);
            if (await TryUpdateModelAsync<PPSPSClass>(
                classToUpdate,
                "",
                c => c.ClassName, c => c.ClassTeacherId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ClassesOverview));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                                 "Zkuste to znovu později a pokud problém přetrvává, " +
                                                 "obraťte se na správce systému.");
                }
            }

            return View(classToUpdate);
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
                .Include(y => y.YearsOfStudies)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        public async Task<IActionResult> ClassDelete(string? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classes == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Smazání se nezdařilo. Zkuste to znovu později a pokud problém přetrvává, " +
                    "obraťte se na správce systému.";;
            }

            return View(classes);
        }
        // POST: Students/Delete/5
        [HttpPost, ActionName("ClassDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClassDelete(string? id)
        {
            var student = await _context.Classes.FindAsync(id);
            if (student == null)
            {
                return RedirectToAction(nameof(ClassesOverview));
            }

            try
            {
                _context.Classes.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ClassesOverview));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(ClassDelete), new { id = id, saveChangesError = true });
            }
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
                .Include(c => c.Class)
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
        // POST: Students/Delete/5
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
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(TaskDelete), new { id = id, saveChangesError = true });
            }
        }
        public async Task<IActionResult> ClassOverview(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }
        public async Task<IActionResult> SubjectsOverview()
        {
            return View(await _context.Subjects.ToListAsync());
        }

        public IActionResult SubjectCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubjectCreate([Bind("SubjectName, SubjectAbbreviation")] PPSPSSubject subject)
        {
            subject.Id = Guid.NewGuid().ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(subject);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(SubjectsOverview));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                             "Zkuste to znovu později a pokud problém přetrvává, " +
                                             "obraťte se na správce systému.");
            }

            return View(subject);
        }

        public async Task<IActionResult> SubjectDelete(string? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Smazání se nezdařilo. Zkuste to znovu později a pokud problém přetrvává, " +
                    "obraťte se na správce systému.";;
            }

            return View(subject);
        }
        // POST: Students/Delete/5
        [HttpPost, ActionName("SubjectDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubjectDelete(string? id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return RedirectToAction(nameof(SubjectsOverview));
            }

            try
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(SubjectsOverview));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(SubjectDelete), new { id = id, saveChangesError = true });
            }
        }
        public async Task<IActionResult> SubjectEdit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        [HttpPost, ActionName("SubjectEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubjectEdit_Post(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectToUpdate = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id);
            if (await TryUpdateModelAsync<PPSPSSubject>(
                subjectToUpdate,
                "",
                s => s.SubjectName, s => s.SubjectAbbreviation))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(SubjectsOverview));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                                 "Zkuste to znovu později a pokud problém přetrvává, " +
                                                 "obraťte se na správce systému.");
                }
            }

            return View(subjectToUpdate);
        }
        public async Task<IActionResult> SubjectOverview(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }
        public async Task<IActionResult> YearsOfStudiesOverview()
        {
            var years = _context.YearsOfStudies
                .AsNoTracking();
            return View(await years.ToListAsync());
        }

        public IActionResult YearOfStudiesCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> YearOfStudiesCreate([Bind("FirstSemester, SecondSemester")] PPSPSYearsOfStudies years)
        {
            years.Id = Guid.NewGuid().ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(years);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(YearsOfStudiesOverview));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                             "Zkuste to znovu později a pokud problém přetrvává, " +
                                             "obraťte se na správce systému.");
            }

            return View(years);
        }

        public async Task<IActionResult> YearOfStudiesEdit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var years = await _context.YearsOfStudies.FindAsync(id);
            if (years == null)
            {
                return NotFound();
            }

            return View(years);
        }

        [HttpPost, ActionName("YearOfStudiesEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> YearOfStudiesEdit_Post(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yearsToUpdate = await _context.YearsOfStudies.FirstOrDefaultAsync(y => y.Id == id);
            if (await TryUpdateModelAsync<PPSPSYearsOfStudies>(
                yearsToUpdate,
                "",
                y => y.FirstSemester, y => y.SecondSemester))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(YearsOfStudiesOverview));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                                 "Zkuste to znovu později a pokud problém přetrvává, " +
                                                 "obraťte se na správce systému.");
                }
            }

            return View(yearsToUpdate);
        }

        public async Task<IActionResult> YearOfStudiesDelete(string? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var years = await _context.YearsOfStudies
                .AsNoTracking()
                .FirstOrDefaultAsync(y => y.Id == id);
            if (years == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Smazání se nezdařilo. Zkuste to znovu později a pokud problém přetrvává, " +
                    "obraťte se na správce systému.";;
            }

            return View(years);
        }
        // POST: Students/Delete/5
        [HttpPost, ActionName("YearOfStudiesDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> YearOfStudiesDelete(string? id)
        {
            var years = await _context.YearsOfStudies.FindAsync(id);
            if (years == null)
            {
                return RedirectToAction(nameof(YearsOfStudiesOverview));
            }

            try
            {
                _context.YearsOfStudies.Remove(years);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(YearsOfStudiesOverview));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(YearOfStudiesDelete), new { id = id, saveChangesError = true });
            }
        }

        public async Task<IActionResult> YearOfStudiesOverview(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var years = await _context.YearsOfStudies
                .AsNoTracking()
                .FirstOrDefaultAsync(y => y.Id == id);

            if (years == null)
            {
                return NotFound();
            }

            return View(years);
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
