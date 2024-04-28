using CsvHelper;
using CsvHelper.Configuration;
using RestaurantOpeningApi.Common;
using RestaurantOpeningApi.DTOs;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Repository
{
    public class RawDataParserService : IRawDataParser
    {
        public async Task<List<Restaurant>> ProcessCsvFileAsync(Stream fileStream)
        {
            var configuration = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                //set to false because the CSV file does not have a header record.
                HasHeaderRecord = false,
            };

            using var reader = new StreamReader(fileStream);
            using var csv = new CsvReader(reader, configuration);

            var records = new List<Restaurant>();

            await foreach (var record in csv.GetRecordsAsync<RestaurantRawData>())
            {

                var restaurant = new Restaurant
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = record.RestaurantName,
                    OperatingTime = record.OperatingHours
                };
               
                restaurant.restaurantTimes = await ParseRestaurantOperatingTime(record.OperatingHours, restaurant.Id);

                records.Add(restaurant);
            }

            return records;
        }

        public async Task<List<RestaurantTime>> ParseRestaurantOperatingTime(string operatingTime, string restaurantId)
        {
            List<RestaurantTime> restaurantTimes = new List<RestaurantTime>();

            foreach (var item in CommonManagement.ParseRestaurantTimeString(operatingTime))
            {
                TimeSpan totalTime = CommonManagement.GetTotalTimeDuration(item.OpeningTime,item.ClosingTime);
                var restaurantTime = new RestaurantTime
                {
                    Id = Guid.NewGuid().ToString(),
                    RestaurantId = restaurantId,
                    OpeningDay = item.OpeningDay,
                    OpeningTime = item.OpeningTime,
                    ClosingTime = item.ClosingTime,
                    TotalTimeDuration = totalTime
                };

                restaurantTimes.Add(restaurantTime);
            }

            return restaurantTimes;
        }

    }
}
