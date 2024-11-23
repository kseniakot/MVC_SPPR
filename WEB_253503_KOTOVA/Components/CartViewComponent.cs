using Microsoft.AspNetCore.Mvc;
using WEB_253503_KOTOVA.Domain.Models;


namespace WEB_253503_KOTOVA.Components
{


    public class CartViewComponent : ViewComponent
    {
        private readonly Cart _cart;
        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}

