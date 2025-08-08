using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security;

public class ApiKeyMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ApiKeySettings _settings;

  public ApiKeyMiddleware(RequestDelegate next, IOptions<ApiKeySettings> options)
  {
    _next = next;
    _settings = options.Value;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    // Permitir Swagger
    if (!_settings.EnableForSwagger && context.Request.Path.StartsWithSegments("/swagger"))
    {
      await _next(context);
      return;
    }

    // Validar header presente

    if (!context.Request.Headers.TryGetValue(_settings.HeaderName, out var providedKey) || string.IsNullOrWhiteSpace(providedKey))
    {
      context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
      await context.Response.WriteAsJsonAsync(new { error = $"Missing '{_settings.HeaderName}' header" });
      return;
    }
    
    // Validar API Key
    var isValid = _settings.ApiKeys.Any(k => string.Equals(k, providedKey.ToString(), StringComparison.Ordinal));
    if (!isValid)
    {
      context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
      await context.Response.WriteAsJsonAsync(new { error = "Invalid API Key" });
      return;
    }
    await _next(context);
  }
}
