﻿using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;

namespace WEB_253503_KOTOVA.API.Services.ProductServices
{
    public interface IProductService
    {
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="categoryNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <param name="pageSize">количество объектов на странице</param>
        /// <returns></returns>
        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);

        /// <summary>
        /// Поиск объекта по Id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns></returns>
        public Task<ResponseData<Dish>> GetProductByIdAsync(int id);

        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="id">Id изменяемого объекта</param>
        /// <param name="product">Объект с новыми параметрами</param>
        /// <returns></returns>
        public Task UpdateProductAsync(int id, Dish product);

        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="id">Id удаляемого объекта</param>
        /// <returns></returns>
        public Task DeleteProductAsync(int id);

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="product">Новый объект</param>
        /// <returns>Созданный объект</returns>
        public Task<ResponseData<Dish>> CreateProductAsync(Dish product);

        /// <summary>
        /// Сохранить файл изображения для объекта
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="formFile">Файл изображения</param>
        /// <returns>Url к файлу изображения</returns>
        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
    }

}
