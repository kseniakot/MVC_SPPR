using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_KOTOVA.UI.Services.CategoryService;
using WEB_253503_KOTOVA.UI.Services.ProductService;
using WEB_253503_KOTOVA.Domain.Entities;

namespace WEB_253503_KOTOVA.UI.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Dish Dish { get; set; } = new Dish();

        [BindProperty]
        public IFormFile? Upload { get; set; } 

        public SelectList Categories { get; set; } 

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadCategoriesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Dish.Category");
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();  
                return Page();
            }

            
            var categoryResponse = await _categoryService.GetCategoryListAsync();
            var selectedCategory = categoryResponse.Data.FirstOrDefault(c => c.Id == Dish.CategoryId);

            if (selectedCategory != null)
            {
                Dish.Category = selectedCategory;  
            }
            else
            {
                Console.WriteLine(Dish.CategoryId);
                Console.WriteLine("Выбранная категория не существует.");
                ModelState.AddModelError("Category", "Выбранная категория не существует.");
                await LoadCategoriesAsync();
                return Page();
            }
            Console.WriteLine($"Dish Id: {Dish.Id}");
            Console.WriteLine($"Dish Name: {Dish.Name}");
            Console.WriteLine($"Dish Description: {Dish.Description}");
            Console.WriteLine($"Dish Calories: {Dish.Calories}");
            Console.WriteLine($"Dish CategoryId: {Dish.CategoryId}");
            Console.WriteLine($"Dish Category: {Dish.Category?.Name}"); // Use null-conditional operator in case Category is null
            Console.WriteLine($"Dish Price: {Dish.Price}");
            Console.WriteLine($"Dish Image: {Dish.Image}");
            Console.WriteLine($"Dish MimeType: {Dish.MimeType}");

            // Логика создания блюда
            await _productService.CreateProductAsync(Dish, Upload);
            return RedirectToPage("./Index");
        }

        private async Task LoadCategoriesAsync()
        {
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (categoriesResponse.Successfull)
            {
                Categories = new SelectList(categoriesResponse.Data, "Id", "Name");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Не удалось загрузить категории.");
            }
        }
    }
}
