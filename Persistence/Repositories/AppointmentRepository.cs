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

public class AppointmentRepository
{
  private readonly IDbConnectionFactory _factory;
  public AppointmentRepository(IDbConnectionFactory factory) => _factory = factory;

  // INSERT -> devuelve NewId
  public async Task<int> CreateAsync(Appointment a, string createdBy)
  {
    using var conn = _factory.CreateConnection();

    var prm = new DynamicParameters();
    prm.Add("@PatientId", a.PatientId);
    prm.Add("@DoctorId", a.DoctorId);
    prm.Add("@StartUtc", a.StartUtc);
    prm.Add("@EndUtc", a.EndUtc);
    prm.Add("@Diagnosis", a.Diagnosis);
    prm.Add("@Room", a.Room);
    prm.Add("@Status", a.Status);
    prm.Add("@CreatedBy", createdBy);

    try
    {
      var newId = await conn.ExecuteScalarAsync<int>(
          "dbo.Appointment_Insert", prm, commandType: CommandType.StoredProcedure);
      return newId;
    }
    catch (SqlException ex)
    {
      switch (ex.State)
      {
        case 1: // EndUtc <= StartUtc
          {
            var e = new ArgumentException("La hora de término debe ser mayor que la de inicio.", nameof(a.EndUtc), ex);
            e.Data["SqlState"] = 1;
            throw e;
          }
        case 2: // Paciente no encontrado
          {
            var e = new KeyNotFoundException("Paciente no encontrado.", ex);
            e.Data["SqlState"] = 2;
            throw e;
          }
        case 3: // Doctor no encontrado
          {
            var e = new KeyNotFoundException("Doctor no encontrado.", ex);
            e.Data["SqlState"] = 3;
            throw e;
          }
        case 4: // Solapamiento
          {
            var e = new InvalidOperationException("El doctor ya tiene una atención en ese horario.", ex);
            e.Data["SqlState"] = 4;
            throw e;
          }
        default:
          // Por si llega una violación FK estándar (547) u otros
          if (ex.Number == 547)
          {
            var e = new InvalidOperationException("La atención hace referencia a datos inexistentes.", ex);
            e.Data["SqlNumber"] = 547;
            throw e;
          }
          throw; // deja pasar cualquier otra cosa
      }
    }
  }

  // SEARCH (filtros opcionales)
  public async Task<IEnumerable<AppointmentSearchResult>> SearchAsync(
      DateTime? fromUtc, DateTime? toUtc, int? doctorId, int? patientId, int? specialityId, string? status)
  {
    using var conn = _factory.CreateConnection();

    var prm = new DynamicParameters();
    prm.Add("@FromUtc", fromUtc);
    prm.Add("@ToUtc", toUtc);
    prm.Add("@DoctorId", doctorId);
    prm.Add("@PatientId", patientId);
    prm.Add("@SpecialityId", specialityId);
    
    var list = await conn.QueryAsync<AppointmentSearchResult>(
        "dbo.Appointment_Search", prm, commandType: CommandType.StoredProcedure);

    return list;
  }

  // PROMEDIO por especialidad (opcionalmente filtrado por id)
  public async Task<IEnumerable<AverageDurationResult>> AverageBySpecialityAsync(int? specialityId)
  {
    using var conn = _factory.CreateConnection();

    var prm = new DynamicParameters();
    prm.Add("@SpecialityId", specialityId);

    var list = await conn.QueryAsync<AverageDurationResult>(
        "dbo.Appointment_AverageDuration", prm, commandType: CommandType.StoredProcedure);

    return list;
  }

  public async Task<AppointmentSearchResult?> GetByIdAsync(int id)
  {
    using var conn = _factory.CreateConnection();
    var prm = new DynamicParameters();
    prm.Add("@AppointmentId", id);

    var item = await conn.QuerySingleOrDefaultAsync<AppointmentSearchResult>(
        "dbo.Appointment_GetById", prm, commandType: CommandType.StoredProcedure);

    return item;
  }

}
