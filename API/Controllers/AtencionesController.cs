using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Persistence.Repositories;
using Domain.Entities;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AtencionesController : ControllerBase
{
  private readonly AppointmentRepository _repo;

  public AtencionesController(AppointmentRepository repo) => _repo = repo;

  // POST /api/atenciones
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] Appointment a)
  {
    try
    {
      var id = await _repo.CreateAsync(a, "api");
      a.AppointmentId = id;
      return CreatedAtAction(nameof(GetPlaceholder), new { id }, a);
    }
    catch (ArgumentException ex) // State = 2 (End <= Start)
    {
      return BadRequest(new { error = ex.Message });
    }
    catch (KeyNotFoundException ex) // State = 3 (Paciente no encontrado), 4 (Doctor no encontrado)
    {
      return NotFound(new { error = ex.Message });
    }
    catch (InvalidOperationException ex) when ((ex.Data["SqlState"]?.ToString()) == "5")
    {
      // State = 5 (Solapamiento)
      return Conflict(new { error = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
      // Ej: FK 547 mapeado en el repo → 400
      return BadRequest(new { error = ex.Message });
    }
    catch (SqlException ex)
    {
      // SQL no contemplado → 500
      return Problem("Error SQL al crear atención", ex.Message, 500);
    }
    catch (Exception ex)
    {
      return Problem("Error inesperado al crear atención", ex.Message, 500);
    }
  }

  // GET /api/atenciones?fromUtc=...&toUtc=...&doctorId=...&patientId=...&specialityId=...&status=...
  [HttpGet]
  public async Task<IActionResult> Search(
      [FromQuery] DateTime? fromUtc,
      [FromQuery] DateTime? toUtc,
      [FromQuery] int? doctorId,
      [FromQuery] int? patientId,
      [FromQuery] int? specialityId,
      [FromQuery] string? status)
  {
    var list = await _repo.SearchAsync(fromUtc, toUtc, doctorId, patientId, specialityId, status);
    return Ok(list);
  }

  // GET /api/atenciones/avg?specialityId=1
  [HttpGet("avg")]
  public async Task<IActionResult> Average([FromQuery] int? specialityId)
  {
    var list = await _repo.AverageBySpecialityAsync(specialityId);
    return Ok(list);
  }

  // Placeholder para CreatedAtAction (si no tienes GET por id)
  [NonAction]
  public IActionResult GetPlaceholder(int id) => NoContent();
}
