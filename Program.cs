using Api.DataContext;
using Api.Repositories;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configurar PostgreSQL
builder.Services.AddDbContext<AmadeusContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositorios y servicios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPreferenciaUsuarioRepository, PreferenciaUsuarioRepository>();
builder.Services.AddScoped<IPreferenciaUsuarioService, PreferenciaUsuarioService>();
builder.Services.AddScoped<IPreferenciaRepository, PreferenciaRepository>();
builder.Services.AddScoped<IPreferenciaService, PreferenciaService>();
builder.Services.AddScoped<IDestinoRepository, DestinoRepository>();
builder.Services.AddScoped<IDestinoService, DestinoService>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Amadeus API",
        Version = "v1",
        Description = "API para la gestión de usuarios y preferencias en Amadeus."
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Habilitar Swagger en todos los entornos
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    options.RoutePrefix = string.Empty; // Dejar vacío para acceder en la raíz (http://localhost:5220)
});

// Manejo global de excepciones
app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
