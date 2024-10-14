﻿using Microsoft.AspNetCore.Http;

namespace WEB_253503_KOTOVA.API.Services.FileServices
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;

        public ApiFileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            if (formFile == null)
            {
                throw new ArgumentNullException(nameof(formFile));
            }

            var extension = Path.GetExtension(formFile.FileName);
            var newFileName = $"{Guid.NewGuid()}{extension}";

            var uploadPath = Path.Combine("wwwroot", "Images");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, newFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            // Возвращаем относительный путь к файлу
            return $"/Images/{newFileName}";
        }


        public async Task DeleteFileAsync(string fileName)
        {
            var response = await _httpClient.DeleteAsync($"Files/{fileName}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Ошибка при удалении файла");
            }
        }
    }
}
