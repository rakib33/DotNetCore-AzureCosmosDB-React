using Microsoft.AspNetCore.Mvc;
using RestaurantOpeningApi.DTOs;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;
using RestaurantOpeningApi.Repository;

namespace RestaurantOpeningApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataUploadController : ControllerBase
    {   
        private readonly IRawDataParser _dataService;
        private readonly IRestaurantTimeParser _restaurantTimeParser;
        public DataUploadController(IRawDataParser dataService , IRestaurantTimeParser restaurantTimeParser)
        {
            _dataService = dataService;     
            _restaurantTimeParser = restaurantTimeParser;
        }

        [HttpGet("ParseOperatingTime")]
        public async Task<IActionResult> OperatingTimeParse()
        {
            IEnumerable<RestaurantTime> restaurants = await _restaurantTimeParser.ParseRestaurantOperatingTime("");
            return Ok();
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
            IEnumerable<RestaurantRawData> data  = await _dataService.ProcessCsvFileAsync(fileStream);

            if(data.Count() > 0)
            {
                // process this data and save to database
                IEnumerable<Restaurant> restaurants = await _restaurantTimeParser.ParseRestaurantRawData(data);
            }


            // Return a successful response with status code 201 Created
            //return StatusCode(StatusCodes.Status201Created, "File uploaded successfully.");
            return Ok(data);
        }
    }
}
