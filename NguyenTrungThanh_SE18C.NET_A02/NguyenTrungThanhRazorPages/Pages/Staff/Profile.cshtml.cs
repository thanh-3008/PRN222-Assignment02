using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace NguyenTrungThanhRazorPages.Pages.Staff
{
    public class ProfileModel : PageModel
    {
        private readonly IAccountService _accountService;

        public ProfileModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public SystemAccount Account { get; set; } = default!;

        public IActionResult OnGet()
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "1")
            {
                return RedirectToPage("/Account/Login");
            }

            var accountIdString = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdString) || !short.TryParse(accountIdString, out var accountId))
            {
                return RedirectToPage("/Account/Login");
            }

            var account = _accountService.GetAccountById(accountId);
            if (account == null)
            {
                return NotFound();
            }
            Account = account;
            return Page();
        }

        public IActionResult OnPost()
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "1")
            {
                return RedirectToPage("/Account/Login");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingAccount = _accountService.GetAccountById(Account.AccountId);
            if (existingAccount == null)
            {
                return NotFound();
            }

            existingAccount.AccountName = Account.AccountName;

            _accountService.UpdateAccount(existingAccount);

            TempData["Message"] = "Profile updated successfully!";
            return RedirectToPage();
        }
    }
}