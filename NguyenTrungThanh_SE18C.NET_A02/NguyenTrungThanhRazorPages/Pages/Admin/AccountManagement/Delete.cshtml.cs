using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace NguyenTrungThanhRazorPages.Pages.Admin.AccountManagement
{
    public class DeleteModel : PageModel
    {
        private readonly IAccountService _accountService;

        public DeleteModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public SystemAccount Account { get; set; } = default!;

        public IActionResult OnGet(short id)
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "Admin")
            {
                return RedirectToPage("/Account/Login");
            }

            var account = _accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            Account = account;
            return Page();
        }

        public IActionResult OnPost(short id)
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "Admin")
            {
                return RedirectToPage("/Account/Login");
            }

            var accountToDelete = _accountService.GetAccountById(id);
            if (accountToDelete == null)
            {
                return NotFound();
            }

            _accountService.DeleteAccount(accountToDelete);

            return RedirectToPage("./Index");
        }
    }
}