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

public class DoctorRepository(IDbConnectionFactory factory)
{
  private readonly IDbConnectionFactory _factory = factory;

  // INSERT -> devuelve NewId
  public async Task<int> CreateAsync(Doctor d, string createdBy)
  {
    using var conn = _factory.CreateConnection();

    var prm = new DynamicParameters();
    prm.Add("@FirstName", d.FirstName);
    prm.Add("@LastName", d.LastName);
    prm.Add("@Email", d.Email);
    prm.Add("@Phone", d.Phone);
    prm.Add("@LicenseNumber", d.LicenseNumber);
    prm.Add("@SpecialityId", d.SpecialityId);
    prm.Add("@CreatedBy", createdBy);

    try
    {
      var newId = await conn.ExecuteScalarAsync<int>(
          "dbo.Doctor_Insert", prm, commandType: CommandType.StoredProcedure);
      return newId;
    }
    catch (SqlException ex) when (ex.Number is 2601 or 2627) // UNIQUE (LicenseNumber)
    {
      throw new InvalidOperationException("LicenseNumber ya existe.", ex);
    }
  }

  // UPDATE -> devuelve filas afectadas (0/1)
  public async Task<int> UpdateAsync(Doctor d, string modifiedBy)
  {
    using var conn = _factory.CreateConnection();

    var prm = new DynamicParameters();
    prm.Add("@DoctorId", d.DoctorId);
    prm.Add("@FirstName", d.FirstName);
    prm.Add("@LastName", d.LastName);
    prm.Add("@Email", d.Email);
    prm.Add("@Phone", d.Phone);
    prm.Add("@LicenseNumber", d.LicenseNumber);
    prm.Add("@SpecialityId", d.SpecialityId);
    prm.Add("@ModifiedBy", modifiedBy);

    try
    {
      var affected = await conn.ExecuteScalarAsync<int>(
          "dbo.Doctor_Update", prm, commandType: CommandType.StoredProcedure);
      return affected; // 1 si actualizó, 0 si no
    }
    catch (SqlException ex) when (ex.Number is 2601 or 2627)
    {
      throw new InvalidOperationException("LicenseNumber ya existe.", ex);
    }
  }

  // DELETE -> devuelve filas afectadas (0/1)
  public async Task<int> DeleteAsync(int id)
  {
    using var conn = _factory.CreateConnection();

    var prm = new DynamicParameters();
    prm.Add("@DoctorId", id);

    var affected = await conn.ExecuteScalarAsync<int>(
        "dbo.Doctor_Delete", prm, commandType: CommandType.StoredProcedure);

    return affected; // 1 si borró, 0 si no (o error si SP bloquea por atenciones)
  }

  // GET BY ID
  public async Task<Doctor?> GetByIdAsync(int id)
  {
    using var conn = _factory.CreateConnection();

    var prm = new DynamicParameters();
    prm.Add("@DoctorId", id);

    var doc = await conn.QueryFirstOrDefaultAsync<Doctor>(
        "dbo.Doctor_GetById", prm, commandType: CommandType.StoredProcedure);

    return doc; // null si no existe
  }

  // SEARCH (name en FirstName, specialityId exacto, license exacto)
  public async Task<IEnumerable<Doctor>> SearchAsync(string? name, int? specialityId, string? licenseNumber)
  {
    using var conn = _factory.CreateConnection();

    var prm = new DynamicParameters();
    prm.Add("@Name", name);
    prm.Add("@SpecialityId", specialityId);
    prm.Add("@LicenseNumber", licenseNumber);

    var list = await conn.QueryAsync<Doctor>(
        "dbo.Doctor_Search", prm, commandType: CommandType.StoredProcedure);

    return list;
  }


}
