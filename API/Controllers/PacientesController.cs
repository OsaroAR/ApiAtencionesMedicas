using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Persistence.Repositories;
using Microsoft.Data.SqlClient;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController(PacienteRepository repo) : ControllerBase
{
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id)
  {
    var p = await repo.GetByIdAsync(id);
    return p is null ? NotFound() : Ok(p);
  }

  // /api/pacientes?rut=xxx&name=juan&minAge=20&maxAge=40
  [HttpGet]
  public async Task<IActionResult> Search([FromQuery] string? rut, [FromQuery] string? name,
                                          [FromQuery] int? minAge, [FromQuery] int? maxAge)
  {
    var list = await repo.SearchAsync(rut, name, minAge, maxAge);
    return Ok(list);
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] Patient p)
  {
    try
    {
      var id = await repo.CreateAsync(p, createdBy: "api");
      p.PatientId = id;
      return CreatedAtAction(nameof(GetById), new { id }, p);
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("RUT ya existe"))
    {
      return Conflict(new { error = "RUT ya existe" });
    }
    catch (SqlException ex)
    {
      return Problem(title: "Error SQL al crear paciente", detail: ex.Message, statusCode: 500);
    }
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(int id, [FromBody] Patient p)
  {
    if (id != p.PatientId) return BadRequest(new { error = "Ruta y body no coinciden" });

    try
    {
      await repo.UpdateAsync(p, modifiedBy: "api");
      return Ok(p);
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("RUT ya existe"))
    {
      return Conflict(new { error = "RUT ya existe" });
    }
    catch (SqlException ex)
    {
      return Problem(title: "Error SQL al actualizar paciente", detail: ex.Message, statusCode: 500);
    }
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      var affected = await repo.DeleteAsync(id);
      return affected == 0 ? NotFound() : NoContent();
    }
    catch (SqlException ex)
    {
      // p.ej. “tiene atenciones” si el SP lo valida con RAISERROR
      return Problem(title: "Error SQL al eliminar paciente", detail: ex.Message, statusCode: 409);
    }
  }
}
