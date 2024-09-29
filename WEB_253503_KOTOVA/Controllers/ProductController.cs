using Microsoft.AspNetCore.Mvc;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.UI.Services.CategoryService;
using WEB_253503_KOTOVA.UI.Services.ProductService;

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

        public async Task<IActionResult> Index(string category)
        {
            var productResponse =
            await _productService.GetProductListAsync(category);
            if (!productResponse.Successfull)
                return NotFound(productResponse.ErrorMessage);
            return View(productResponse.Data.Items);
        }
    }
}
