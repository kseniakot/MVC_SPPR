using Microsoft.AspNetCore.Mvc;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.UI.Services.CategoryService;
using WEB_253503_KOTOVA.UI.Services.ProductService;
using System.Threading.Tasks;
using WEB_253503_KOTOVA.Domain.Models;

namespace WEB_253503_KOTOVA.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category)
        {
            // Получаем список категорий 
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (!categoriesResponse.Successfull)
                return NotFound(categoriesResponse.ErrorMessage);

            var categories = categoriesResponse.Data;

            // Получаем продукты, отфильтрованные по категории 
            var productResponse = await _productService.GetProductListAsync(category);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);

            var products = productResponse.Data.Items;

            // Создаем ListModel с полученными данными 
            var listModel = new ListModel<Dish>
            {
                Items = products,
                CurrentPage = 1, // Установите номер текущей страницы, если требуется 
                TotalPages = 1   // Поставьте количество страниц, если требуется 
            };

            // Передаем текущую категорию и список категорий в представление 
            var currentCategory = categories.FirstOrDefault(c => c.NormalizedName == category);
            ViewData["currentCategory"] = currentCategory != null ? currentCategory.Name : "Все"; // Если null, то устанавливаем "Все" 
            ViewBag.Categories = categories;

            return View(listModel); // Возвращаем модель правильного типа 
        }

    }
}
