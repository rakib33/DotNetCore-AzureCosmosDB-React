using CsvHelper;
using CsvHelper.Configuration;
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

                records.Add(new Restaurant { 
                  Id = Guid.NewGuid().ToString(),
                  Name = record.RestaurantName,
                  OperatingTime = record.OperatingHours,
                });
            }

            return records;
        }

    }
}
