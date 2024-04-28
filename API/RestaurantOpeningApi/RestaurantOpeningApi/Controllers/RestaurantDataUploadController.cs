using Microsoft.AspNetCore.Mvc;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantDataUploadController : ControllerBase
    {   
        private readonly IRawDataParser _dataService;
        private readonly IRestaurantService _restaurantService;
        public RestaurantDataUploadController(IRawDataParser dataService , IRestaurantService restaurantService)
        {
            _dataService = dataService;     
            _restaurantService = restaurantService;
        }
        

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsvFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Check the file extension
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".csv")
            {
                return BadRequest("Invalid file type. Only CSV files are allowed.");
            }

            using var fileStream = file.OpenReadStream();
            List<Restaurant> restaurants = await _dataService.ProcessCsvFileAsync(fileStream);

            if (restaurants.Count() > 0)
            {
                // process this data and save to database                

                try
                {                  
                    await _restaurantService.AddBulkRestaurantAsync(restaurants);
                    await _restaurantService.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status201Created, "Data uploaded successfully.");
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Data uploaded failed.");
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "File don't have any data.");
            }

            
          
        }
    }
}
