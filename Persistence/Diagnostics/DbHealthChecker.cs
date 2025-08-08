using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Application.Common.Interfaces;
using Dapper;

namespace Persistence.Diagnostics;

public class DbHealthChecker(IDbConnectionFactory connectionFactory)
{
  private readonly IDbConnectionFactory _factory = connectionFactory;

  public async Task<(bool ok, string? error, string? server, string? database)> PingAsync()
  {
    try
    {
      using var conn = _factory.CreateConnection();

      var server = await conn.ExecuteScalarAsync<string>("SELECT @@VERSION");
      var database = await conn.ExecuteScalarAsync<string>("SELECT DB_NAME();");

      return (true, null, server, database);
    }
    catch (Exception ex)
    {
      return (false, ex.Message, null, null);
    }
  }

}
