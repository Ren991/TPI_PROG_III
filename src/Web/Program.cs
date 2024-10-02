using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Evitaa ciclos de referencias entre las entidades.
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Convierte Los ENUM. El de TypePayment y el de Role.
    });
string connectionString = builder.Configuration["ConnectionStrings:MiradaPerfectaDBConnectionString"]!;

// Configure the SQLite connection
var connection = new SqliteConnection("Data Source= miradaPerfecta.db");
connection.Open();

builder.Services.AddDbContext<ApplicationDbContext>(dbContextOptions => dbContextOptions.UseSqlite(connection));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
