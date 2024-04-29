using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Moq;
using RestaurantOpeningApi.Common;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;
using RestaurantOpeningApi.Repository;
using RestaurantOpeningApi.Services;
using System.Net.NetworkInformation;
using Xunit;

namespace RestaurantOpeningApi.Test.ServiceTest
{
    public class DataUploadServiceTest
    {
        private readonly Mock<IRestaurantService> _restaurantServiceMock;
        private readonly RestaurantDataService _restaurantDataService;


        public DataUploadServiceTest()
        {
            _restaurantServiceMock = new Mock<IRestaurantService>();
            _restaurantDataService = new RestaurantDataService(_restaurantServiceMock.Object);
        }

        [Fact]
        public async Task ProcessCsvFileAsync_CsvDataTest()
        {
            // Arrange
            var dataService = new RawDataParserService();

            // Create a memory stream with sample CSV data
            var csvData = "\"Kushi Tsuru\",\"Mon-Sun 11:30 am - 9 pm\"\n\"Osakaya Restaurant\",\"Mon-Thu, Sun 11:30 am - 9 pm  / Fri-Sat 11:30 am - 9:30 pm\"";
            using var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvData));

            // Act
            var data = await dataService.ProcessCsvFileAsync(memoryStream);

            // Assert
            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
            Assert.Equal("Kushi Tsuru", data.First().Name);
            Assert.Equal("Mon-Sun 11:30 am - 9 pm", data.First().OperatingTime);
        }
       

        [Fact]
        public async Task AddRestaurantBatchAsync_ReturnTimeSpan_NotZero()
        {
            // Arrange
            var restaurants = new List<Restaurant>
            {
                new Restaurant {Id ="1", Name = "Kushi Tsuru", OperatingTime ="Mon-Sun 11:30 am - 9 pm"}
            };
            var batchSize = 10;           

            // Act
            var result = await _restaurantDataService.AddRestaurantBatchAsync(restaurants, batchSize);

            // Assert
            Assert.NotEqual(TimeSpan.Zero, result);
            // Add more assertions if needed
        }

        [Fact]
        public async Task GetRestaurantAsync_ReturnListOfRestaurants()
        {
            // Arrange
            var parameters = new RestaurantParameters
            {
                // Set your parameters here
                // For example:
                name = "Kushi Tsuru",
                day = "Monday"
            };
            var expectedStudents = new List<Restaurant>
            {
                new Restaurant {Id ="1", Name = "Kushi Tsuru", OperatingTime ="Mon-Sun 11:30 am - 9 pm"}
            };


            // var mockStudentService = new Mock<IStudentService>();
            _restaurantServiceMock.Setup(x => x.GetAllRestaurantAsync(parameters))
                              .ReturnsAsync(expectedStudents);



            // Act
            var result = await _restaurantDataService.GetRestaurantAsync(parameters);

            // Assert
            Assert.Equal(expectedStudents, result);
        }

    }
}
