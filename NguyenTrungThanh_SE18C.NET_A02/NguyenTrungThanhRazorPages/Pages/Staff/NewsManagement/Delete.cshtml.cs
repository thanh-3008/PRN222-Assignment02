using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using NguyenTrungThanhRazorPages.Hubs;
using System.Threading.Tasks;

namespace NguyenTrungThanhRazorPages.Pages.Staff.NewsManagement
{
    public class DeleteModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly IHubContext<NewsHub> _hubContext;

        public DeleteModel(INewsArticleService newsArticleService, IHubContext<NewsHub> hubContext)
        {
            _newsArticleService = newsArticleService;
            _hubContext = hubContext;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "1")
            {
                return RedirectToPage("/Account/Login");
            }

            if (id == null) return NotFound();

            var articleToDelete = _newsArticleService.GetNewsArticleById(id);
            if (articleToDelete != null)
            {
                _newsArticleService.DeleteNewsArticle(articleToDelete);
                await _hubContext.Clients.All.SendAsync("LoadNews");
            }

            return RedirectToPage("./Index");
        }
    }
}