using RestaurantOpeningApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantOpeningApi.Test
{
    public class RestaurantDataHelper
    {
        public static List<Restaurant> GetFakeRestaurantList()
        {
            return new List<Restaurant>()
            {
                new Restaurant {Id ="1", Name = "Kushi Tsuru", OperatingTime ="Mon-Sun 11:30 am - 9 pm"},
                new Restaurant {Id ="1", Name = "Kushi Tsuru 2", OperatingTime ="Mon-Sun, Fri 11:30 am - 9 pm"},
            };
        }
    }
}
