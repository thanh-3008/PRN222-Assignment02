using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace NguyenTrungThanhRazorPages.Pages.Admin.AccountManagement
{
    public class EditModel : PageModel
    {
        private readonly IAccountService _accountService;

        public EditModel(IAccountService accountService)
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

        public IActionResult OnPost()
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "Admin")
            {
                return RedirectToPage("/Account/Login");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _accountService.UpdateAccount(Account);

            return RedirectToPage("./Index");
        }
    }
}