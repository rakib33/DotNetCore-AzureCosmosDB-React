using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantOpeningApi.Common;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantDataUploadController : ControllerBase
    {   
        private readonly IRawDataParser _dataService;
        private readonly IRestaurantDataService _restaurantService;
        public RestaurantDataUploadController(IRawDataParser dataService , IRestaurantDataService restaurantService)
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
                    await _restaurantService.AddRestaurantBatchAsync(restaurants,100);   
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

        [HttpGet("GetRestaurants")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants(string name,string day,TimeSpan time, int page = 1, int pageSize = 50)
        {
            RestaurantParameters parms = new RestaurantParameters();
            parms.Pagination.PageNumber = page;
            parms.Pagination.PageSize = pageSize;
            parms.name = name;
            parms.day = day;
            parms.time = time;

            var items = await _restaurantService.GetRestaurantAsync(parms);

            /*
             .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

             */

            return Ok(items);
        }
    }
}
