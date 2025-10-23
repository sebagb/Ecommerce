using ProductService.Api;
using ProductService.Application;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["Database:ConnectionString"];
var databaseName = builder.Configuration["Database:Name"];

builder.Services.AddOpenApi();
builder.Services.AddApplication(connectionString!, databaseName!);

var app = builder.Build();

app.MapOpenApi();

app.UseHttpsRedirection();

app.RegisterProductEndpoints();

app.Run();