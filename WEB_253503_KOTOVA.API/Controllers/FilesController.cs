using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace WEB_253503_KOTOVA.API.Controllers
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class FilesController : ControllerBase
        {
            // Путь к папке wwwroot/Images
            private readonly string _imagePath;

            public FilesController(IWebHostEnvironment webHost)
            {
                _imagePath = Path.Combine(webHost.WebRootPath, "Images");
            }

            [HttpPost]
            public async Task<IActionResult> SaveFile(IFormFile file)
            {
                if (file == null)
                {
                    return BadRequest("Файл не выбран.");
                }

                // Путь к сохраненному файлу
                var filePath = Path.Combine(_imagePath, file.FileName);
                var fileInfo = new FileInfo(filePath);

                // Если такой файл существует, удаляем его
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                // Сохраняем файл
                using (var fileStream = fileInfo.Create())
                {
                    await file.CopyToAsync(fileStream);
                }

                // Получаем URL сохраненного файла
                var host = HttpContext.Request.Host;
                var fileUrl = $"https://{host}/Images/{file.FileName}";

                return Ok(fileUrl);
            }

            [HttpDelete("{fileName}")]
            public IActionResult DeleteFile(string fileName)
            {
                var filePath = Path.Combine(_imagePath, fileName);
                var fileInfo = new FileInfo(filePath);

                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                    return Ok("Файл удален.");
                }

                return NotFound("Файл не найден.");
            }
        }
    }

