using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantOpeningApi.Models;
using RestaurantOpeningApi.Services;
using System.Net;

namespace RestaurantOpeningApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantDataController : ControllerBase
    {
        private readonly RestaurentDataService _restaurentDataService;
        public RestaurantDataController(RestaurentDataService restaurentDataService)
        {
            _restaurentDataService = restaurentDataService;
        }

        [HttpPost("SaveRestaurant")]
        public async Task<IActionResult> SaveRestaurantData()
        {
          
            try
            {
               var IsSaved =  await  _restaurentDataService.UploadRestaurentDataToCosmosDB();
                // Return 201 Created and provide the location of the created resource
                return CreatedAtAction(nameof(SaveRestaurantData),IsSaved);             

            }
            catch (Exception ex)
            {
                // Handle any unexpected errors and return 500 Internal Server Error
                return StatusCode((int)HttpStatusCode.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
