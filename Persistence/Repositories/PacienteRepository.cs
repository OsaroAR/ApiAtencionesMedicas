using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Application.Common.Interfaces;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Persistence.Repositories;

public class PacienteRepository(IDbConnectionFactory factory)
{
  private readonly IDbConnectionFactory _factory = factory;

  public async Task<int> CreateAsync(Patient p, string createdBy)
  {
    using var conn = _factory.CreateConnection();
    var prm = new DynamicParameters();
    prm.Add("@FirstName", p.FirstName);
    prm.Add("@LastName", p.LastName);
    prm.Add("@RUT", p.RUT);
    prm.Add("@DateOfBirth", p.DateOfBirth);
    prm.Add("@Gender", p.Gender);
    prm.Add("@Phone", p.Phone);
    prm.Add("@Email", p.Email);
    prm.Add("@AddressLine1", p.AddressLine1);
    prm.Add("@AddressLine2", p.AddressLine2);
    prm.Add("@City", p.City);
    prm.Add("@State", p.State);
    prm.Add("@PostalCode", p.PostalCode);
    prm.Add("@CreatedBy", createdBy);

    try
    {
      var newId = await conn.ExecuteScalarAsync<int>(
          "dbo.Patient_Insert", prm, commandType: CommandType.StoredProcedure);
      return newId;
    }
    catch (SqlException ex) when (ex.Number is 2601 or 2627) // RUT duplicado
    {
      throw new InvalidOperationException("RUT ya existe.", ex);
    }
  }

  public async Task<int> UpdateAsync(Patient p, string modifiedBy)
  {
    using var conn = _factory.CreateConnection();
    var prm = new DynamicParameters();
    prm.Add("@Id", p.PatientId);
    prm.Add("@FirstName", p.FirstName);
    prm.Add("@LastName", p.LastName);
    prm.Add("@RUT", p.RUT);
    prm.Add("@DateOfBirth", p.DateOfBirth);
    prm.Add("@Gender", p.Gender);
    prm.Add("@Phone", p.Phone);
    prm.Add("@Email", p.Email);
    prm.Add("@AddressLine1", p.AddressLine1);
    prm.Add("@AddressLine2", p.AddressLine2);
    prm.Add("@City", p.City);
    prm.Add("@State", p.State);
    prm.Add("@PostalCode", p.PostalCode);
    prm.Add("@ModifiedBy", modifiedBy);

    try
    {
      // Si el SP lanza RAISERROR por RUT duplicado o no encontrado,
      // cae en SqlException y lo manejamos arriba.
      await conn.ExecuteAsync("dbo.Patient_Update", prm, commandType: CommandType.StoredProcedure);
      return 1;
    }
    catch (SqlException ex) when (ex.Number is 2601 or 2627)
    {
      throw new InvalidOperationException("RUT ya existe.", ex);
    }
  }

  public async Task<int> DeleteAsync(int id)
  {
    using var conn = _factory.CreateConnection();
    var affected = await conn.ExecuteScalarAsync<int>(
        "dbo.Patient_Delete",
        new { PatientId = id },
        commandType: CommandType.StoredProcedure);
    return affected; // 1 si borró, 0 si no
  }

  public async Task<Patient?> GetByIdAsync(int id)
  {
    using var conn = _factory.CreateConnection();
    return await conn.QueryFirstOrDefaultAsync<Patient>(
        "dbo.Patient_GetById",
        new { PatientId = id },
        commandType: CommandType.StoredProcedure);
  }

  public async Task<IEnumerable<Patient>> SearchAsync(string? rut, string? name, int? minAge, int? maxAge)
  {
    using var conn = _factory.CreateConnection();
    return await conn.QueryAsync<Patient>(
        "dbo.Patient_Search",
        new { RUT = rut, Name = name, MinAge = minAge, MaxAge = maxAge },
        commandType: CommandType.StoredProcedure);
  }
}
