using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_253503_KOTOVA.API.Services.FileServices;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.UI.Services.CategoryService;
using WEB_253503_KOTOVA.UI.Services.ProductService;

namespace WEB_253503_KOTOVA.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;

        public EditModel(IProductService productService, ICategoryService categoryService, IFileService fileService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _fileService = fileService;
        }

        [BindProperty]
        public Dish Dish { get; set; } = new Dish();

        [BindProperty]
        public IFormFile? Upload { get; set; } // Для загрузки изображения

        public SelectList Categories { get; set; } // Для категорий

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _productService.GetProductByIdAsync(id.Value);
            if (!response.Successfull || response.Data == null)
            {
                return NotFound();
            }

            Dish = response.Data;

            // Получаем категории для выбора
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

            if (Upload != null)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/jpg" };
                if (!allowedTypes.Contains(Upload.ContentType))
                {
                    Console.WriteLine(Upload.ContentType);
                    ModelState.AddModelError("Upload", "Неподдерживаемый тип файла. Пожалуйста, загрузите изображение.");
                    await LoadCategoriesAsync(); // Загружаем категории при ошибках
                    return Page();
                }

                var imageUrl = await _fileService.SaveFileAsync(Upload);
                if (string.IsNullOrEmpty(imageUrl))
                {
                    ModelState.AddModelError("", "Не удалось загрузить изображение.");
                    await LoadCategoriesAsync(); // Загружаем категории при ошибках
                    return Page();
                }

                Dish.Image = imageUrl; // Обновляем путь к изображению
            }

            var response = await _productService.UpdateProductAsync(Dish.Id, Dish, Upload);
            if (!response.Successfull)
            {
                ModelState.AddModelError("", "Не удалось обновить блюдо: " + response.ErrorMessage);
                await LoadCategoriesAsync(); // Загружаем категории при ошибках
                return Page();
            }

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
