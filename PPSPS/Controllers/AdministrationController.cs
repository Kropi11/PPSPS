#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> UsersOverview()
        {
            return View(await _context.Users.ToListAsync());
        }

        public async Task<IActionResult> UserOverview(string? id)
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
                s => s.FirstName, s => s.LastName, s => s.Email))
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
            return View(await _context.Tasks.ToListAsync());
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
                t => t.TaskName, t => t.Description, t => t.DateEntered, t => t.DateDeadline, t => t.Class))
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

        public async Task<IActionResult> ClassesOverview()
        {
            return View(await _context.Classes.ToListAsync());
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
                c => c.ClassName, c => c.ClassTeacher))
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
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

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
    }
}
