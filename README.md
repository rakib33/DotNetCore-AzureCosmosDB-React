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
        public TimeSpan OpeningTime { get; set; }

        [Required]
        [Display(Name = "Closing Time")]
        public TimeSpan ClosingTime { get; set; }
        public TimeSpan TotalTimeDuration { get; set; }

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
        Task<List<Restaurant>> ProcessCsvFileAsync(Stream fileStream);
        Task<List<RestaurantTime>> ParseRestaurantOperatingTime(string operatingTime, string restaurantId);
    }
  }
  ```
- Create another interface IRestaurantService.cs

```
 public interface IRestaurantService
    {
        Task<List<Restaurant>> GetAllRestaurantAsync(RestaurantParameters restaurantParameters);      
        Task AddBulkRestaurantAsync(List<Restaurant> restaurant);      
        void DeleteAsync(string id);
        Task SaveChangesAsync();
    }
```

- Create a folder Repository and implement this interface . Create implementing class RawDataParserService.cs

  ```
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
              
                  TimeSpan closingTime = CommonManagement.SetClosingTime(item.OpeningTime, item.ClosingTime);
                 
                  var restaurantTime = new RestaurantTime
                  {
                      Id = Guid.NewGuid().ToString(),
                      RestaurantId = restaurantId,
                      OpeningDay = item.OpeningDay,
                      OpeningTime = item.OpeningTime,
                      ClosingTime = closingTime,
                      TotalTimeDuration = totalTime
                  };
  
                  restaurantTimes.Add(restaurantTime);
              }
  
              return restaurantTimes;
          }
  
      }
  }    
  ```

- For RestaurantRepoService  that implememnt IRestaurantService

  ```
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Azure.Cosmos;
  using Microsoft.EntityFrameworkCore;
  using RestaurantOpeningApi.Common;
  using RestaurantOpeningApi.DataContext;
  using RestaurantOpeningApi.Interfaces;
  using RestaurantOpeningApi.Models;
  
  
  namespace RestaurantOpeningApi.Services
  {
      public class RestaurantRepoService : IRestaurantService
      {
          RestaurantContext _context;
          public RestaurantRepoService(RestaurantContext restaurantContext)
          {
              _context = restaurantContext;
          }
  
          public async Task AddBulkRestaurantAsync(List<Restaurant> restaurant)
          {         
              await  _context.Restaurants.AddRangeAsync(restaurant);              
          }
  
          public async Task AddRestaurantAsync(Restaurant restaurant)
          {
              try
              {
                  //disable change tracker
                  _context.ChangeTracker.AutoDetectChangesEnabled = false;
                  //new entities and  don't exist in the database , So apply AsNoTracking to prevent EF Core from tracking them.This improve speed.
                  _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
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
  
          
          public async Task<List<Restaurant>> GetAllRestaurantAsync(RestaurantParameters p)
          {
  
             var query =  _context.Restaurants.AsQueryable();
              
              //Apply filtering 
              if (!string.IsNullOrEmpty(p.name))
                  query = query.Where(s => s.Name.Contains(p.name));
             
              List<Restaurant> restaurants = await query.ToListAsync();
              //Explicit Loading
              foreach (var restaurant in restaurants)
              {
                  if (!string.IsNullOrEmpty(p.day))
                      await _context.Entry(restaurant).Collection(s => s.restaurantTimes.Where(c=>c.OpeningDay.Contains(p.day))).LoadAsync();
  
                  if (p.time != null)
                      await _context.Entry(restaurant).Collection(s => s.restaurantTimes.Where(c => p.time <= c.ClosingTime && p.time >= c.OpeningTime)).LoadAsync();
                  else
                      await _context.Entry(restaurant).Collection(p=>p.restaurantTimes).LoadAsync();
              }
  
              return restaurants.Skip((p.Pagination.Page -1) * p.Pagination.PageSize).Take(p.Pagination.PageSize).ToList();
  
          }
  
          public async Task SaveChangesAsync()
          {
              await _context.SaveChangesAsync();
          }
  
     
      }
  }
  
  ``` 
- Create CommonManagement.cs class from Common folder. This is importent class. we parse complex operating time string here also some common method used overall the project.

  ```
  using Microsoft.Azure.Cosmos.Spatial;
  using RestaurantOpeningApi.DTOs;
  using RestaurantOpeningApi.Models;
  using System;
  using System.Text.RegularExpressions;
  
  namespace RestaurantOpeningApi.Common
  {
      public class CommonManagement
      {
         
          // Function to convert three-character day names to DayOfWeek enum
          static DayOfWeek GetDayOfWeek(string day)
          {
              switch (day.ToLower())
              {
                  case "sun": return DayOfWeek.Sunday;
                  case "mon": return DayOfWeek.Monday;
                  case "tue": return DayOfWeek.Tuesday;
                  case "tues": return DayOfWeek.Thursday;
                  case "wed": return DayOfWeek.Wednesday;
                  case "weds": return DayOfWeek.Wednesday;
                  case "thu": return DayOfWeek.Thursday;
                  case "thurs": return DayOfWeek.Thursday;                
                  case "fri": return DayOfWeek.Friday;
                  case "sat": return DayOfWeek.Saturday;
                  default: throw new ArgumentException("Invalid day name");
              }
          }
          static List<string> GetDayRange(string startDay, string endDay)
          {
  
                  // Get the DayOfWeek enum representation of start and end days
                  DayOfWeek startDayOfWeek = GetDayOfWeek(startDay);
                  DayOfWeek endDayOfWeek = GetDayOfWeek(endDay);
  
                  List<string> days = new List<string>();
  
                  // Loop through the days and print the names
                  for (int i = (int)startDayOfWeek; i % 7 != (int)endDayOfWeek; i++)
                  {
                      days.Add(((DayOfWeek)(i % 7)).ToString());
                  }
                  days.Add(endDayOfWeek.ToString());
  
                  return days;
           
          }
          public  static TimeSpan ParseTime(string timeString)
          {
              // Define custom time formats
              string[] formats = { "h tt", "h:mmtt", "htt", "hhtt" }; // For example: "11 am", "11:00am", "11am", "11am"
  
              // Parse the time string using custom formats
              if (DateTime.TryParseExact(timeString, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out     DateTime parsedTime))
              {
                  return parsedTime.TimeOfDay;
              }
              else
              {
                  throw new ArgumentException("Invalid time format");
              }
          }
          public   static List<RestaurantTimeDTO> ParseRestaurantTimeString(string operatingTimeString)
          {
  
              string[] splitbySlash = operatingTimeString.Split('/');
              List<string> operatingTime = new List<string>(splitbySlash);
  
  
              List<RestaurantTimeDTO> restaurantTime = new List<RestaurantTimeDTO>();
  
              foreach (var item in operatingTime)
              {
  
                  string itm = Regex.Replace(item, @"\s+", "");
  
                  char[] numberArr = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                  int index = itm.IndexOfAny(numberArr);
                  string dayPart = itm.Substring(0, index);
                  string timePart = itm[index..itm.Length];
  
                  var days = dayPart.Split(',');
  
                  //Time part 
                  var splitTime = timePart.Split("-");
                  TimeSpan startTime = ParseTime(splitTime[0]);
                  TimeSpan endTime = ParseTime(splitTime[1]);
             
                  try
                  {
                      foreach (var day in days)
                          if (day.Contains("-"))
                          {
                              var dayRange = day.Split('-');
  
                              List<string> getDaysRange = GetDayRange(dayRange[0], dayRange[1]);
  
                              foreach (var dayname in getDaysRange)
                              {
                                  restaurantTime.Add(new RestaurantTimeDTO
                                  {
                                      OpeningDay = dayname,
                                      OpeningTime = startTime,
                                      ClosingTime = endTime,
                                  });
                              }
                          }
                          else
                              restaurantTime.Add(new RestaurantTimeDTO
                              {
                                  OpeningDay = day,
                                  OpeningTime = startTime,
                                  ClosingTime = endTime,
                              });
                  }catch (Exception ex)
                  {
                      string msg = ex.Message;
                  }
              }
  
              return restaurantTime;
  
          }
  
          public static TimeSpan SetClosingTime(TimeSpan openTime, TimeSpan closingTime)
          {
              if (closingTime < openTime)
              {
                  // Add 24 hours to the end time to make it on the same day
                  closingTime = closingTime.Add(new TimeSpan(24, 0, 0));
              }
  
              return closingTime;
          }
          public static TimeSpan GetTotalTimeDuration(TimeSpan openTime, TimeSpan closingTime)
          {
              TimeSpan duration;
              //// If the end time is before the start time, it means it's on the next day
              if (closingTime < openTime)
              {
                  // Add 24 hours to the end time to make it on the same day
                  closingTime = closingTime.Add(new TimeSpan(24, 0, 0));
                  // Calculate the duration between the times
                  duration = closingTime - openTime;
              }
              else
              {
                  duration = closingTime - openTime;
              }
  
              return duration;
              
          }
  
          public static bool IsTimeOnDuration(TimeSpan time, TimeSpan openTime, TimeSpan closingTime)
          {
              TimeSpan duration;
              //// If the end time is before the start time, it means it's on the next day
              if (closingTime < openTime)           
                  // Add 24 hours to the end time to make it on the same day
                  closingTime = closingTime.Add(new TimeSpan(24, 0, 0));             
              if(time < openTime)
                  time = time.Add(new TimeSpan(24, 0, 0));
  
              if (time > openTime && time < closingTime)
                  return true;
              else return false;
          }
  
          public static TimeSpan GetTimeSpanFromString(string time)
          {
              // Parsing a TimeSpan from a string representation
              TimeSpan span;
             var parseTime =  TimeSpan.TryParse(time,out span);
              return span;
          }
      }
      }   

  ```

- Now we need to inject this service to our project in Program.cs file. Open the file and put this line of code.

  ```    
  // Add services to the container.
  builder.Services.AddControllers();
  
  builder.Services.AddScoped<IRawDataParser, RawDataParserService>();
  builder.Services.AddScoped<IRestaurantService, RestaurantRepoService>();
  builder.Services.AddScoped<IRestaurantTimeService, RestaurentTimeRepoService>();
  builder.Services.AddScoped<IRestaurantDataService, RestaurantDataService>();
  ```
- Enable cors for this project. Add those in program.cs file.
  ```
  builder.Services.AddCors(options =>
  {
      options.AddPolicy("AllowAngularOrigins",
      builder =>
      {
          builder.WithOrigins(
                              "http://localhost:4200"
                              )
                              .AllowAnyHeader()
                              .AllowAnyMethod();
      });
  });

  ....
  ....

  builder.Services.AddCors();
  var app = builder.Build();
  ....
  ....
  app.UseCors("AllowAngularOrigins");
  
  app.UseHttpsRedirection();
  app.UseRouting();
  app.UseAuthorization();
  app.MapControllers();
  app.Run();
  ```
- Create a service RestaurantDataService under RestaurantRawDataService.cs file from Services folder. We will save our restaurant data from csv file after parsing.

  ```
  using RestaurantOpeningApi.Common;
  using RestaurantOpeningApi.Interfaces;
  using RestaurantOpeningApi.Models;
  
  namespace RestaurantOpeningApi.Services
  {
      public class RestaurantDataService : IRestaurantDataService
      {
        
          private readonly IRestaurantService _restaurantService;
          private readonly IRestaurantTimeService _restaurantTimeService;
          private DateTime Start;
          private TimeSpan TimeSpan;
  
          public RestaurantDataService( IRestaurantService restaurantService, IRestaurantTimeService restaurantTimeService)
          {   
              _restaurantService = restaurantService;          
              _restaurantTimeService = restaurantTimeService;
          }
  
          public async Task<TimeSpan> AddRestaurantBatchAsync(List<Restaurant> restaurants, int batchSize)
          {
              Start = DateTime.Now;
  
              for (int i = 0; i < restaurants.Count; i += batchSize)
              {
                  List<Restaurant> batch = restaurants.Skip(i).Take(batchSize).ToList();
                  await _restaurantService.AddBulkRestaurantAsync(batch);
                  await _restaurantService.SaveChangesAsync();
              }
              
              TimeSpan = DateTime.Now - Start;
              return TimeSpan;
          }
  
          public async Task<List<Restaurant>> GetRestaurantAsync(RestaurantParameters restaurantParameters)
          {
              return await _restaurantService.GetAllRestaurantAsync(restaurantParameters);
          }
      }
  }

  ``` 
   
- Now Create a web api controller in Controllers folder named RestaurantDataController.cs to upload csv file data on cosmos database using RestaurantDataService.
  
  ```
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using RestaurantOpeningApi.Common;
  using RestaurantOpeningApi.Interfaces;
  using RestaurantOpeningApi.Models;
  
  namespace RestaurantOpeningApi.Controllers
  {
      [Route("api/[controller]")]
      [ApiController]
      public class RestaurantDataUploadController : ControllerBase
      {   
          private readonly IRawDataParser _dataService;
          private readonly IRestaurantDataService _restaurantService;
          public RestaurantDataUploadController(IRawDataParser dataService , IRestaurantDataService restaurantService)
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
                      await _restaurantService.AddRestaurantBatchAsync(restaurants,100);   
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
  
          [HttpGet("GetRestaurants")]
          [ProducesResponseType(typeof(List<Restaurant>), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status404NotFound)]
          public async Task<IActionResult> GetRestaurants(string name,string day,string time, int page = 1, int pageSize = 50)
          {
              RestaurantParameters parms = new RestaurantParameters();
              Pagination pagination = new Pagination();
              pagination.Page = 1;
              pagination.PageSize = 50;
              parms.name = name;
              parms.day = day;
              parms.time = CommonManagement.GetTimeSpanFromString(time);
              parms.Pagination = pagination;
          
              var items = await _restaurantService.GetRestaurantAsync(parms);
              return Ok( items);
  
          }
        
      }
  }

  ```
- Now we need to create a Database in azure cosmos DB . Though we want not to create in azure portal that is needed payment credit card info. We will use azure 
  cosmos db emulator that 
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

## Test Coverage

- Add  Test coverage.We are using a free tools for test coverage of this application. This test report shows us how many code are testable , how many code are passed 
  the test or failed or not covered .

  1. Download [(Fine Code Coverage)https://marketplace.visualstudio.com/items?itemName=FortuneNgwenya.FineCodeCoverage2022]

  2. Install FineCodeCoverage2022.vsix

      ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/f9520120-1cf3-4f41-b6d0-b34b2f03ba4d)

  3. Also can download from extension. Check this article https://codesloth.blog/visual-studio-code-coverage-with-fine-code-coverage-visual-studio-2022/

  4. Run the test case.

     ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/cfaaca9e-7de2-4351-8c1b-51dddce913fd)

  6. Now from the bottom click Fine Code Coverage button. Covergae indicates the test coverage percentage.

    ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/eecfccd7-7035-45f1-9adb-0c92d9df5661)

  7. We can check how many code is covered in summary section.

     ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/d462482c-1491-4e49-925c-66234c1c01b9)

  9. We can check the risk factor.

     ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/addf4232-f43e-4500-aa5a-e2c3f8f30b95)

  11. Coverage log

      ![image](https://github.com/rakib33/rakibul-islam-backend-test-21April2024/assets/10026710/057a9625-6b99-4d60-b0e9-53a193964c75)
 


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

 - In app.component.ts call the api now. We will create service for api calling lather.

  ```
  import { HttpClient ,HttpClientModule } from '@angular/common/http';
  import { Component, OnInit } from '@angular/core';
  import { RouterOutlet } from '@angular/router';
  import { error } from 'console';
  import { response } from 'express';
  
  @Component({
    selector: 'app-root',
    standalone: true,
    imports: [RouterOutlet],
    templateUrl: './app.component.html',
    styleUrl: './app.component.css'
  })
  export class AppComponent implements OnInit{
    title = 'Restaurant App';
    restaurants: any;
    
    constructor(private http: HttpClient){}
    ngOnInit(): void {
     //this.getRestaurants();
     this.getAllRestaurants();
    }
  
    getAllRestaurants(): void {
  
      // Make the HTTP GET request
      //?name=${this.restaurantName}&day=${this.day}&time=${this.time}&page=${this.currentPage}&pageSize=${this.ItemPerPage}
      this.http.get<any>("https://localhost:7222/api/RestaurantDataUpload/GetRestaurants?name=''&day=''&time=''&page=1&pageSize=50")
        .subscribe(
          (response) => {
            // Handle the response data
            console.log('response :'+response);
            this.restaurants = response.data;
            console.log('response data :'+ this.restaurants);
          },
          (error) => {
            // Handle errors
            console.error('Error fetching restaurants:', error);
          }
        );
    }
  
  }

  ```

 - Add HttpClient provider on app.config.ts file

  ```
    import { ApplicationConfig } from '@angular/core';
    import { provideRouter } from '@angular/router';
    
    import { routes } from './app.routes';
    import { provideClientHydration } from '@angular/platform-browser';
    import { HttpClientModule, provideHttpClient, withFetch } from '@angular/common/http';
    
    export const appConfig: ApplicationConfig = {
      providers: [provideRouter(routes), provideClientHydration(),provideHttpClient(withFetch()),HttpClientModule]
    };

 ```
## ToDo Features
 
 - Add filtering option on UI data table. Filter by name, day and given time span. Add ordering features on data table.
 - Add pagination on UI.
 - Cover the pending testcase.
   
## References

- https://github.com/Azure-Samples/cosmos-dotnet-core-todo-app
- https://github.com/Azure/azure-cosmos-dotnet-v3
- https://www.c-sharpcorner.com/article/angular-app-with-asp-net-core-and-cosmos-db/
- https://www.codeproject.com/Articles/1256191/Angular-6-Application-with-Cosmos-DB-and-Web-API-2
- https://www.c-sharpcorner.com/article/bulk-operations-in-entity-framework-core/
- https://codesloth.blog/visual-studio-code-coverage-with-fine-code-coverage-visual-studio-2022/
-https://stackoverflow.com/questions/54219742/mocking-ef-core-dbcontext-and-dbset
