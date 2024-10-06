using Microsoft.AspNetCore.Mvc;
using WEB_253503_KOTOVA.API.Services.CategoryServices;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;

namespace WEB_253503_KOTOVA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<Category>>>> GetCategories()
        {
            var result = await _categoryService.GetCategoryListAsync();
            if (!result.Successfull)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var result = await _categoryService.GetCategoryListAsync();
            var category = result.Data.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest("Id mismatch");
            }

            // Здесь стоит добавить метод для обновления категории, если требуется
            // Однако, в данном примере CategoryService не реализует обновление

            return NoContent();
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            // CategoryService в текущем виде не содержит методов для создания новых категорий
            // Необходимо будет добавить эту логику в сервисе

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            // Логика удаления категории также не реализована в CategoryService, но
            // можно реализовать метод удаления в соответствии с требованиями

            return NoContent();
        }
    }
}
