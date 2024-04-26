using Microsoft.AspNetCore.Mvc;
using RestaurantOpeningApi.Common;
using RestaurantOpeningApi.DTOs;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;
using RestaurantOpeningApi.Repository;

namespace RestaurantOpeningApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantDataUploadController : ControllerBase
    {   
        private readonly IRawDataParser _dataService;
        private readonly IRestaurantRawDataService _restaurantRawDataService;
        public RestaurantDataUploadController(IRawDataParser dataService , IRestaurantRawDataService restaurantRawDataService)
        {
            _dataService = dataService;     
            _restaurantRawDataService = restaurantRawDataService;
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

                try
                {
                    var responseTime = await _restaurantRawDataService.AddRestaurantBatchAsync(restaurants,100);
                    return Ok(StatusCode(StatusCodes.Status201Created, "Data uploaded successfully. UploadTime:"+ responseTime));
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
