using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace NguyenTrungThanhRazorPages.Pages.Admin.AccountManagement
{
    public class CreateModel : PageModel
    {
        private readonly IAccountService _accountService;

        public CreateModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public SystemAccount Account { get; set; } = new();

        public IActionResult OnGet()
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "Admin")
            {
                return RedirectToPage("/Account/Login");
            }
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

            _accountService.CreateAccount(Account);

            return RedirectToPage("./Index");
        }
    }
}