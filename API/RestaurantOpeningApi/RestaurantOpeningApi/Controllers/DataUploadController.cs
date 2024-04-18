using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantOpeningApi.DataContext;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;
using System.Net;

namespace RestaurantOpeningApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataUploadController : ControllerBase
    {
        
        private readonly RestaurantContext _context;
        private static bool _ensureCreated { get; set; } = false;
        
        private readonly IDataUploadService _dataService;
        private readonly IRestaurantTimeService _restaurantService;

        public DataUploadController(IDataUploadService dataService, IRestaurantTimeService restaurantService)
        {
            _dataService = dataService;
            _restaurantService = restaurantService;
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


        [HttpPost("SaveRestaurant")]
        public async Task<IActionResult> SaveRestaurantData()
        {
            //if (dataModel == null)
            //{
            //    // Return 400 Bad Request
            //    return StatusCode((int)HttpStatusCode.BadRequest, "Data model cannot be null.");
            //}
            var RestaurantData = new Restaurant()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test Name 2"
            };
            // Time value as a string
            string timeString1 = "11:30 am";
            string timeString2 = "9 pm";

            // Parse the time string into a TimeOnly structure
            TimeOnly time1 = TimeOnly.Parse(timeString1);
            TimeOnly time2 = TimeOnly.Parse(timeString2);

            var RestaurentTime = new RestaurantTime()
            {
                Id = Guid.NewGuid().ToString(),
                RestaurantId = RestaurantData.Id,
                Restaurant = RestaurantData,
                OpeningDay = "Mon",
                OpeningTime = time1,
                ClosingTime = time2
            };

            try
            {
                // Call the data service to save the data
                //    var data =await _restaurantService.AddRestaurant(RestaurantData);       

                // Return 201 Created and provide the location of the created resource
                //  return CreatedAtAction(nameof(SaveRestaurantData),data);
                return null;
         
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors and return 500 Internal Server Error
                return StatusCode((int)HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }


    }
}
