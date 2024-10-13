using Microsoft.AspNetCore.Http;

namespace WEB_253503_KOTOVA.API.Services.FileServices
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpContext _httpContext;

        public ApiFileService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContext = httpContextAccessor.HttpContext; // Получение текущего HttpContext
        }

        public async Task DeleteFileAsync(string fileUri)
        {
            // Убедитесь, что URI файла не пустой
            if (string.IsNullOrEmpty(fileUri))
            {
                throw new ArgumentException("URI файла не может быть пустым", nameof(fileUri));
            }

            // Создаем DELETE запрос к API
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(fileUri) // Указываем URI файла, который нужно удалить
            };

            // Отправляем запрос на сервер
            var response = await _httpClient.SendAsync(request);

            // Проверяем успешен ли ответ
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при удалении файла: {response.ReasonPhrase}");
            }
        }


        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            // Создать объект запроса
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };

            // Сформировать случайное имя файла, сохранив расширение
            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);

            // Создать контент, содержащий поток загруженного файла
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", newName);

            // Поместить контент в запрос
            request.Content = content;

            // Отправить запрос к API
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                // Вернуть полученный Url сохраненного файла
                return await response.Content.ReadAsStringAsync();
            }

            return String.Empty;
        }
    }

}
