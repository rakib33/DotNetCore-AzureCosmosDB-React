using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly IRestaurantDataService _restaurantDataService;
        public RestaurantDataUploadController(IRawDataParser dataService , IRestaurantDataService restaurantDataService)
        {
            _dataService = dataService;     
            _restaurantDataService = restaurantDataService;
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
                    var responseTime = await _restaurantDataService.AddRestaurantBatchAsync(restaurants,100);
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

        /// <summary>
        ///  get all restaurant
        /// </summary>
        /// <returns>The list of restaurant </returns>
        [HttpGet("GetRestaurantData")]
        [ProducesResponseType(typeof(List<Restaurant>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync(int currentPage, int totalItemPerPage,string name,string day,string time)
        {
             RestaurantParameters restaurantParameters = new RestaurantParameters();
            var restaurantData = await _restaurantDataService.GetRestaurantAsync(restaurantParameters);

           // restaurants = restaurants.Skip((p.Pagination.PageNumber - 1) * p.Pagination.PageSize).Take(p.Pagination.PageSize).ToList();
            //var metadata = new
            //{
            //    owners.TotalCount,
            //    owners.PageSize,
            //    owners.CurrentPage,
            //    owners.TotalPages,
            //    owners.HasNext,
            //    owners.HasPrevious
            //};
            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            //_logger.LogInfo($"Returned {owners.TotalCount} owners from database.");
            //return Ok(owners);
            return  restaurantData;

        }
    }
}
