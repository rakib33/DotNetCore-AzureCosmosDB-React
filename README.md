# Restaurant Opening Hours API

## Introduction

This is a .Net Core Web API project with Cosmos DB. The objective of this sample project is to develop an API server and easy navigation and interaction with restaurant opening hours data. We are using Angular for the frontend, ASP.Net Core for the backend, and Cosmos DB for the database.

## Raw Data

Here is the raw data for restaurant opening hours: [hours.csv](https://gist.githubusercontent.com/ramzan-bs-23/bbc98dc64516242ccdb37fbd08b7cc4f/raw/dd55d01a7fc8efcbb08c5a2062b5c59c3c0471f9/hours.csv).

We treat this data as a raw source that requires extraction, transformation, and loading into the database.

## Goals 

- Create a ASP.Net Core Web API project from scratch.  
- Create database and table using Azure Cosmos DB Emulator for local machine.
- Configure database connection using.
- Install necessary nuget packages.  
- Create code first database migration.
- Create api to load raw data from csv.
- Create api to display resturant data.
- Resturant data filtering by name , date and time.
- Restaurant data pagination functionality
- Restaurant data sorting functionality
- Repository pattern Dependency Injection configuration.
- xUnit testing.
- Create a Angular project from screatch.
- Create component for the api.  
- Simple Bootstrap design.
- File upload option.
- Get resturant data and display.
- Angular datatable for pagination and sorting. 
- Configure unit test code coverage and generate reports.

## Azure Cosmos DB Emulator

We are using Azure Cosmos DB capabilities and features using the Azure Cosmos DB Emulator because we don't need to purchase any services from Azure right now.
Download Azure Cosmos DB Emulator for local machine from ([cosmosdb-emulator](https://aka.ms/cosmosdb-emulator))

## Technology Used

- ASP.NET Core Web API
- Framework .Net 6
- Database Cosmos DB
- Angular for UI

## Install Package

  - Install package Microsoft.EntityFrameworkCore version 6.0.14
  - Microsoft.EntityFrmaeworkCore Version-6.0.14
  - Microsoft.EntityFrameworkCore.Cosmos Version-6.0.14
  - Microsoft.EntityFrmaeworkCore.Tools Version-6.0.14
  - Microsoft.AspNetCore.Cors Version-2.2.0
  - CsvHelper Version-31.0.3
  - Moq
  - xunit
  - xunit.runner.visualstudio    

## Create .Net Web API Project

To get started developing a Web API in Visual Studio the first step is to create a new project. In Visual Studio 2022 you can create a new project using the New Project dialog. 

- Open Visual Studio 2022 go to File -> New -> Project..

  ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/427b15a2-7d99-4c7c-bd72-c35009c18bf7)

- Project Dialogue box is open.Select ASP.NET Core Web API and click Next button.

  ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/64765fe4-97a7-4c71-ac90-0308acc5d007)

- Configure window is opening. Give a project name and got to next.

  ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/9c23857f-14eb-44c2-b3fc-b49f88cf35dc)

- Select Framework and Create the project

  ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/15054e10-ef7a-4ec7-beaa-3d053f075421)

## Csv Data Uploading API

For data upload from given csv file to database we need to parse the data and then save into database. We can normalize the data into two table. For restaurant name is one table and opening day with time is another table. Now we will create an uploading api for csv and data transfer object model.

- Create a folder DTOs and create a model class RestaurantRawData.cs

  ```
   public class RestaurantRawData
   {
        public string RestaurantName { get; set; }
        public string OperatingHours { get; set; }
   }
  ```
- Create a folder name Interfaces and create an Interface class IDataUploadService.cs

  ```
   public interface IDataUploadService
   {
        Task<IEnumerable<RestaurantRawData>> ProcessCsvFileAsync(Stream fileStream);
   }
  ```
- Create a folder Repository and implement this interface . Create implementing class DataUploadService.cs

  ```
    using CsvHelper;
    using CsvHelper.Configuration;
    using RestaurantOpeningApi.DTOs;
    using RestaurantOpeningApi.Interfaces;
    
    namespace RestaurantOpeningApi.Repository
    {
        public class DataUploadService : IDataUploadService
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
    
  ```

- Now we need to inject this service to our project in Program.cs file. Open the file and put this line of code.

  ```  
   builder.Services.AddScoped<IDataUploadService, DataUploadService>();
  ```
- Create a web api controller in Controllers folder named DataUploadController.cs to upload csv file

  ```
    using Microsoft.AspNetCore.Mvc;
    using RestaurantOpeningApi.Interfaces;
    
    namespace RestaurantOpeningApi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class DataUploadController : ControllerBase
        {   
            private readonly IDataUploadService _dataService;
            public DataUploadController(IDataUploadService dataService)
            {
                _dataService = dataService;       
            }
    
            [HttpPost("upload")]
            public async Task<IActionResult> UploadCsvFile(IFormFile file)
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Invalid file.");
                }
    
                using var fileStream = file.OpenReadStream();
                var data = await _dataService.ProcessCsvFileAsync(fileStream);
    
                // Return a successful response with status code 201 Created
                return StatusCode(StatusCodes.Status201Created, "File uploaded successfully.");
            }
        }
    }
  ```
- Run the application and check in swagger Ui . Hear is an upload option to upload file 

  ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/368add46-a066-457c-b3eb-7e63e9fc501c)

## Create Angular App

- Install Node in your pc https://nodejs.org/en/download
- Open command promt as admin and check node version.
- Install angular cli 
  ```
  npm install -g @angular/cli
  ```
- create new angular app
  ```
  ng new restaurantApp
  ```
- Open visual studio code editor
  ```
  code .
  ```
- Open project src folder in vs code editor
  ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/7cf14011-f9b9-41bc-8f49-43226b1f5a36)
  
  
## References

- https://github.com/Azure-Samples/cosmos-dotnet-core-todo-app
- https://github.com/Azure/azure-cosmos-dotnet-v3
- https://www.c-sharpcorner.com/article/angular-app-with-asp-net-core-and-cosmos-db/
- https://www.codeproject.com/Articles/1256191/Angular-6-Application-with-Cosmos-DB-and-Web-API-2
