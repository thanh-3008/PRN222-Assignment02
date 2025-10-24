using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace NguyenTrungThanhRazorPages.Pages.Staff.CategoryManagement
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public DeleteModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        [TempData]
        public string ErrorMessage { get; set; }

        public IActionResult OnGet(short id)
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "1")
            {
                return RedirectToPage("/Account/Login");
            }

            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
            return Page();
        }

        public IActionResult OnPost(short id)
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "1")
            {
                return RedirectToPage("/Account/Login");
            }

            if (_categoryService.IsCategoryInUse(id))
            {
                ErrorMessage = "Cannot delete this category because it is currently in use by one or more news articles.";
                return RedirectToPage("./Index");
            }

            var categoryToDelete = _categoryService.GetCategoryById(id);
            if (categoryToDelete != null)
            {
                _categoryService.DeleteCategory(categoryToDelete);
            }

            return RedirectToPage("./Index");
        }
    }
}