using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace NguyenTrungThanhRazorPages.Pages.Staff.NewsManagement
{
    public class DetailsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public DetailsModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        public NewsArticle NewsArticle { get; set; } = default!;

        public IActionResult OnGet(string id)
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "1")
            {
                return RedirectToPage("/Account/Login");
            }

            if (id == null) return NotFound();

            var article = _newsArticleService.GetNewsArticleById(id);
            if (article == null) return NotFound();

            NewsArticle = article;
            return Page();
        }
    }
}