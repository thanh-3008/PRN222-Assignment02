using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NguyenTrungThanhRazorPages.Pages.Admin.AccountManagement
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IList<SystemAccount> Accounts { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty]
        public SystemAccount Account { get; set; }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        public IActionResult OnGet()
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");

            Accounts = _accountService.GetAccounts(SearchTerm);
            return Page();
        }

        public IActionResult OnGetAccount(short id)
        {
            if (!IsAdmin()) return Unauthorized();

            var account = _accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }

            var result = new
            {
                accountId = account.AccountId,
                accountName = account.AccountName,
                accountEmail = account.AccountEmail,
                accountRole = account.AccountRole
            };

            return new JsonResult(result);
        }

        public IActionResult OnPostCreate()
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");

            if (!ModelState.IsValid)
            {
                Accounts = _accountService.GetAccounts(SearchTerm);
                return Page();
            }

            _accountService.CreateAccount(Account);
            return RedirectToPage("./Index");
        }

        public IActionResult OnPostEdit()
        {
            if (!IsAdmin()) return RedirectToPage("/Account/Login");

            ModelState.Remove("Account.AccountPassword");

            if (!ModelState.IsValid)
            {
                Accounts = _accountService.GetAccounts(SearchTerm);
                return Page();
            }

            _accountService.UpdateAccount(Account);
            return RedirectToPage("./Index");
        }
    }
}