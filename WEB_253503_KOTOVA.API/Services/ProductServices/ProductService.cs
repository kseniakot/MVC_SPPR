
using WEB_253503_KOTOVA.Domain.Models;
using Microsoft.EntityFrameworkCore;
using WEB_253503_KOTOVA.Domain.Entities;
using System.Drawing.Printing;


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

            // Включаем загрузку связанных данных с помощью Include
            var query = _context.Dishes
                                .Include(d => d.Category) // Загружаем связанную категорию
                                .AsQueryable();

            var dataList = new ListModel<Dish>();

            // Фильтруем по категории, если указано
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

        public async Task<ResponseData<object>> DeleteProductAsync(int id)
        {
            var product = await _context.Dishes.FindAsync(id);
            if (product == null)
            {
                return ResponseData<object>.Error("Product not found");
            }

            _context.Dishes.Remove(product);
            await _context.SaveChangesAsync();

            return ResponseData<object>.Success(null);  
        }

        public async Task<ResponseData<Dish>> CreateProductAsync(Dish product)
            {
            _context.Categories.Attach(product.Category);
            Console.WriteLine($"Dish Id: {product.Id}");
            Console.WriteLine($"Dish Name: {product.Name}");
            Console.WriteLine($"Dish Description: {product.Description}");
            Console.WriteLine($"Dish Calories: {product.Calories}");
            Console.WriteLine($"Dish CategoryId: {product.CategoryId}");
            Console.WriteLine($"Dish Category: {product.Category?.Name}"); // Use null-conditional operator in case Category is null
            Console.WriteLine($"Dish Price: {product.Price}");
            Console.WriteLine($"Dish Image: {product.Image}");
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
