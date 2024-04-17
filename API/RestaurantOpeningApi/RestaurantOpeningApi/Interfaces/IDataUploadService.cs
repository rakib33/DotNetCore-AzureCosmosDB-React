using RestaurantOpeningApi.DTOs;

namespace RestaurantOpeningApi.Interfaces
{
    public interface IDataUploadService
    {
        Task<IEnumerable<RestaurantRawData>> ProcessCsvFileAsync(Stream fileStream);
    }
}
