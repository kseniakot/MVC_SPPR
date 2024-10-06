using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace WEB_253503_KOTOVA.API.Services.CategoryServices
{
    

   
        public class CategoryService : ICategoryService
        {
            private readonly AppDbContext _context;

            public CategoryService(AppDbContext context)
            {
                _context = context;
            }

            public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
            {
                var categories = await _context.Categories.ToListAsync();
                return ResponseData<List<Category>>.Success(categories);
            }
        }
    

}
