using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Application.Common.Interfaces;

namespace Persistence.Infrastructure;

public class SqlConnectionFactory : IDbConnectionFactory
{
  private readonly string _connectionString;

  public SqlConnectionFactory(string connectionString)
  {
    _connectionString = connectionString;
  }

  public IDbConnection CreateConnection()
  {
    return new SqlConnection(_connectionString);
  }
}