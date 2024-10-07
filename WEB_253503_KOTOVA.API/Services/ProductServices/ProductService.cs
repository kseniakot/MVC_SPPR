
using WEB_253503_KOTOVA.Domain.Models;
using Microsoft.EntityFrameworkCore;
using WEB_253503_KOTOVA.Domain.Entities;


namespace WEB_253503_KOTOVA.API.Services.ProductServices
{
   
   
        public class ProductService : IProductService
        {
            private readonly AppDbContext _context;
            private readonly int _maxPageSize = 20;

            public ProductService(AppDbContext context)
            {
                _context = context;
            }



        public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Dishes.AsQueryable();
            var dataList = new ListModel<Dish>();

            // Проверка категории, корректируем кавычки
            if (!string.IsNullOrEmpty(categoryNormalizedName) && categoryNormalizedName != "Все")
            {
                query = query.Where(d => d.Category.NormalizedName.Equals(categoryNormalizedName));
            }

            // Количество элементов в списке
            var count = await query.CountAsync();
            if (count == 0)
            {
                return ResponseData<ListModel<Dish>>.Success(dataList);
            }

            // Количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
            {
                return ResponseData<ListModel<Dish>>.Error("No such page");
            }

            // Получение элементов текущей страницы
            dataList.Items = await query
                .OrderBy(d => d.Id)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;

            return ResponseData<ListModel<Dish>>.Success(dataList);
        }

        public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
            {
                var dish = await _context.Dishes
                    .Include(d => d.Category)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (dish == null)
                {
                    return ResponseData<Dish>.Error("Product not found");
                }

                return ResponseData<Dish>.Success(dish);
            }

            public async Task UpdateProductAsync(int id, Dish product)
            {
                if (id != product.Id)
                {
                    throw new ArgumentException("Id mismatch");
                }

                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            public async Task DeleteProductAsync(int id)
            {
                var dish = await _context.Dishes.FindAsync(id);
                if (dish != null)
                {
                    _context.Dishes.Remove(dish);
                    await _context.SaveChangesAsync();
                }
            }

            public async Task<ResponseData<Dish>> CreateProductAsync(Dish product)
            {
                _context.Dishes.Add(product);
                await _context.SaveChangesAsync();
                return ResponseData<Dish>.Success(product);
            }

            public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
            {
                // Здесь можно добавить логику для сохранения изображений
                var dish = await _context.Dishes.FindAsync(id);
                if (dish == null)
                {
                    return ResponseData<string>.Error("Product not found");
                }

                // Логика для сохранения изображения и получения его URL
                var imageUrl = $"/Images/{formFile.FileName}";
                dish.Image = imageUrl;
                await _context.SaveChangesAsync();

                return ResponseData<string>.Success(imageUrl);
            }
        }
    

}
