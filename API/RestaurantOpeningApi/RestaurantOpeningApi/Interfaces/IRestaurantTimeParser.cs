using RestaurantOpeningApi.DTOs;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Interfaces
{
    public interface IRestaurantTimeParser
    {
        Task<IEnumerable<Restaurant>> ParseRestaurantRawData(IEnumerable<RestaurantRawData> restaurantRawDatas);
        Task<IEnumerable<RestaurantTime>> ParseRestaurantOperatingTime(string operatingTime);
    }
}
