using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Persistence.Repositories;
using Microsoft.Data.SqlClient;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctoresController : ControllerBase
{
  private readonly DoctorRepository _repo;
  public DoctoresController(DoctorRepository repo)=> _repo = repo;

  // GET /api/doctores/5
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id)
  {
    var d = await _repo.GetByIdAsync(id);
    return d is null ? NotFound() : Ok(d);
  }

  // GET /api/doctores?name=ana&specialityId=1&licenseNumber=MED-123
  [HttpGet]
  public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] int? specialityId, [FromQuery] string? licenseNumber)
  {
    var list = await _repo.SearchAsync(name, specialityId, licenseNumber);
    return Ok(list);
  }

  // POST /api/doctores
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] Doctor d)
  {
    try
    {
      var id = await _repo.CreateAsync(d, "api");
      d.DoctorId = id;
      return CreatedAtAction(nameof(GetById), new { id }, d);
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("LicenseNumber ya existe"))
    {
      return Conflict(new { error = "LicenseNumber ya existe" });
    }
    catch (SqlException ex)
    {
      return Problem("Error SQL al crear doctor", ex.Message, 500);
    }
  }

  // PUT /api/doctores/5
  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(int id, [FromBody] Doctor d)
  {
    if (id != d.DoctorId) return BadRequest(new { error = "Ruta y body no coinciden" });

    try
    {
      var affected = await _repo.UpdateAsync(d, "api");
      return affected == 0 ? NotFound() : Ok(d);
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("LicenseNumber ya existe"))
    {
      return Conflict(new { error = "LicenseNumber ya existe" });
    }
    catch (SqlException ex)
    {
      return Problem("Error SQL al actualizar doctor", ex.Message, 500);
    }
  }

  // DELETE /api/doctores/5
  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      var affected = await _repo.DeleteAsync(id);
      return affected == 0 ? NotFound() : NoContent();
    }
    catch (SqlException ex)
    {
      return Problem("Error SQL al eliminar doctor", ex.Message, 409);
    }
  }
}