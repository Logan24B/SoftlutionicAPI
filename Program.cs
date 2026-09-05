using Microsoft.EntityFrameworkCore;
using SoftlutionicAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Conexión a Azure SQL mediante Entity Framework
builder.Services.AddDbContext<SoftlutionicDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSQL")));

// CORS: permite que la Static Web App llame a la API desde otro dominio
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirSoftlutionic", policy =>
        policy.WithOrigins("https://happy-field-061ad4010.7.azurestaticapps.net")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

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
