using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;
using WEB_253503_KOTOVA.UI.Services.CategoryService;

namespace WEB_253503_KOTOVA.UI.Services.ProductService
{
    public class MemoryProductService : IProductService
    {


        private int _itemsPerPage;
        private List<Dish> _dishes;
        private List<Category> _categories;

        public MemoryProductService(IConfiguration config, ICategoryService categoryService)
        {
            _itemsPerPage = int.Parse(config["ItemsPerPage"] ?? "3"); // Получаем количество элементов на страницу из конфигурации
            _categories = categoryService.GetCategoryListAsync().Result.Data;
            SetupData();
        }




        /// <summary>
        /// Инициализация списков с тестовыми данными
        /// </summary>
        private void SetupData()
        {
            _dishes = new List<Dish>
{
    // Супы
    new Dish { Id = 1, Name = "Суп-харчо",
        Description = "Очень острый, невкусный",
        Calories = 200, Image = "Images/harcho.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
    },
    new Dish { Id = 2, Name = "Борщ",
        Description = "Много сала, без сметаны",
        Calories = 330, Image = "Images/borsch.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
    },
    new Dish { Id = 3, Name = "Окрошка",
        Description = "Летний холодный суп",
        Calories = 150, Image = "Images/okroshka.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
    },
    new Dish { Id = 4, Name = "Том Ям",
        Description = "Острый тайский суп с креветками",
        Calories = 250, Image = "Images/tom_yam.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
    },
    new Dish { Id = 5, Name = "Щи",
        Description = "Традиционный русский суп из капусты",
        Calories = 180, Image = "Images/shchi.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("soups"))
    },

    // Салаты
    new Dish { Id = 6, Name = "Цезарь",
        Description = "Салат с курицей",
        Calories = 400, Image = "Images/Chesar.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("salads"))
    },
    new Dish { Id = 7, Name = "Оливье",
        Description = "Традиционный салат с колбасой",
        Calories = 350, Image = "Images/olivie.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("salads"))
    },
    new Dish { Id = 8, Name = "Винегрет",
        Description = "Овощной салат с маслом",
        Calories = 180, Image = "Images/vinegret.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("salads"))
    },
    new Dish { Id = 9, Name = "Греческий салат",
        Description = "Салат с овощами и сыром фета",
        Calories = 250, Image = "Images/greek_salad.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("salads"))
    },
    new Dish { Id = 10, Name = "Капрезе",
        Description = "Салат с помидорами и моцареллой",
        Calories = 320, Image = "Images/caprese.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("salads"))
    },

    // Напитки
    new Dish { Id = 11, Name = "Компот",
        Description = "Сладкий напиток",
        Calories = 120, Image = "Images/compot.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("drinks"))
    },
    new Dish { Id = 12, Name = "Чай",
        Description = "Чай черный",
        Calories = 1, Image = "Images/tea.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("drinks"))
    },
    new Dish { Id = 13, Name = "Морс",
        Description = "Напиток из ягод",
        Calories = 90, Image = "Images/mors.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("drinks"))
    },
    new Dish { Id = 14, Name = "Кофе",
        Description = "Кофе черный без сахара",
        Calories = 5, Image = "Images/coffee.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("drinks"))
    },
    new Dish { Id = 15, Name = "Лимонад",
        Description = "Лимонад с лимоном и мятой",
        Calories = 150, Image = "Images/lemonade.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("drinks"))
    },

    // Основные блюда
    new Dish { Id = 16, Name = "Картофель по-деревенски",
        Description = "Картофель, жаренный с травами",
        Calories = 500, Image = "Images/fried_potatoes.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("main-dishes"))
    },
    new Dish { Id = 17, Name = "Стейк из говядины",
        Description = "Мясной стейк средней прожарки",
        Calories = 700, Image = "Images/steak.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("main-dishes"))
    },
    new Dish { Id = 18, Name = "Рыба на гриле",
        Description = "Рыба, приготовленная на гриле",
        Calories = 350, Image = "Images/grilled_fish.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("main-dishes"))
    },
    new Dish { Id = 19, Name = "Паста карбонара",
        Description = "Итальянская паста с соусом карбонара",
        Calories = 600, Image = "Images/carbonara.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("main-dishes"))
    },
    new Dish { Id = 20, Name = "Ризотто",
        Description = "Итальянское блюдо из риса",
        Calories = 480, Image = "Images/risotto.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("main-dishes"))
    },

    // Десерты
    new Dish { Id = 21, Name = "Пирожное",
        Description = "Десерт со сливками",
        Calories = 500, Image = "Images/choco_cake.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("desserts"))
    },
    new Dish { Id = 22, Name = "Тирамису",
        Description = "Итальянский десерт",
        Calories = 450, Image = "Images/tiramisu.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("desserts"))
    },
    new Dish { Id = 23, Name = "Мороженое",
        Description = "Ванильное мороженое",
        Calories = 250, Image = "Images/ice_cream.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("desserts"))
    },
    new Dish { Id = 24, Name = "Эклеры",
        Description = "Заварные пирожные с кремом",
        Calories = 400, Image = "Images/eclairs.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("desserts"))
    },
    new Dish { Id = 25, Name = "Брауни",
        Description = "Шоколадный десерт",
        Calories = 350, Image = "Images/brownie.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("desserts"))
    },

    // Стартеры
    new Dish { Id = 26, Name = "Брускетта",
        Description = "Закуска с томатами и базиликом",
        Calories = 200, Image = "Images/bruschetta.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("starters"))
    },
    new Dish { Id = 27, Name = "Фаршированные яйца",
        Description = "Яйца с начинкой из тунца",
        Calories = 150, Image = "Images/deviled_eggs.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("starters"))
    },
    new Dish { Id = 28, Name = "Карпаччо",
        Description = "Тонко нарезанное сырое мясо",
        Calories = 300, Image = "Images/carpaccio.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("starters"))
    },
    new Dish { Id = 29, Name = "Креветки на гриле",
        Description = "Креветки, приготовленные на гриле",
        Calories = 180, Image = "Images/grilled_shrimp.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("starters"))
    },
    new Dish { Id = 30, Name = "Хумус с хлебцами",
        Description = "Традиционная ближневосточная закуска",
        Calories = 220, Image = "Images/hummus.webp",
        Category = _categories.Find(c => c.NormalizedName.Equals("starters"))
    }
};

        }

        /// <summary>
        /// Получение списка продуктов с фильтрацией по категории и разбиением на страницы
        /// </summary>
        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            // Проверяем, если categoryNormalizedName == null или пустая строка, то возвращаем все блюда 
            var filteredDishes = _dishes
                .Where(d => string.IsNullOrEmpty(categoryNormalizedName) || categoryNormalizedName == "Все" || d.Category.NormalizedName.Equals(categoryNormalizedName))
                .ToList();

            // Вычисляем общее количество страниц 
            var totalPages = (int)Math.Ceiling((double)filteredDishes.Count / _itemsPerPage);

            // Убедитесь, что pageNo находится в допустимых пределах 
            if (pageNo < 1 || pageNo > totalPages)
            {
                pageNo = 1; // Сбрасываем на первую страницу, если номер вне пределов 
            }

            // Получаем нужную страницу 
            var paginatedDishes = filteredDishes
                .Skip((pageNo - 1) * _itemsPerPage) // Пропускаем элементы до нужной страницы 
                .Take(_itemsPerPage) // Берем только количество элементов на страницу 
                .ToList();

            var result = new ListModel<Dish>
            {
                Items = paginatedDishes,
                CurrentPage = pageNo,
                TotalPages = totalPages // Устанавливаем общее количество страниц 
            };

            return Task.FromResult(ResponseData<ListModel<Dish>>.Success(result));
        }

        public Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            var dish = _dishes.FirstOrDefault(d => d.Id == id);

            if (dish == null)
            {
                return Task.FromResult(ResponseData<Dish>.Error("Блюдо не найдено."));
            }

            return Task.FromResult(ResponseData<Dish>.Success(dish));
        }

    }

}
