using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_253503_KOTOVA.Domain.Models;
using WEB_253503_KOTOVA.UI.Services.ProductService;

namespace WEB_253503_KOTOVA.UI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private Cart _cart;
        public CartController(IProductService productService, Cart cart)
        {
            _productService = productService;
            _cart = cart;
        }
        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _productService.GetProductByIdAsync(id);
            if (data.Successfull)
            {
                _cart.AddToCart(data.Data);
            }
            return Redirect(returnUrl);
        }
        [Route("[controller]/delete/{id:int}")]
        public ActionResult Delete(int id)
        {
            _cart.RemoveItems(id);
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            return View(_cart);
        }
    }
}
