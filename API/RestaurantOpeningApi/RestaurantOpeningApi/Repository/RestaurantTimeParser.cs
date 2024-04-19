using Microsoft.AspNetCore.Http;
using RestaurantOpeningApi.Common;
using RestaurantOpeningApi.DTOs;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;
using System.Text.RegularExpressions;

namespace RestaurantOpeningApi.Repository
{
    public class RestaurantTimeParser : IRestaurantTimeParser
    {

        public Task<IEnumerable<Restaurant>> ParseRestaurantRawData(IEnumerable<RestaurantRawData> restaurantRawDatas)
        {
            IEnumerable<Restaurant> restaurants = new List<Restaurant>();
            foreach (var item in restaurantRawDatas)
            {
                              
            }

            throw new NotImplementedException();
        }

        public Task<IEnumerable<RestaurantTime>> ParseRestaurantOperatingTime(string operatingTime)
        {
            IEnumerable<RestaurantTimeParser> restaurantTimeParsers = new List<RestaurantTimeParser>();
            
            var test1 = "Mon, Fri 5 pm - 6:15 pm /Mon-Thu, Fri 5 pm - 6:15 pm / Thurs- Sun 10 am - 3 pm / Sun 6:30 am - 12:45 pm";
            var test2 = "Mon-Wed 5 pm - 12:30 am  / Thu-Fri 5 pm - 1:30 am  / Sat 3 pm - 1:30 am  / Sun 3 pm - 11:30 pm" ; // test passed
            var test3 = "Mon-Sun 11:30 am - 9 pm" ;
            var test4 = "Sun 11:30 am - 9 pm" ;

            var parts = test2.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                Dictionary<string, string> keyValuePairs = CommonManagement.ParseOperatingDayAndTime(part);
            }                 

            return null;
        }
    }
}
