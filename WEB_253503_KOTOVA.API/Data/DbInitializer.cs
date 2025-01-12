﻿using Microsoft.EntityFrameworkCore;
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
            //context.Dishes.RemoveRange(context.Dishes);
            //context.SaveChanges();
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
    // Супы
    new Dish { Name = "Суп-харчо", Description = "Очень острый, невкусный", Calories = 200, Image = $"{apiUrl}/Images/harcho.webp", Category = context.Categories.First(c => c.NormalizedName == "soups") },
    new Dish { Name = "Борщ", Description = "Много сала, без сметаны", Calories = 330, Image = $"{apiUrl}/Images/borsch.webp", Category = context.Categories.First(c => c.NormalizedName == "soups") },
    new Dish { Name = "Окрошка", Description = "Летний холодный суп", Calories = 150, Image = $"{apiUrl}/Images/okroshka.webp", Category = context.Categories.First(c => c.NormalizedName == "soups") },
    new Dish { Name = "Том Ям", Description = "Острый тайский суп с креветками", Calories = 250, Image = $"{apiUrl}/Images/tom_yam.webp", Category = context.Categories.First(c => c.NormalizedName == "soups") },
    new Dish { Name = "Щи", Description = "Традиционный русский суп из капусты", Calories = 180, Image = $"{apiUrl}/Images/shchi.webp", Category = context.Categories.First(c => c.NormalizedName == "soups") },

    // Салаты
    new Dish { Name = "Цезарь", Description = "Салат с курицей", Calories = 400, Image = $"{apiUrl}/Images/Chesar.webp", Category = context.Categories.First(c => c.NormalizedName == "salads") },
    new Dish { Name = "Оливье", Description = "Традиционный салат с колбасой", Calories = 350, Image = $"{apiUrl}/Images/olivie.webp", Category = context.Categories.First(c => c.NormalizedName == "salads") },
    new Dish { Name = "Винегрет", Description = "Овощной салат с маслом", Calories = 180, Image = $"{apiUrl}/Images/vinegret.webp", Category = context.Categories.First(c => c.NormalizedName == "salads") },
    new Dish { Name = "Греческий салат", Description = "Салат с овощами и сыром фета", Calories = 250, Image = $"{apiUrl}/Images/greek_salad.webp", Category = context.Categories.First(c => c.NormalizedName == "salads") },
    new Dish { Name = "Капрезе", Description = "Салат с помидорами и моцареллой", Calories = 320, Image = $"{apiUrl}/Images/caprese.webp", Category = context.Categories.First(c => c.NormalizedName == "salads") },

    // Напитки
    new Dish { Name = "Компот", Description = "Сладкий напиток", Calories = 120, Image = $"{apiUrl}/Images/compot.webp", Category = context.Categories.First(c => c.NormalizedName == "drinks") },
    new Dish { Name = "Чай", Description = "Чай черный", Calories = 1, Image = $"{apiUrl}/Images/tea.webp", Category = context.Categories.First(c => c.NormalizedName == "drinks") },
    new Dish { Name = "Морс", Description = "Напиток из ягод", Calories = 90, Image = $"{apiUrl}/Images/mors.webp", Category = context.Categories.First(c => c.NormalizedName == "drinks") },
    new Dish { Name = "Кофе", Description = "Кофе черный без сахара", Calories = 5, Image = $"{apiUrl}/Images/coffee.webp", Category = context.Categories.First(c => c.NormalizedName == "drinks") },
    new Dish { Name = "Лимонад", Description = "Лимонад с лимоном и мятой", Calories = 150, Image = $"{apiUrl}/Images/lemonade.webp", Category = context.Categories.First(c => c.NormalizedName == "drinks") },

    // Основные блюда
    new Dish { Name = "Картофель по-деревенски", Description = "Картофель, жаренный с травами", Calories = 500, Image = $"{apiUrl}/Images/fried_potatoes.webp", Category = context.Categories.First(c => c.NormalizedName == "main-dishes") },
    new Dish { Name = "Стейк из говядины", Description = "Мясной стейк средней прожарки", Calories = 700, Image = $"{apiUrl}/Images/steak.webp", Category = context.Categories.First(c => c.NormalizedName == "main-dishes") },
    new Dish { Name = "Рыба на гриле", Description = "Рыба, приготовленная на гриле", Calories = 350, Image = $"{apiUrl}/Images/grilled_fish.webp", Category = context.Categories.First(c => c.NormalizedName == "main-dishes") },
    new Dish { Name = "Паста карбонара", Description = "Итальянская паста с соусом карбонара", Calories = 600, Image = $"{apiUrl}/Images/carbonara.webp", Category = context.Categories.First(c => c.NormalizedName == "main-dishes") },
    new Dish { Name = "Ризотто", Description = "Итальянское блюдо из риса", Calories = 480, Image = $"{apiUrl}/Images/risotto.webp", Category = context.Categories.First(c => c.NormalizedName == "main-dishes") },

    // Десерты
    new Dish { Name = "Пирожное", Description = "Десерт со сливками", Calories = 500, Image = $"{apiUrl}/Images/choco_cake.webp", Category = context.Categories.First(c => c.NormalizedName == "desserts") },
    new Dish { Name = "Тирамису", Description = "Итальянский десерт", Calories = 450, Image = $"{apiUrl}/Images/tiramisu.webp", Category = context.Categories.First(c => c.NormalizedName == "desserts") },
    new Dish { Name = "Мороженое", Description = "Ванильное мороженое", Calories = 250, Image = $"{apiUrl}/Images/ice_cream.webp", Category = context.Categories.First(c => c.NormalizedName == "desserts") },
    new Dish { Name = "Эклеры", Description = "Заварные пирожные с кремом", Calories = 400, Image = $"{apiUrl}/Images/eclairs.webp", Category = context.Categories.First(c => c.NormalizedName == "desserts") },
    new Dish { Name = "Брауни", Description = "Шоколадный десерт", Calories = 350, Image = $"{apiUrl}/Images/brownie.webp", Category = context.Categories.First(c => c.NormalizedName == "desserts") },

    // Стартеры
    new Dish { Name = "Брускетта", Description = "Закуска с томатами и базиликом", Calories = 200, Image = $"{apiUrl}/Images/bruschetta.webp", Category = context.Categories.First(c => c.NormalizedName == "starters") },
    new Dish { Name = "Фаршированные яйца", Description = "Яйца с начинкой из тунца", Calories = 150, Image = $"{apiUrl}/Images/deviled_eggs.webp", Category = context.Categories.First(c => c.NormalizedName == "starters") },
    new Dish { Name = "Карпаччо", Description = "Тонко нарезанное сырое мясо", Calories = 300, Image = $"{apiUrl}/Images/carpaccio.webp", Category = context.Categories.First(c => c.NormalizedName == "starters") },
    new Dish { Name = "Креветки на гриле", Description = "Креветки, приготовленные на гриле", Calories = 180, Image = $"{apiUrl}/Images/grilled_shrimp.webp", Category = context.Categories.First(c => c.NormalizedName == "starters") },
    new Dish { Name = "Хумус с хлебцами", Description = "Традиционная ближневосточная закуска", Calories = 220, Image = $"{apiUrl}/Images/hummus.webp", Category = context.Categories.First(c => c.NormalizedName == "starters") }
};


                context.Dishes.AddRange(dishes);
                await context.SaveChangesAsync();
            }
        }
    }
}
