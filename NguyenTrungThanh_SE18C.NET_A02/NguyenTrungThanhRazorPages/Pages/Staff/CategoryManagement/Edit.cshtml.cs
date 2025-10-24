using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace NguyenTrungThanhRazorPages.Pages.Staff.CategoryManagement
{
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public EditModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

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

        public IActionResult OnPost()
        {
            var userRole = HttpContext.Session.GetString("Role");
            if (userRole != "1")
            {
                return RedirectToPage("/Account/Login");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _categoryService.UpdateCategory(Category);
            return RedirectToPage("./Index");
        }
    }
}