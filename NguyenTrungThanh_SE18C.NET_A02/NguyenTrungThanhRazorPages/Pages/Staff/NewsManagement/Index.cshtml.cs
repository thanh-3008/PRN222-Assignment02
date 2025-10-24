using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NguyenTrungThanhRazorPages.Hubs;

namespace NguyenTrungThanhRazorPages.Pages.Staff.NewsManagement
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IHubContext<NewsHub> _hubContext;

        public IndexModel(
            INewsArticleService newsArticleService,
            ICategoryService categoryService,
            ITagService tagService,
            IHubContext<NewsHub> hubContext)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _hubContext = hubContext;
        }

        public IList<NewsArticle> NewsArticles { get; set; }
        public SelectList CategoryList { get; set; }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; }

        [BindProperty]
        public string TagsString { get; set; }

        public IActionResult OnGet()
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "1")
            {
                return RedirectToPage("/Account/Login");
            }

            NewsArticles = _newsArticleService.GetNewsArticles();
            CategoryList = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName");

            return Page();
        }

        public IActionResult OnGetArticle(string id)
        {
            var article = _newsArticleService.GetNewsArticleById(id);
            if (article == null)
            {
                return NotFound();
            }

            var tagsString = string.Join(", ", article.NewsTags.Select(t => t.Tag.TagName));

            var result = new
            {
                newsArticleId = article.NewsArticleId,
                newsTitle = article.NewsTitle,
                headline = article.Headline,
                newsContent = article.NewsContent,
                categoryId = article.CategoryId,
                newsStatus = article.NewsStatus,
                createdDate = article.CreatedDate,
                createdById = article.CreatedById,
                tagsString = tagsString
            };

            return new JsonResult(result);
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage();
            }

            var tagNames = TagsString?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? Array.Empty<string>();
            var newTagsList = new List<NewsTag>();

            foreach (var tagName in tagNames)
            {
                var tag = _tagService.GetOrCreateTagByName(tagName);
                newTagsList.Add(new NewsTag { NewsArticleId = NewsArticle.NewsArticleId, TagId = tag.TagId });
            }

            NewsArticle.NewsTags = newTagsList;
            NewsArticle.ModifiedDate = DateTime.Now;

            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId != null)
            {
                NewsArticle.UpdatedById = (short)accountId;
            }

            _newsArticleService.UpdateNewsArticle(NewsArticle);

            await _hubContext.Clients.All.SendAsync("LoadNews");

            return RedirectToPage("./Index");
        }
    }
}