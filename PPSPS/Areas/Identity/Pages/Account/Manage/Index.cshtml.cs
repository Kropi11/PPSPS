using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPSPS.Areas.Identity.Data;
using PPSPS.Data;

namespace PPSPS.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<PPSPSUser> _userManager;
        private readonly SignInManager<PPSPSUser> _signInManager;
        private readonly AuthDBContext _context;


        public IndexModel(
            UserManager<PPSPSUser> userManager,
            SignInManager<PPSPSUser> signInManager,
            AuthDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [Display(Name = "E-mail / Uživatelské jméno")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Křestní jméno")]
            public string FirstName { get; set; }

            [Display(Name = "Příjmení")]
            public string LastName { get; set; }

            [Display(Name = "Třída")]
            public string ClassId { get; set; }

            [Display(Name = "Třída")]
            public string ClassName { get; set; }

            [Phone]
            [Display(Name = "Telefonní číslo")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(PPSPSUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ClassId = user.ClassId,
                PhoneNumber = phoneNumber
            };
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nelze načíst uživatele s ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nelze načíst uživatele s ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Při pokusu o nastavení telefonního čísla došlo k neočekávané chybě.";
                    return RedirectToPage();
                }
            }
            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Váš profil byl aktualizován";

            return RedirectToPage();
        }
    }
}
