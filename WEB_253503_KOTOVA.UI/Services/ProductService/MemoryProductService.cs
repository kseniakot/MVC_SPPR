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
                Calories = 200, Image = "images/harcho.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
            },
            new Dish { Id = 2, Name = "Борщ",
                Description = "Много сала, без сметаны",
                Calories = 330, Image = "images/borsch.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
            },
            new Dish { Id = 3, Name = "Цезарь",
                Description = "Салат с курицей",
                Calories = 400, Image = "images/Chesar.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("salads"))
            },
            new Dish { Id = 4, Name = "Компот",
                Description = "Сладкий напиток",
                Calories = 120, Image = "images/compot.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("drinks"))
            },
            new Dish { Id = 5, Name = "Пирожное",
                Description = "Десерт со сливками",
                Calories = 500, Image = "images/choco_cake.jpg",
                Category = _categories.Find(c => c.NormalizedName.Equals("desserts"))
            }
        };
        }

        /// <summary>
        /// Получение списка продуктов с фильтрацией по категории и разбиением на страницы
        /// </summary>
        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var filteredDishes = _dishes.Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName)).ToList();
            var result = new ListModel<Dish>
            {
                Items = filteredDishes,
                CurrentPage = pageNo,
                TotalPages = 1 // В этой реализации пагинация не учитывается, но можно добавить
            };
            return Task.FromResult(ResponseData<ListModel<Dish>>.Success(result));
        }
    }

}
