﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Moq;
using RestaurantOpeningApi.Common;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Models;
using RestaurantOpeningApi.Repository;
using RestaurantOpeningApi.Services;
using System.Net.NetworkInformation;
using Xunit;

namespace RestaurantOpeningApi.Test.ServiceTest
{
    public class DataUploadServiceTest
    {
        [Fact]
        public async Task ProcessCsvFileAsync_CsvDataTest()
        {
            // Arrange
            var dataService = new RawDataParserService();

            // Create a memory stream with sample CSV data
            var csvData = "\"Kushi Tsuru\",\"Mon-Sun 11:30 am - 9 pm\"\n\"Osakaya Restaurant\",\"Mon-Thu, Sun 11:30 am - 9 pm  / Fri-Sat 11:30 am - 9:30 pm\"";
            using var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvData));

            // Act
            var data = await dataService.ProcessCsvFileAsync(memoryStream);

            // Assert
            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
            Assert.Equal("Kushi Tsuru", data.First().Name);
            Assert.Equal("Mon-Sun 11:30 am - 9 pm", data.First().OperatingTime);
        }
    }
}
