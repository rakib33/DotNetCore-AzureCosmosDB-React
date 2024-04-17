using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantOpeningApi.Interfaces;

namespace RestaurantOpeningApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataUploadController : ControllerBase
    {
        private readonly IDataUploadService _dataService;

        public DataUploadController(IDataUploadService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsvFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            using var fileStream = file.OpenReadStream();
            var data = await _dataService.ProcessCsvFileAsync(fileStream);

            return Ok(data);
        }
    }
}
