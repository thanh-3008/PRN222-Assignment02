using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NguyenTrungThanhRazorPages.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public LoginModel(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            string userRole = null;
            string userEmail = null;
            string accountId = null;
            string accountName = null;

            if (Email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) && Password.Equals(adminPassword))
            {
                userEmail = adminEmail;
                userRole = "Admin";
                accountId = "-1";
                accountName = "System Admin";
            }
            else
            {
                var dbAccount = _accountService.GetAccountByEmail(Email);

                if (dbAccount != null && dbAccount.AccountPassword == Password)
                {
                    userEmail = dbAccount.AccountEmail;
                    userRole = dbAccount.AccountRole?.ToString();
                    accountId = dbAccount.AccountId.ToString();
                    accountName = dbAccount.AccountName;
                }
            }

            if (userRole == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            HttpContext.Session.SetString("Email", userEmail);
            HttpContext.Session.SetString("Role", userRole);
            HttpContext.Session.SetString("AccountId", accountId);
            HttpContext.Session.SetString("Name", accountName);

            switch (userRole)
            {
                case "Admin":
                    return RedirectToPage("/Admin/AccountManagement/Index");
                case "1": 
                    return RedirectToPage("/Staff/NewsManagement/Index");
                case "2": 
                    return RedirectToPage("/Index");
                default:
                    return RedirectToPage("/Index");
            }
        }
    }
}