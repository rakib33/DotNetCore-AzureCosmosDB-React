using Moq;
using RestaurantOpeningApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RestaurantOpeningApi.Test.ServiceTest
{
    public class DataUploadServiceTest
    {
        [Fact]
        public async Task ProcessCsvFileAsync_CsvDataTest()
        {
            // Arrange
            var dataService = new DataUploadService();

            // Create a memory stream with sample CSV data
            var csvData = "\"Kushi Tsuru\",\"Mon-Sun 11:30 am - 9 pm\"\n\"Osakaya Restaurant\",\"Mon-Thu, Sun 11:30 am - 9 pm  / Fri-Sat 11:30 am - 9:30 pm\"";
            using var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvData));

            // Act
            var data = await dataService.ProcessCsvFileAsync(memoryStream);

            // Assert
            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
            Assert.Equal("Kushi Tsuru", data.First().RestaurantName);
            Assert.Equal("Mon-Sun 11:30 am - 9 pm", data.First().OperatingHours);
        }
        
    }
}
