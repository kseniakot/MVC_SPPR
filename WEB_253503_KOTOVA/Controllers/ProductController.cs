using Microsoft.AspNetCore.Mvc;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.UI.Services.CategoryService;
using WEB_253503_KOTOVA.UI.Services.ProductService;
using WEB_253503_KOTOVA.UI.Extensions;

namespace WEB_253503_KOTOVA.Controllers
{
    [Route("Catalog")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [Route("")] // Маршрут для localhost:хххх/Catalog
        [Route("{category?}")] // Маршрут для localhost:хххх/Catalog/имя_категории

        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            // Получаем список категорий
            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            if (!categoriesResponse.Successfull)
                return NotFound(categoriesResponse.ErrorMessage);

            // Получаем продукты
            var productResponse = await _productService.GetProductListAsync(category, pageNo);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);


            var currentCategory = categoriesResponse.Data.FirstOrDefault(c => c.NormalizedName == category);

            ViewData["currentCategoryName"] = currentCategory != null ? currentCategory.Name : "Все";
            ViewData["currentCategory"] = category != null ? category: "Все";
         
            ViewBag.Categories = categoriesResponse.Data;

            // Проверка на AJAX-запрос
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Shared/Components/Dish/_DishListPartial.cshtml", productResponse.Data);
            }

            return View(productResponse.Data);
        }

    }
}
