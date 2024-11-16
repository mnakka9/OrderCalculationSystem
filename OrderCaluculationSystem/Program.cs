using OCS.Infrastructure.Services;
using OrderCalculationSystem.Apis.Orders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddRepositories();
builder.Services.AddRequiredServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(opt =>
    {
        opt.Theme = ScalarTheme.Kepler;
        opt.Title = "Tax System API";
        opt.DarkMode = false;
    });
}

app.UseHttpsRedirection();

// Uncomment to seed the data to test API's
//TaxSystemContext taxSystemContext = new TaxSystemContext();
//taxSystemContext.Database.EnsureCreated();

//foreach (var num in Enumerable.Range(1, 4))
//{
//    await SeedData.SeedDataAsync(taxSystemContext, num, num + 7);
//}

// Minimal API adding order calculation API and create order API
app.MapOrderApis();

app.Run();
