
using WEB_253503_KOTOVA.Domain.Models;
using WEB_253503_KOTOVA.Domain.Entities;

namespace WEB_253503_KOTOVA.UI.Services.CategoryService
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
