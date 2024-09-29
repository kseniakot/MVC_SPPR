using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;
using WEB_253503_KOTOVA.UI.Services.CategoryService;

namespace WEB_253503_KOTOVA.UI.Services.ProductService
{
    public class MemoryProductService : IProductService
    {
        private List<Dish> _dishes;
        private List<Category> _categories;
        private readonly int _itemsPerPage;

        public MemoryProductService(ICategoryService categoryService, IConfiguration config)
        {
            // Получаем категории через categoryService
            _categories = categoryService.GetCategoryListAsync().Result.Data;

            // Получаем количество элементов на странице из конфигурации
            _itemsPerPage = int.Parse(config["ItemsPerPage"] ?? "3");

            // Заполняем коллекции данными
            SetupData();
        }

       


        /// <summary>
        /// Инициализация списков с тестовыми данными
        /// </summary>
        private void SetupData()
        {
            _dishes = new List<Dish>
        {
            new Dish { Id = 1, Name = "Суп-харчо",
                Description = "Очень острый, невкусный",
                Calories = 200, Image = "Images/Суп.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
            },
            new Dish { Id = 2, Name = "Борщ",
                Description = "Много сала, без сметаны",
                Calories = 330, Image = "Images/Борщ.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
            },
            new Dish { Id = 3, Name = "Цезарь",
                Description = "Салат с курицей",
                Calories = 400, Image = "Images/Цезарь.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("salads"))
            },
            new Dish { Id = 4, Name = "Компот",
                Description = "Сладкий напиток",
                Calories = 120, Image = "Images/Компот.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("drinks"))
            },
            new Dish { Id = 5, Name = "Пирожное",
                Description = "Десерт со сливками",
                Calories = 500, Image = "Images/Пирожное.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("desserts"))
            }
        };
        }

        /// <summary>
        /// Получение списка продуктов с фильтрацией по категории и разбиением на страницы
        /// </summary>
        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // Фильтруем список по категории
            var filteredDishes = _dishes
                .Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName))
                .ToList();

            // Вычисляем общее количество страниц
            var totalPages = (int)Math.Ceiling(filteredDishes.Count / (double)_itemsPerPage);

            // Получаем данные для нужной страницы
            var dishesOnPage = filteredDishes
                .Skip((pageNo - 1) * _itemsPerPage)
                .Take(_itemsPerPage)
                .ToList();

            // Формируем результат
            var result = new ListModel<Dish>
            {
                Items = dishesOnPage,
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            return Task.FromResult(ResponseData<ListModel<Dish>>.Success(result));
        }
    }

}
