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
- Create Model folder and add class Restaurant.cs that containes restaurant name and operating time
  ```
  public class Restaurant
  {
      [Key]      
      public string Id { get; set; }


      [Required]
      [MaxLength(500)]
      [Display(Name = "Restaurant Name")]
      public string Name { get; set; }
      public string OperatingTime { get; set; }

      // Navigation property for RestaurantTime
      public ICollection<RestaurantTime> restaurantTimes { get; set; } = new List<RestaurantTime>();
  }
  ```
- Now we also create another class named RestaurantTime.cs. This is child property we will save restaurant opening day and opening time and closing time also.
  ```
  public class RestaurantTime
  {
      [Key]     
      public string Id { get; set; }


      [Required]
      [MaxLength(15)]
      [Display(Name = "Opening Day")]
      public string OpeningDay { get; set; }

      [Required]
      [Display(Name = "Opening Time")]
      public TimeOnly OpeningTime { get; set; }

      [Required]
      [Display(Name = "Closing Time")]
      public TimeOnly ClosingTime { get; set; }

      //Foreign key referencing the Restaurant table as parent
      public string RestaurantId { get; set; }

      //Navigation property for Restaurant
      public Restaurant Restaurant { get; set; }
  }
  ```
- Create a folder name Interfaces and create an Interface class IRawDataParser.cs

  ```
  using RestaurantOpeningApi.DTOs;

  namespace RestaurantOpeningApi.Interfaces
  {
      public interface IRawDataParser
      {
          Task<IEnumerable<Restaurant>> ProcessCsvFileAsync(Stream fileStream);
      }
  }
  ```
- Create another interface IRestaurantService.cs

```
  public interface IRestaurantService
  {
      Task<List<Restaurant>> GetAllRestaurantAsync();
      Task AddRestaurantAsync(Restaurant restaurant);      
      Task AddListRestaurantAsync(List<Restaurant> restaurant);      
      void DeleteAsync(string id);
      Task SaveChangesAsync();

  }
```

- Create a folder Repository and implement this interface . Create implementing class RawDataParserService.cs

  ```
  using CsvHelper;
  using CsvHelper.Configuration;
  using RestaurantOpeningApi.DTOs;
  using RestaurantOpeningApi.Interfaces;
  
  namespace RestaurantOpeningApi.Repository
  {
   public class RawDataParserService : IRawDataParser
    {
       public async Task<IEnumerable<Restaurant>> ProcessCsvFileAsync(Stream fileStream)
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

    
  ```
- For IRestaurantService 
  ```
   public class RestaurantRepoService : IRestaurantService
   {
       RestaurantContext _context;
       public RestaurantRepoService(RestaurantContext restaurantContext)
       {
           _context = restaurantContext;
       }
  
       public async Task AddListRestaurantAsync(List<Restaurant> restaurant)
       {
           try
           {
             await  _context.Restaurants.AddRangeAsync(restaurant);
           }
           catch (Exception)
           {
               throw;
           }
       }
  
       public async Task AddRestaurantAsync(Restaurant restaurant)
       {
           try
           {
              await _context.Restaurants.AddAsync(restaurant);                
           }
           catch (Exception)
           {
               throw;
           }
       }
  
       public void DeleteAsync(string id)
       {
           var restaurant =  _context.Restaurants.Where(t=>t.Id == id).FirstOrDefault();
           if (restaurant != null)
           {
               _context.Restaurants.Remove(restaurant);
           }
       }
  
       public Task<List<Restaurant>> GetAllRestaurantAsync()
       {
           throw new NotImplementedException();
       }
  
       public async Task SaveChangesAsync()
       {
           await _context.SaveChangesAsync();
       }

   }
  ``` 

- Now we need to inject this service to our project in Program.cs file. Open the file and put this line of code.

  ```    
  builder.Services.AddScoped<IRawDataParser, RawDataParserService>();
  builder.Services.AddScoped<IRestaurantService, RestaurantRepoService>();
  ```
- Create a web api controller in Controllers folder named RestaurantDataController.cs to upload csv file

  ```
    using Microsoft.AspNetCore.Mvc;
    using RestaurantOpeningApi.Interfaces;
    using RestaurantOpeningApi.Models;
    
    namespace RestaurantOpeningApi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class RestaurantDataUploadController : ControllerBase
        {   
            private readonly IRawDataParser _dataService;
            private readonly IRestaurantService _restaurantService;
            public RestaurantDataUploadController(IRawDataParser dataService , IRestaurantService restaurantService)
            {
                _dataService = dataService;     
                _restaurantService = restaurantService;
            }
            
    
            [HttpPost("upload")]
            public async Task<IActionResult> UploadCsvFile(IFormFile file)
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }
    
                // Check the file extension
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (extension != ".csv")
                {
                    return BadRequest("Invalid file type. Only CSV files are allowed.");
                }
    
                using var fileStream = file.OpenReadStream();
                List<Restaurant> restaurants = await _dataService.ProcessCsvFileAsync(fileStream);
    
                if (restaurants.Count() > 0)
                {
                    // process this data and save to database                
    
                    try
                    {                  
                        await _restaurantService.AddListRestaurantAsync(restaurants);
                        await _restaurantService.SaveChangesAsync();
                        return StatusCode(StatusCodes.Status201Created, "Data uploaded successfully.");
                    }
                    catch (Exception)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Data uploaded failed.");
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, "File don't have any data.");
                }                  
            }
        }
    }

  ```
- Now we need to create a Database in azure cosmos DB . Though we want not to create in azure portal that is needed payment credit card info. We will use azure cosmos db emulator that 
  is as same as azure portal. After installing the emultor run this. 

  ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/9b1d0e6e-b36a-4bf4-ad68-37d6598a6ea2)

- Click Open Data Explorer and it open on a browser.
  ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/ec334235-d6d7-4c3f-91ab-ac093d3260cd)

- Hear URI is account end point and Primary Key is Account Key. Now lets create a db context class. To connect this database.Create a folder DataContext and class RestaurantContext.cs

   ```
    using Microsoft.EntityFrameworkCore;
    using RestaurantOpeningApi.Models;
    using Microsoft.EntityFrameworkCore.Cosmos;
   
    namespace RestaurantOpeningApi.DataContext
    {
        public class RestaurantContext : DbContext
        {
            public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
            {
            }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
               base.OnConfiguring(optionsBuilder);
               optionsBuilder.UseCosmos(
                    "https://localhost:8081",
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                    databaseName: "restaurant-db");
            }
            public DbSet<Restaurant> Restaurants { get; set; }
            public DbSet<RestaurantTime> RestaurantTimes { get; set; }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                
                // Set the table name for the Parent entity
                modelBuilder.Entity<Restaurant>()
                    .ToContainer("Restaurant")
                    .HasPartitionKey(e=>e.Id); // Specify the table name
    
                // Set the table name for the Child entity
                modelBuilder.Entity<RestaurantTime>()
                    .ToContainer("RestaurantTime")
                    .HasPartitionKey(e=>e.Id); // Specify the table name
    
                modelBuilder.Entity<Restaurant>()
                                 .HasMany(p => p.restaurantTimes)
                                 .WithOne(c => c.Restaurant)
                                 .HasForeignKey(c => c.RestaurantId);    
               
            }
        }
    }

  ```
- Database connection string is here
  ```
  optionsBuilder.UseCosmos(
                    "https://localhost:8081",
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                    databaseName: "restaurant-db");
  ```
- Now create the database in cosmos db and create two table. In cosmos db table is container. Click Explorer , give Database Id name as database name ,Container Id is table name and 
  Partition Id is Primary Key name ID
  
  ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/4f309548-b236-42da-aa27-541176a647a4)

- Same approch create another container RestaurantTime. We only create the container no table field as like sql table schema . This is Schemaless. 
  
  ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/5931529c-eadd-4d96-a9c4-98f4696a7ceb)
  
- Run the application and check in swagger Ui . Hear is an upload option to upload file from swagger. Select the csv file and Execute.

   ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/85776dbe-fe69-44a1-95a7-ec84bae1b9be)

- Swagger will return status code. Check the status code. 

- Now open cosmos explorer and check the data are saved.
  ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/3ee38bf9-a55f-424f-8abc-03c8c0fd6c5b)

## Parsing OperatingTime

- We need to do parsing OperatingTime and insert into child table RestaurantTime.So we need to refactor our avobe code after parsing operating time.We are now create an parser algorithm 
  using regular expression.
- Create a class CommonManagement.cs under Common folder. And create a method ParseOperatingDayAndTime.

   ```
  public static Dictionary<string, string> ParseOperatingDayAndTime(string schedule)
    {
        // Initialize a dictionary to hold the parsed data
        var scheduleDict = new Dictionary<string, string>();

        // Regular expression to match day ranges and time ranges
        string pattern = @"((?:[A-Za-z]+(?:[,\s*-][A-Za-z]+)?(?:,\s*)?)+)\s+(\d{1,2}\s+[ap]m\s+-\s+\d{1,2}:\d{2}\s+[ap]m)";
  
        // Find matches in the schedule string
        var matches = Regex.Matches(schedule, pattern);

        if (matches.Count() != 0)
        {
            foreach (Match match in matches)
            {
                // Extract the day and time range from the match
                string days = match.Groups[1].Value.Trim();
                string timeRange = match.Groups[2].Value.Trim();

                // Split the days part by commas to handle multiple day ranges
                var dayRanges = days.Split(',');

                foreach (var dayRange in dayRanges)
                {
                    // Trim the day range and add it to the dictionary
                    string trimmedDayRange = dayRange.Trim();
                    scheduleDict[trimmedDayRange] = timeRange;
                }
            }
        }
        else
        {
            scheduleDict[schedule] = schedule;
        }
        

        return scheduleDict;
    }
  ```
## Unit Testing 

- Create a xUnit test project named RestaurantOpeningApi.Test
- Create Data upload Service test under folder ServiceTest
- Add reference from RestaurantOpeningApi and install xUnit test , moq .
  
  ```
    using RestaurantOpeningApi.Repository;
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

  ```
- Run the test cases.
  ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/846e1847-a5a4-4394-8b8d-4ad962757424)

  

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
  
# Basic structure

 We now create some folder on app folder here. This is basic structure we are following .
     - components       // Reusable UI components
     - pages            // Components representing entire pages/views
     - services         // Services for data manipulation, API calls, etc.
     - models           // Data models/interfaces
     - guards           // Route guards for authentication, authorization, etc.
     - interceptors     // HTTP interceptors for global error handling, logging, etc.
     - utils            // Utility functions
     - shared           // Shared modules, directives, pipes, etc.
     - core             // Core module (singleton services, app-wide imports, etc.)
     - layouts          // Layout components (header, footer, sidebar, etc.)
     - assets           // Static assets like images, fonts, etc.
     - styles           // Global styles, CSS, SCSS files
     
## References

- https://github.com/Azure-Samples/cosmos-dotnet-core-todo-app
- https://github.com/Azure/azure-cosmos-dotnet-v3
- https://www.c-sharpcorner.com/article/angular-app-with-asp-net-core-and-cosmos-db/
- https://www.codeproject.com/Articles/1256191/Angular-6-Application-with-Cosmos-DB-and-Web-API-2
- https://www.c-sharpcorner.com/article/bulk-operations-in-entity-framework-core/
