
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WEB_253503_KOTOVA.API.Services.FileServices;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;

namespace WEB_253503_KOTOVA.UI.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiProductService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly IFileService _fileService;


        public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger, IFileService fileService)
        {
            _httpClient = httpClient;


            var apiUri = configuration["UriData:ApiUri"];
            if (!string.IsNullOrEmpty(apiUri))
            {
                _httpClient.BaseAddress = new Uri(apiUri);
            }

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _fileService = fileService;
        }

        public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            
            var urlString = new StringBuilder($"{_httpClient.BaseAddress}dishes/");

            // Добавляем категорию в маршрут
            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                urlString.Append($"{categoryNormalizedName}/");
            }

            // Добавляем номер страницы в маршрут
            if (pageNo > 1)
            {
                urlString.Append($"?pageNo={pageNo}");
            }

            // Добавляем размер страницы в строку запроса
            if (!pageSize.Equals(3))
            {
                urlString.Append($"&pageSize={pageSize}");
            }

            // Отправка запроса к API
            var response = await _httpClient.GetAsync(urlString.ToString());

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Dish>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Ошибка десериализации: {ex.Message}");
                    return ResponseData<ListModel<Dish>>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"Ошибка сервера: {response.StatusCode}");
            return ResponseData<ListModel<Dish>>.Error($"Данные не получены от сервера. Error: {response.StatusCode}");
        }

        public async Task<ResponseData<Dish>> CreateProductAsync(Dish product)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "dishes");

            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
                return data; // Возвращаем данные
            }

            _logger.LogError($"Объект не добавлен. Error: {response.StatusCode}");
            return ResponseData<Dish>.Error($"Объект не добавлен. Error: {response.StatusCode}");
        }

        public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
        {
            
            var urlString = $"{_httpClient.BaseAddress.AbsoluteUri}dishes/{id}";

            var response = await _httpClient.GetAsync(urlString);
            if (response.IsSuccessStatusCode)
            {
                var dish = await response.Content.ReadFromJsonAsync<Dish>();  
                if (dish != null)
                {
                    return ResponseData<Dish>.Success(dish); 
                }
            }

            return ResponseData<Dish>.Error("Dish not found");
        }

        public async Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile)
        {
            // Первоначально используем картинку по умолчанию
            product.Image = "Images/noimage.jpg";

            // Сохранить файл изображения, если он предоставлен
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                // Добавить в объект URL изображения
                if (!string.IsNullOrEmpty(imageUrl))
                    product.Image = imageUrl;
            }

            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Dishes");
            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
            }

            _logger.LogError($"-----> объект не создан. Error: {response.StatusCode}");
            return ResponseData<Dish>.Error($"Объект не добавлен. Error: {response.StatusCode}");
        }
        public async Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish product, IFormFile? formFile = null)
        {
            var existingDishResponse = await GetProductByIdAsync(id);
            if (!existingDishResponse.Successfull)
            {
                return ResponseData<Dish>.Error("Блюдо не найдено");
            }

            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    product.Image = imageUrl;
                }
            }

            // Отправляем обновленные данные блюда на сервер
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Dishes/{id}");
            var response = await _httpClient.PutAsJsonAsync(uri, product);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // Возвращаем локально обновленный объект, если сервер возвращает NoContent
                    return ResponseData<Dish>.Success(existingDishResponse.Data);
                }

                return await response.Content.ReadFromJsonAsync<ResponseData<Dish>>(_serializerOptions);
            }

            return ResponseData<Dish>.Error($"Ошибка обновления: {response.StatusCode}");
        }

        public async Task<ResponseData<object>> DeleteProductAsync(int id)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Dishes/{id}");
            var response = await _httpClient.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                return ResponseData<object>.Success(null); // Успешное удаление
            }

            return ResponseData<object>.Error($"Ошибка удаления: {response.StatusCode}");
        }

    }
}
