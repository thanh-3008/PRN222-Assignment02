using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Interfaces;
using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using NguyenTrungThanhRazorPages.Hubs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NguyenTrungThanhRazorPages.Pages.Staff.NewsManagement
{
    public class CreateModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService; 
        private readonly IHubContext<NewsHub> _hubContext;

        public CreateModel(INewsArticleService newsArticleService, ICategoryService categoryService, ITagService tagService, IHubContext<NewsHub> hubContext)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _tagService = tagService; 
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = new();

        [BindProperty]
        public string TagsString { get; set; }

        public SelectList CategoryList { get; set; }

        public IActionResult OnGet()
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "1")
            {
                return RedirectToPage("/Account/Login");
            }

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

            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (accountId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            NewsArticle.NewsArticleId = Guid.NewGuid().ToString();
            NewsArticle.CreatedById = (short)accountId;
            NewsArticle.CreatedDate = DateTime.Now;
            NewsArticle.NewsStatus = true;

            var tagNames = TagsString?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? Array.Empty<string>();
            var newTagsList = new List<NewsTag>();
            foreach (var tagName in tagNames)
            {
                var tag = _tagService.GetOrCreateTagByName(tagName);
                newTagsList.Add(new NewsTag { NewsArticleId = NewsArticle.NewsArticleId, TagId = tag.TagId });
            }
            NewsArticle.NewsTags = newTagsList;

            _newsArticleService.SaveNewsArticle(NewsArticle);
            await _hubContext.Clients.All.SendAsync("LoadNews");
            return RedirectToPage("./Index");
        }
    }
}