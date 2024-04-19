using CsvHelper;
using CsvHelper.Configuration;
using RestaurantOpeningApi.DTOs;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.Repository
{
    public class RawDataParserService : IRawDataParser
    {
        public async Task<IEnumerable<RestaurantRawData>> ProcessCsvFileAsync(Stream fileStream)
        {
            var configuration = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                //set to false because the CSV file does not have a header record.
                HasHeaderRecord = false,
            };

            using var reader = new StreamReader(fileStream);
            using var csv = new CsvReader(reader, configuration);

            var records = new List<RestaurantRawData>();

            await foreach (var record in csv.GetRecordsAsync<RestaurantRawData>())
            {
                records.Add(record);
            }

            return records;
        }

    }
}
