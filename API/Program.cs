using Application.Common.Interfaces;
using Persistence.Infrastructure;
using Microsoft.OpenApi.Models;
using Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<Persistence.Diagnostics.DbHealthChecker>();
builder.Services.AddScoped<Persistence.Repositories.PacienteRepository>();
builder.Services.AddScoped<Persistence.Repositories.DoctorRepository>();


// Swagger + ApiKey en UI
builder.Services.AddSwaggerGen(c =>
{
  var headerName = builder.Configuration["Security:HeaderName"] ?? "X-API-Key";
  var scheme = new OpenApiSecurityScheme
  {
    Name = headerName,
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Description = $"Ingrese su API Key en el header '{headerName}'",
    Scheme = "ApiKeyScheme"
  };
  c.AddSecurityDefinition("ApiKey", scheme);
  c.AddSecurityRequirement(new OpenApiSecurityRequirement { { scheme, new List<string>() } });
});

// Dapper connection factory
builder.Services.AddSingleton<IDbConnectionFactory>(sp =>
    new SqlConnectionFactory(builder.Configuration.GetConnectionString("DefaultConnection")!));

// ApiKey settings
builder.Services.Configure<ApiKeySettings>(builder.Configuration.GetSection("Security"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseApiKeyAuthentication();
app.MapControllers();
app.Run();
