using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;


namespace Infrastructure.Security;

public static class MiddlewareExtensions
{
  public static IApplicationBuilder UseApiKeyAuthentication(this IApplicationBuilder app)
        => app.UseMiddleware<ApiKeyMiddleware>();
}
