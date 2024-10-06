using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;

namespace WEB_253503_KOTOVA.API.Services.CategoryServices
{
    public interface ICategoryService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}
