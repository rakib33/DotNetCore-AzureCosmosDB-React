using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RestaurantOpeningApi.DataContext;
using RestaurantOpeningApi.DTOs;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Repository;
using RestaurantOpeningApi.Services;
using System.Configuration;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);



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

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddResponseCaching();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Configure database context
builder.Services.AddDbContext<RestaurantContext>(options =>
    options.UseCosmos(
         "https://localhost:8081",
          "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
          databaseName: "restaurant-db"
        )
   );

builder.Services.AddScoped<IRawDataParser, RawDataParserService>();
builder.Services.AddScoped<IRestaurantService, RestaurantRepoService>();
builder.Services.AddScoped<IRestaurantTimeService, RestaurentTimeRepoService>();
builder.Services.AddScoped<IRestaurantDataService, RestaurantDataService>();


//using IOptions pattern
builder.Services.Configure<CosmosDbOptions>(builder.Configuration.GetSection("CosmosDb"));
//possible object cycle avoiding 
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
builder.Services.AddCors();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAngularOrigins");

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
