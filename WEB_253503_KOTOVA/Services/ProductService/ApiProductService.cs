
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;

namespace WEB_253503_KOTOVA.UI.Services.ProductService
{
    public class ApiProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiProductService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;

        public ApiProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiProductService> logger)
        {
            _httpClient = httpClient;

            // Установка BaseAddress для HttpClient из конфигурации
            var apiUri = configuration["UriData:ApiUri"];
            if (!string.IsNullOrEmpty(apiUri))
            {
                _httpClient.BaseAddress = new Uri(apiUri); // Устанавливаем базовый URL из конфигурации
            }

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            // Подготовка URL запроса
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

    }
}
