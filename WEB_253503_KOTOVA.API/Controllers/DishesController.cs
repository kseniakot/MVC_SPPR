using Microsoft.AspNetCore.Mvc;
using WEB_253503_KOTOVA.API.Services.ProductServices;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;

namespace WEB_253503_KOTOVA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IProductService _productService;

        public DishesController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Dishes/{category}?pageNo=1&pageSize=3
        [HttpGet("{category?}")]
        public async Task<ActionResult<ResponseData<List<Dish>>>> GetDishes(
            string? category,
            int pageNo = 1,
            int pageSize = 3)
        {
            var result = await _productService.GetProductListAsync(category, pageNo, pageSize);
            return Ok(result);
        }

        // GET: api/Dishes/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseData<Dish>>> GetDish(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.Successfull)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        // PUT: api/Dishes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            if (id != dish.Id)
            {
                return BadRequest("Id mismatch");
            }

            await _productService.UpdateProductAsync(id, dish);
            return NoContent();
        }

        // POST: api/Dishes
        [HttpPost]
        public async Task<ActionResult<ResponseData<Dish>>> PostDish(Dish dish)
        {
            var result = await _productService.CreateProductAsync(dish);
            if (!result.Successfull)
            {
                return BadRequest(result.ErrorMessage);
            }
            return CreatedAtAction(nameof(GetDish), new { id = dish.Id }, result.Data);
        }

        // DELETE: api/Dishes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }

}
