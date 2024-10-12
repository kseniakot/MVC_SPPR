using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.UI.Services.ProductService;

namespace WEB_253503_KOTOVA.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IList<Dish> Dish { get; set; } = new List<Dish>();

        public async Task OnGetAsync()
        {
            var response = await _productService.GetProductListAsync(null, 1);
            if (response.Successfull)
            {
                Dish = response.Data.Items;
            }
            else
            {
                Dish = new List<Dish>();
            }
        }
    }
}
