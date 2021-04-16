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
    [Authorize(Roles = "Administrator,Directorate")]
    public class AdministrationController : Controller
    {
        private readonly ILogger<AdministrationController> _logger;
        private readonly AuthDBContext _context;

        public AdministrationController(ILogger<AdministrationController> logger, AuthDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> UsersOverview()
        {
            var users = _context.Users
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
                s => s.FirstName, s => s.LastName, s => s.Email, s => s.PhoneNumber, s => s.EmailConfirmed, c => c.ClassId, g => g.GroupId))
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
            catch (DbUpdateException ex)
            {
                return RedirectToAction(nameof(UserDelete), new { id = id, saveChangesError = true });
            }
        }

        public async Task<IActionResult> TasksOverview()
        {
            var users = _context.Tasks
                .Include(s => s.Subject)
                .Include(g => g.Group)
                .Include(y => y.YearsOfStudies)
                .OrderByDescending(t => t.DateEntered)
                .AsNoTracking();

            PopulateClassesWithoutIdDropDownList();
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
                t => t.TaskName, t => t.Description, t => t.DateEntered, t => t.DateDeadline, t => t.ClassId, t => t.GroupId, t => t.SubjectId, t => t.YearsOfStudiesId))
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
            PopulateYearsOfStudiesDropDownList(taskToUpdate.YearsOfStudiesId);
            return View(taskToUpdate);
        }

        public async Task<IActionResult> ClassesOverview()
        {
            var classes = _context.Classes
                .Include(u => u.ClassTeacher)
                .OrderBy(c => c.ClassName)
                .AsNoTracking();
            return View(await classes.ToListAsync());
        }

        public IActionResult ClassCreate()
        {
            PopulateClassTeacherDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClassCreate([Bind("ClassName, YearOfEntry, ClassTeacherId")] PPSPSClass classes)
        {
            classes.Id = Guid.NewGuid().ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(classes);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ClassesOverview));
                }
            }
            catch (DbUpdateException  ex)
            {
                ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                             "Zkuste to znovu později a pokud problém přetrvává, " +
                                             "obraťte se na správce systému.");
            }

            PopulateClassTeacherDropDownList(classes.ClassTeacherId);
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

            PopulateClassTeacherDropDownList(classes.ClassTeacherId);
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
                c => c.ClassName, c => c.YearOfEntry, c => c.ClassTeacherId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ClassesOverview));
                }
                catch (DbUpdateException ex)
                {
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
                .Include(s => s.Subject)
                .Include(g => g.Group)
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
                .Include(u => u.ClassTeacher)
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
            catch (DbUpdateException ex)
            {
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
        public async Task<IActionResult> ClassOverview(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes
                .Include(t => t.ClassTeacher)
                .Include(u => u.User)
                    .Where(u => u.User.ClassId == id)
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
            var subject = _context.Subjects
                .OrderBy(s => s.SubjectAbbreviation)
                .AsNoTracking();

            return View(await subject.ToListAsync());
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
            catch (DbUpdateException ex)
            {
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
            catch (DbUpdateException ex)
            {
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
                catch (DbUpdateException ex)
                {
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
                .OrderByDescending(y => y.FirstSemester)
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
            catch (DbUpdateException ex)
            {
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
                catch (DbUpdateException ex)
                {
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
            catch (DbUpdateException ex)
            {
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

        public async Task<IActionResult> GroupsOverview()
        {
            var group = _context.Groups
                .OrderBy(s => s.GroupName)
                .AsNoTracking();

            return View(await group.ToListAsync());
        }

        public IActionResult GroupCreate()
        {
            return View();
        }

        public async Task<IActionResult> GroupOverview(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupCreate([Bind("GroupName, GroupAbbreviation")] PPSPSGroup group)
        {
            group.Id = Guid.NewGuid().ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(group);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(GroupsOverview));
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                             "Zkuste to znovu později a pokud problém přetrvává, " +
                                             "obraťte se na správce systému.");
            }

            return View(group);
        }

        public async Task<IActionResult> GroupEdit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups.FindAsync(id);
            if (groups == null)
            {
                return NotFound();
            }

            return View(groups);
        }

        [HttpPost, ActionName("GroupEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupEdit_Post(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupToUpdate = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if (await TryUpdateModelAsync<PPSPSGroup>(
                groupToUpdate,
                "",
                g => g.GroupName, g => g.GroupAbbreviation))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(GroupsOverview));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Nebylo možné uložit změny. " +
                                                 "Zkuste to znovu později a pokud problém přetrvává, " +
                                                 "obraťte se na správce systému.");
                }
            }

            return View(groupToUpdate);
        }

        public async Task<IActionResult> GroupDelete(string? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Groups
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

        [HttpPost, ActionName("GroupDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GroupDelete(string? id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return RedirectToAction(nameof(GroupsOverview));
            }

            try
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GroupsOverview));
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction(nameof(GroupDelete), new { id = id, saveChangesError = true });
            }
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

        private void PopulateClassTeacherDropDownList(object selectedClassTeacher = null)
        {
            var ClassTeacherQuery = from u in _context.Users
                orderby u.Email
                select u;
            ViewBag.ClassTeacherId =
                new SelectList(ClassTeacherQuery.AsNoTracking(), "Id", "Email", selectedClassTeacher);
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
