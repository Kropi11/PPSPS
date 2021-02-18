using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PPSPS.Areas.Identity.Data;

namespace PPSPS.Areas.Identity.Pages.Account.Manage
{
    public class Disable2faModel : PageModel
    {
        private readonly UserManager<PPSPSUser> _userManager;
        private readonly ILogger<Disable2faModel> _logger;

        public Disable2faModel(
            UserManager<PPSPSUser> userManager,
            ILogger<Disable2faModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nelze načíst uživatele s ID '{_userManager.GetUserId(User)}'.");
            }

            if (!await _userManager.GetTwoFactorEnabledAsync(user))
            {
                throw new InvalidOperationException($"Nelze zakázat 2FA pro uživatele s ID '{_userManager.GetUserId(User)}', protože není aktuálně povoleno.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nelze načíst uživatele s ID '{_userManager.GetUserId(User)}'.");
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new InvalidOperationException($"Při zakázání 2FA pro uživatele s ID '{_userManager.GetUserId(User)}' došlo k neočekávané chybě.");
            }

            _logger.LogInformation("Pro uživatele s ID '{UserId}' byl zakázán 2fa.", _userManager.GetUserId(User));
            StatusMessage = "2fa byla deaktivována. 2fa můžete znovu povolit, když nastavíte aplikaci ověřovatele.";
            return RedirectToPage("./TwoFactorAuthentication");
        }
    }
}