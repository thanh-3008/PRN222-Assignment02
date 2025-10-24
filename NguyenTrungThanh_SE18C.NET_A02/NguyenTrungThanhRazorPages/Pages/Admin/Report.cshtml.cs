using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace NguyenTrungThanhRazorPages.Pages.Admin
{
    public class ReportModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public ReportModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [BindProperty]
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-30);

        [BindProperty]
        public DateTime EndDate { get; set; } = DateTime.Now;

        public IList<NewsArticle> ReportData { get; set; }

        public IActionResult OnGet()
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "Admin")
            {
                return RedirectToPage("/Account/Login");
            }

            ReportData = _newsArticleService.GetNewsArticlesByPeriod(StartDate, EndDate);
            return Page();
        }

        public IActionResult OnPost()
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "Admin")
            {
                return RedirectToPage("/Account/Login");
            }

            if (StartDate > EndDate)
            {
                ModelState.AddModelError(string.Empty, "Start Date cannot be after End Date.");
                ReportData = _newsArticleService.GetNewsArticlesByPeriod(StartDate, EndDate);
                return Page();
            }

            ReportData = _newsArticleService.GetNewsArticlesByPeriod(StartDate, EndDate);
            return Page();
        }
    }
}