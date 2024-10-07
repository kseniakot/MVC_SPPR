using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;

namespace WEB_253503_KOTOVA.UI.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiCategoryService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;

        public ApiCategoryService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiCategoryService> logger)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        // Метод для получения списка категорий
        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            // Отправка GET запроса к API для получения списка категорий
            var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}categories");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    // Чтение и десериализация ответа
                    var result = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
                    return result;
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"Ошибка десериализации: {ex.Message}");
                    return ResponseData<List<Category>>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"Ошибка сервера: {response.StatusCode}");
            return ResponseData<List<Category>>.Error($"Данные не получены от сервера. Error: {response.StatusCode}");
        }

        // Метод для создания новой категории
        public async Task<ResponseData<Category>> CreateCategoryAsync(Category category)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "categories");

            var response = await _httpClient.PostAsJsonAsync(uri, category, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<Category>>(_serializerOptions);
                return result;
            }

            _logger.LogError($"Категория не добавлена. Error: {response.StatusCode}");
            return ResponseData<Category>.Error($"Категория не добавлена. Error: {response.StatusCode}");
        }

       
    }
}
