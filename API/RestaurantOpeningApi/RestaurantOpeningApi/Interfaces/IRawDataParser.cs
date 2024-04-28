using RestaurantOpeningApi.DTOs;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Interfaces
{
    public interface IRawDataParser
    {
        Task<List<Restaurant>> ProcessCsvFileAsync(Stream fileStream);
        Task<List<RestaurantTime>> ParseRestaurantOperatingTime(string operatingTime, string restaurantId);

    }
}
