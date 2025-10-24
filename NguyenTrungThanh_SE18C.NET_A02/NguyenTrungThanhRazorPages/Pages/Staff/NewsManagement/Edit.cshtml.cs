using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using NguyenTrungThanhRazorPages.Hubs;
using System.Threading.Tasks;

namespace NguyenTrungThanhRazorPages.Pages.Staff.NewsManagement
{
    public class EditModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IHubContext<NewsHub> _hubContext;

        public EditModel(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService, IHubContext<NewsHub> hubContext)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        [BindProperty]
        public string TagsString { get; set; }

        public SelectList CategoryList { get; set; }

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
            TagsString = string.Join(",", article.NewsTags.Select(t => t.Tag.TagName));
            CategoryList = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "1")
            {
                return RedirectToPage("/Account/Login");
            }

            if (!ModelState.IsValid)
            {
                CategoryList = new SelectList(_categoryService.GetCategories(), "CategoryId", "CategoryName");
                return Page();
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