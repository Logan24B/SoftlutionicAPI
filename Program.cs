using Microsoft.EntityFrameworkCore;
using SoftlutionicAPI.Data;
using SoftlutionicAPI.Configuration;
using SoftlutionicAPI.Services;
using MongoDB.Driver;
using MongoDB.Bson;



var builder = WebApplication.CreateBuilder(args);

// Conexión a Azure SQL mediante Entity Framework
builder.Services.AddDbContext<SoftlutionicDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AzureSQL")
    ));

// CORS: permite que la Static Web App llame a la API desde otro dominio
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirSoftlutionic", policy =>
        policy.WithOrigins("https://softlutionic-web-hchvbrbxc3eqarc2.westus-01.azurewebsites.net")
              .AllowAnyHeader()
              .AllowAnyMethod());
});
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDb"));

builder.Services.AddSingleton<MongoContactoService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger habilitado siempre para las evidencias de la actividad
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("PermitirSoftlutionic");
app.UseAuthorization();
app.MapControllers();

app.Run();

//Temporal
var mongoUri = builder.Configuration["MongoDb:ConnectionString"];

if (!string.IsNullOrWhiteSpace(mongoUri))
{
    try
    {
        var settings = MongoClientSettings.FromConnectionString(mongoUri);
        var client = new MongoClient(settings);

        var result = client
            .GetDatabase("admin")
            .RunCommand<BsonDocument>(
                new BsonDocument("ping", 1)
            );

        Console.WriteLine("✅ MongoDB Atlas conectado correctamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Error al conectar con MongoDB Atlas:");
        Console.WriteLine(ex.Message);
    }
}
else
{
    Console.WriteLine("❌ No se encontró MongoDb:ConnectionString.");
}
