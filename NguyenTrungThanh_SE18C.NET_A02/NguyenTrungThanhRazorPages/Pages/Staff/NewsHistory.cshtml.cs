using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace NguyenTrungThanhRazorPages.Pages.Staff
{
    public class NewsHistoryModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsHistoryModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        public IList<NewsArticle> NewsArticles { get; set; } = default!;

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

            NewsArticles = _newsArticleService.GetNewsArticlesByAccountId(accountId);
            return Page();
        }
    }
}