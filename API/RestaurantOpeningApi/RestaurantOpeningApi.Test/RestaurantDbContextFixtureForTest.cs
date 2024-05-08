using Microsoft.EntityFrameworkCore;
using RestaurantOpeningApi.DataContext;
using RestaurantOpeningApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOpeningApi.Test
{

    //Configure DbContext for in-memory database
    public class RestaurantDbContextFixtureForTest : IDisposable
    {
        public RestaurantContext    DbContext { get; private set; }
        public RestaurantDbContextFixtureForTest() {

            //use in memory database for testing
            var options = new DbContextOptionsBuilder<RestaurantContext>().UseCosmos(
                accountEndpoint: "https://localhost:8081",
                accountKey: "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                databaseName: "restaurant-db").Options;
            DbContext = new RestaurantContext(options);

            // Add fack data
            DbContext.Restaurants.AddRange(new List<Restaurant>
            {
                new Restaurant {Id ="1", Name = "Kushi Tsuru", OperatingTime ="Mon-Sun 11:30 am - 9 pm"},
                new Restaurant {Id ="1", Name = "Kushi Tsuru 2", OperatingTime ="Mon-Sun, Fri 11:30 am - 9 pm"},
            });

          //  DbContext.SaveChanges();
        }
        public void Dispose()
        {
            // Clean up after tests
            DbContext.Dispose();
        }
    }
}
