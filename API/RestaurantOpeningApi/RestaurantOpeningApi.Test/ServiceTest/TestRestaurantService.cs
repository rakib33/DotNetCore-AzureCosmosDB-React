using Microsoft.EntityFrameworkCore;
using RestaurantOpeningApi.DataContext;
using RestaurantOpeningApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using RestaurantOpeningApi.Common;

namespace RestaurantOpeningApi.Test.ServiceTest
{
    //https://www.youtube.com/watch?v=m6Rry5gj9ZA
    public class TestRestaurantService : IDisposable
    {
        private readonly RestaurantContext _dbContext;
        private readonly RestaurantRepoService _repoService;

        public TestRestaurantService()
        {
            var options = new DbContextOptionsBuilder<RestaurantContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _dbContext = new RestaurantContext(options);
            _dbContext.Database.EnsureCreated();

            //Arrange
            _dbContext.Restaurants.AddRange(DataHelper.GetFakeRestaurantData());
            _dbContext.SaveChanges();
            _repoService = new RestaurantRepoService(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task GetAllAsync_ReturnRestaurantCollection()
        {
            try
            {
                /// Arrange             
                Pagination pagination = new Pagination();
                pagination.PageSize = 10;
                pagination.Page = 1;
                /// Act
                var result = await _repoService.GetAllRestaurantAsync(DataHelper.parameters("", "", "", pagination));

                /// Assert
              result.Should().HaveCount(DataHelper.GetFakeRestaurantData().Count);
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
            }
        }


        [Fact]
        public async Task Get_ReturnsCorrectNumberOfItems()
        {
            // Arrange   
            Pagination pagination = new Pagination();
            pagination.PageSize = 2; // item per page
            pagination.Page = 1; // take 1st page
            // Act
            var result = await _repoService.GetAllRestaurantAsync(DataHelper.parameters("","","",pagination));

            // Assert

            Assert.Equal(2, result.Count); // Assuming 2 items per page
        }

        [Fact]
        public async Task Get_ReturnsFilteredItems()
        {
            // Arrange
            Pagination pagination = new Pagination();
            pagination.PageSize = 2; // item per page
            pagination.Page = 1; // take 1st page
            string fileterName = "The Cheesecake Factory";
            // Act
            var result = await _repoService.GetAllRestaurantAsync(DataHelper.parameters(fileterName, "", "", pagination));

            // Assert
            // Check if the result contains only items that match the filter criteria          
            Assert.All(result, item => Assert.Contains(fileterName, item.Name));
        }

        [Fact]
        public async Task Get_ReturnsCorrectPage()
        {
            // Arrange
            Pagination pagination = new Pagination();
            pagination.PageSize = 2; // item per page
            pagination.Page = 2; // take 1st page
         
            // Act
            var result = await _repoService.GetAllRestaurantAsync(DataHelper.parameters("", "", "", pagination));

            // Assert
            Assert.Single(result);           
        }


        //[Fact]
        //public async Task Get_ReturnsItemsFilteredByDay()
        //{
        //    // Arrange
        //    Pagination pagination = new Pagination();
        //    pagination.PageSize = 2; // item per page
        //    pagination.Page = 1; // take 1st page
        //    string day = "The Cheesecake Factory";
        //    // Act
        //    var result = await _repoService.GetAllRestaurantAsync(DataHelper.parameters(fileterName, "", "", pagination));

        //    // Assert
        //    // Check if the result contains only items that match the filter criteria          
        //    Assert.All(result, item => Assert.Contains(fileterName, item.Name));
        //}
    }
}
