using RestaurantOpeningApi.DTOs;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Interfaces
{
    public interface IRawDataParser
    {
        Task<IEnumerable<RestaurantRawData>> ProcessCsvFileAsync(Stream fileStream);
     
    }
}
