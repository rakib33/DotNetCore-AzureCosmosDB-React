using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Moq;
using RestaurantOpeningApi.Common;
using RestaurantOpeningApi.Controllers;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RestaurantOpeningApi.Test.ControllerTest
{
    public class RestaurantControllerTest
    {
        private readonly RestaurantDataUploadController _restaurantDataUploadController;
        private readonly Mock<IRawDataParser> _dataService;
        private readonly Mock<IRestaurantDataService> _restaurantService;
        public RestaurantControllerTest()
        {
            _dataService = new Mock<IRawDataParser>();
            _restaurantService = new Mock<IRestaurantDataService>();
            _restaurantDataUploadController = new RestaurantDataUploadController(_dataService.Object,_restaurantService.Object);
        }

        [Fact]
        public void GetRestaurants_ReturnIActionResult()
        {
            //Arrange
            var ExpectedResut = new List<Restaurant> { };
            //act
            var result = _restaurantDataUploadController.GetRestaurants("","","");
            //assert
            Assert.NotNull(result);
            //assert
            Assert.IsAssignableFrom<Task<IActionResult>>(result);
        }

        [Fact]
        public void UploadCsvFile_ReturnIActionResult()
        {
            var csvData = "\"Kushi Tsuru\",\"Mon-Sun 11:30 am - 9 pm\"\n\"Osakaya Restaurant\",\"Mon-Thu, Sun 11:30 am - 9 pm  / Fri-Sat 11:30 am - 9:30 pm\"";
            using var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvData));

        }

        [Fact]
        public async Task UploadCsvFile_WhenFileIsNull_ShouldReturnBadRequest()
        {
            // Arrange           
            var nullFile = null as IFormFile;
            // Act
            var result = await _restaurantDataUploadController.UploadCsvFile(nullFile);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file uploaded.", badRequestResult.Value);
        }

        [Fact]
        public async Task UploadCsvFile_WhenFileHasZeroLength_ShouldReturnBadRequest()
        {
            // Arrange           
            var emptyFile = new FormFile(null, 0, 0, "file", "empty.csv");

            // Act
            var result = await _restaurantDataUploadController.UploadCsvFile(emptyFile);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file uploaded.", badRequestResult.Value);
        }

    }
}
