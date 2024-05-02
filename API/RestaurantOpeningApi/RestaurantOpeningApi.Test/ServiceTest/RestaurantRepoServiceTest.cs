using Microsoft.EntityFrameworkCore;
using RestaurantOpeningApi.Common;
using RestaurantOpeningApi.DataContext;
using RestaurantOpeningApi.Services;
using Xunit;
using RestaurantOpeningApi.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RestaurantOpeningApi.Test.ServiceTest
{
    public class RestaurantRepoServiceTest
    {
        private readonly RestaurantRepoService _service;
        private readonly RestaurantContext _context;        
        private readonly DbContextOptions<RestaurantContext> _DbContextOptions;
       public RestaurantRepoServiceTest() 
        {

            //var options = new DbContextOptionsBuilder<RestaurantContext>().UseCosmos(
            //     "https://localhost:8081",
            //     "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            //      databaseName: "restaurant-db").Options;

            //context = new RestaurantContext(options);

            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
           var options = new DbContextOptionsBuilder<RestaurantContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .UseInternalServiceProvider(serviceProvider).Options;
            _context = new RestaurantContext(options);
            _service = new RestaurantRepoService(_context);
        }

        [Fact]
        public async Task GetRestaurantAsync_ReturnNullValueCheck()
        {

            // Arrange
            var parameters = new RestaurantParameters
            {
                // Set your parameters here
                // For example:
                name = "Kushi Tsuru",
                day = "Monday"
            };
        
     
            // Act
            var result = (await _service.GetAllRestaurantAsync(parameters));
           
            // Assert
            Assert.Null(result);

        }

        //[Fact]
        //public async Task GetRestaurant_ReturnsRestaurantListAsync()
        //{
        //    // Arrange
        //    var parameters = new RestaurantParameters
        //    {
        //        // Set your parameters here
        //        // For example:
        //        name = "Kushi Tsuru",
        //        day = "Monday"
        //    };
        //    var mock = RestaurantDataHelper.GetFakeRestaurantList().BuildMock().BuildMockDbSet();

        //    var ContextMock = new Mock<RestaurantContext>();
        //         ContextMock.Setup(x => x.Restaurants).Returns(mock.Object);
            
        //    //Act
           
        //    RestaurantRepoService restaurantServic = new RestaurantRepoService(ContextMock.Object);

        //    var restaurant = await restaurantServic.GetAllRestaurantAsync(parameters);
        //    //Assert
        //    Assert.NotNull(restaurant);
        //    Assert.Equal(2,restaurant.Count());
        //}
    }
}
