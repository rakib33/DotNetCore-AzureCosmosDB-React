
using Microsoft.EntityFrameworkCore;
using RestaurantOpeningApi.DataContext;
using RestaurantOpeningApi.Interfaces;
using RestaurantOpeningApi.Repository;
using RestaurantOpeningApi.Services;

var builder = WebApplication.CreateBuilder(args);

//Configure database context
builder.Services.AddDbContext<RestaurantContext>(options =>
    options.UseCosmos(
         "https://localhost:8081",
          "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
          databaseName: "restaurant-db"
        )
   );

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();               
        });
});



// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<IRawDataParser, RawDataParserService>();
builder.Services.AddScoped<IRestaurantService, RestaurantRepoService>();
builder.Services.AddScoped<IRestaurantRawDataService, RestaurantRawDataService>();

//builder.Services.AddScoped<IRestaurantTimeParser, RestaurantTimeParser>();
//builder.Services.AddScoped<IRestaurantTimeService, RestaurantTimeRepoService>();

//builder.Services.AddScoped<RestaurantDataService>();

builder.Services.AddResponseCaching();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    //app.UseExceptionHandler("/Error");
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowAllOrigins"); 
app.UseAuthorization();
app.MapControllers();
app.Run();
