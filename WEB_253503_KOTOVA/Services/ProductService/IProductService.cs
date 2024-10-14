﻿using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;

namespace WEB_253503_KOTOVA.UI.Services.ProductService
{
    public interface IProductService
    {
        
        public Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3);
        public Task<ResponseData<Dish>> GetProductByIdAsync(int id);

        public Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product, IFormFile? formFile);
        Task<ResponseData<object>> DeleteProductAsync(int id);

        public Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile);

    }
}
