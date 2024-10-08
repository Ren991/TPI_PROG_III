using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using static Infrastructure.Services.AuthenticationService;
using System.Text;
using System.Text.Json.Serialization;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers()

    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        // Prevent reference cycles
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Convert Enums
    });

builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("miradaPerfecta", new OpenApiSecurityScheme() //Esto va a permitir usar swagger con el token.
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Acá pegar el token generado al loguearse."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "miradaPerfecta" } //Tiene que coincidir con el id seteado arriba en la definición
                }, new List<string>() }
    });

});

string connectionString = builder.Configuration["ConnectionStrings:MiradaPerfectaDBConnectionString"]!;

// Configure the SQLite connection
var connection = new SqliteConnection("Data Source= miradaPerfecta.db");
connection.Open();

builder.Services.AddDbContext<ApplicationDbContext>(dbContextOptions => dbContextOptions.UseSqlite(connection));

builder.Services.Configure<AuthenticationServiceOptions>(
    builder.Configuration.GetSection(AuthenticationServiceOptions.AuthenticationService));

builder.Services.AddAuthentication("Bearer") //"Bearer" es el tipo de auntenticación que tenemos que elegir después en PostMan para pasarle el token
    .AddJwtBearer(options => //Acá definimos la configuración de la autenticación. le decimos qué cosas queremos comprobar. La fecha de expiración se valida por defecto.
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AuthenticacionService:Issuer"],
            ValidAudience = builder.Configuration["AuthenticacionService:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AuthenticacionService:SecretForKey"]))
        };
    }
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>(); // Assuming UserService exists

builder.Services.AddScoped<IProductRepository, ProductRepository>(); // Assuming ProductRepository exists
builder.Services.AddScoped<IProductService, ProductService>(); // Assuming ProductService exists
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

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
