using Api.DataContext;
using Api.Repositories;
using Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using Microsoft.AspNetCore.HttpsPolicy;

var builder = WebApplication.CreateBuilder(args);

// Configure development certificate
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.UseKestrel(options =>
    {
        options.ListenAnyIP(5220); // HTTP port
        options.ListenAnyIP(7037, listenOptions =>
        {
            listenOptions.UseHttps(httpsOptions =>
            {
                httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;
                // Desactivar la verificación del cliente para desarrollo
                httpsOptions.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.NoCertificate;
            });
        });
    });

    // Agregar política de seguridad para desarrollo
    builder.Services.AddHsts(options =>
    {
        options.MaxAge = TimeSpan.FromDays(1);
        options.IncludeSubDomains = false;
    });
}

//Configurarar autorizacionces
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("ADMIN"));
});
// Configurar JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
// Configurar PostgreSQL
builder.Services.AddDbContext<AmadeusContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositorios y servicios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>((provider) => {
    var usuarioRepository = provider.GetRequiredService<IUsuarioRepository>();
    var destinoRepository = provider.GetRequiredService<IDestinoRepository>();
    var context = provider.GetRequiredService<AmadeusContext>();
    return new UsuarioService(usuarioRepository, destinoRepository, context);
});
builder.Services.AddScoped<IPreferenciaUsuarioRepository, PreferenciaUsuarioRepository>();
builder.Services.AddScoped<IPreferenciaUsuarioService, PreferenciaUsuarioService>();
builder.Services.AddScoped<IPreferenciaRepository, PreferenciaRepository>();
builder.Services.AddScoped<IPreferenciaService, PreferenciaService>();
builder.Services.AddScoped<IDestinoRepository, DestinoRepository>();
builder.Services.AddScoped<IDestinoService, DestinoService>();
builder.Services.AddScoped<IAuthService, AuthService>();


// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        // Leer los orígenes permitidos desde la configuración
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
        
        if (allowedOrigins != null && allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials()
                  .WithExposedHeaders("Authorization", "Content-Type");
        }
        else
        {
            // Configuración por defecto si no hay orígenes configurados
            policy.WithOrigins("http://localhost:3000", "https://localhost:3000", 
                               "http://localhost:5173", "https://localhost:5173",
                               "http://127.0.0.1:3000", "https://127.0.0.1:3000",
                               "http://127.0.0.1:5173", "https://127.0.0.1:5173")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials()
                  .SetIsOriginAllowed(_ => true) // Para desarrollo
                  .WithExposedHeaders("Authorization", "Content-Type");
        }
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

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese el token de acceso con el prefijo 'Bearer'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

// Middleware para CORS debe ir antes de UseRouting y otros middleware
app.UseCors("AllowAll");

// Después de CORS
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Habilitar Swagger en todos los entornos
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    options.RoutePrefix = string.Empty; // Dejar vacío para acceder en la raíz (http://localhost:5220)
});

// Manejo global de excepciones
app.UseExceptionHandler("/error");

app.MapControllers();
app.Run();
