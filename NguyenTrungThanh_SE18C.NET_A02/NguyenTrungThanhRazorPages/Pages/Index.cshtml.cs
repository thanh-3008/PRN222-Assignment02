using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces; 

namespace NguyenTrungThanhRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly INewsArticleService _newsArticleService; 

        public IndexModel(ILogger<IndexModel> logger, INewsArticleService newsArticleService)
        {
            _logger = logger;
            _newsArticleService = newsArticleService;
        }

        public List<NewsArticle> NewsArticleList { get; set; } = new List<NewsArticle>();

        public void OnGet()
        {
            NewsArticleList = _newsArticleService.GetNewsArticles();
        }
    }
}