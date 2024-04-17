using RestaurantOpeningApi.DTOs;

namespace RestaurantOpeningApi.Interfaces
{
    public interface IDataLoadingService
    {
        Task<IEnumerable<RestaurantRawData>> ProcessCsvFileAsync(Stream fileStream);
    }
}
