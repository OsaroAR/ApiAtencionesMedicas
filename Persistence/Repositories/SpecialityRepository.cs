using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Application.Common.Interfaces;
using Dapper;

namespace Persistence.Repositories;

public class SpecialityRepository
{
  private readonly IDbConnectionFactory _factory;
  public SpecialityRepository(IDbConnectionFactory factory) => _factory = factory;

  public async Task<int> CreateAsync(string name, string? description, string createdBy)
  {
    using var conn = _factory.CreateConnection();
    var prm = new DynamicParameters();
    prm.Add("@Name", name);
    prm.Add("@Description", description);
    prm.Add("@CreatedBy", createdBy);

    var newId = await conn.ExecuteScalarAsync<int>(
        "dbo.Speciality_Insert", prm, commandType: CommandType.StoredProcedure);
    return newId;
  }

  public async Task<int> UpdateAsync(int id, string name, string? description, string modifiedBy)
  {
    using var conn = _factory.CreateConnection();
    var prm = new DynamicParameters();
    prm.Add("@SpecialityId", id);
    prm.Add("@Name", name);
    prm.Add("@Description", description);
    prm.Add("@ModifiedBy", modifiedBy);

    var affected = await conn.ExecuteScalarAsync<int>(
        "dbo.Speciality_Update", prm, commandType: CommandType.StoredProcedure);
    return affected;
  }

  public async Task<int> DeleteAsync(int id)
  {
    using var conn = _factory.CreateConnection();
    var prm = new DynamicParameters();
    prm.Add("@SpecialityId", id);

    var affected = await conn.ExecuteScalarAsync<int>(
        "dbo.Speciality_Delete", prm, commandType: CommandType.StoredProcedure);
    return affected;
  }

  public async Task<object?> GetByIdAsync(int id)
  {
    using var conn = _factory.CreateConnection();
    var prm = new DynamicParameters();
    prm.Add("@SpecialityId", id);

    return await conn.QueryFirstOrDefaultAsync<object>(
        "dbo.Speciality_GetById", prm, commandType: CommandType.StoredProcedure);
  }

  public async Task<IEnumerable<object>> GetAllAsync()
  {
    using var conn = _factory.CreateConnection();
    return await conn.QueryAsync<object>(
        "dbo.Speciality_GetAll", commandType: CommandType.StoredProcedure);
  }
}
