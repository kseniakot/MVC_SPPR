using Microsoft.AspNetCore.Mvc;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.UI.Services.CategoryService;
using WEB_253503_KOTOVA.UI.Services.ProductService;

namespace WEB_253503_KOTOVA.Controllers
{
    [Route("menu/{category?}/{pageNo?}")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

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

            return View(productResponse.Data);
        }

    }
}
