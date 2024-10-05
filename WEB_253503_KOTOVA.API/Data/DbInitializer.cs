using Microsoft.EntityFrameworkCore;
using WEB_253503_KOTOVA.API.Data;
using WEB_253503_KOTOVA.Domain.Entities;

namespace WEB_253503_KOTOVA.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Получаем контекст БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Выполняем миграции
            await context.Database.MigrateAsync();

            // Получаем URL приложения из конфигурации
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var apiUrl = configuration["AppSettings:ApiUrl"];

            // Проверяем, есть ли уже данные в базе
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Стартеры", NormalizedName = "starters" },
                    new Category { Name = "Салаты", NormalizedName = "salads" },
                    new Category { Name = "Супы", NormalizedName = "soups" },
                    new Category { Name = "Основные блюда", NormalizedName = "main-dishes" },
                    new Category { Name = "Напитки", NormalizedName = "drinks" },
                    new Category { Name = "Десерты", NormalizedName = "desserts" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            if (!context.Dishes.Any())
            {
                var dishes = new List<Dish>
                {
                    new Dish { Name = "Суп-харчо", Description = "Очень острый, невкусный", Calories = 200, Image = $"{apiUrl}/Images/harcho.webp", Category = context.Categories.First(c => c.NormalizedName == "soups") },
                    new Dish { Name = "Борщ", Description = "Много сала, без сметаны", Calories = 330, Image = $"{apiUrl}/Images/borsch.webp", Category = context.Categories.First(c => c.NormalizedName == "soups") },
                    new Dish { Name = "Окрошка", Description = "Летний холодный суп", Calories = 150, Image = $"{apiUrl}/Images/okroshka.webp", Category = context.Categories.First(c => c.NormalizedName == "soups") },
                    new Dish { Name = "Цезарь", Description = "Салат с курицей", Calories = 400, Image = $"{apiUrl}/Images/Chesar.webp", Category = context.Categories.First(c => c.NormalizedName == "salads") },
                    new Dish { Name = "Оливье", Description = "Традиционный салат с колбасой", Calories = 350, Image = $"{apiUrl}/Images/olivie.webp", Category = context.Categories.First(c => c.NormalizedName == "salads") },
                    new Dish { Name = "Компот", Description = "Сладкий напиток", Calories = 120, Image = $"{apiUrl}/Images/compot.webp", Category = context.Categories.First(c => c.NormalizedName == "drinks") },
                    new Dish { Name = "Чай", Description = "Чай черный", Calories = 1, Image = $"{apiUrl}/Images/tea.webp", Category = context.Categories.First(c => c.NormalizedName == "drinks") },
                    new Dish { Name = "Пирожное", Description = "Десерт со сливками", Calories = 500, Image = $"{apiUrl}/Images/choco_cake.webp", Category = context.Categories.First(c => c.NormalizedName == "desserts") }
                };

                context.Dishes.AddRange(dishes);
                await context.SaveChangesAsync();
            }
        }
    }
}
