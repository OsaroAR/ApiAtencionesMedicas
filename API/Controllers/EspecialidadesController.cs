using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Persistence.Repositories;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EspecialidadesController : ControllerBase
{
  private readonly SpecialityRepository _repo;
  public EspecialidadesController(SpecialityRepository repo) => _repo = repo;

  // GET /api/especialidades
  [HttpGet]
  public async Task<IActionResult> GetAll()
      => Ok(await _repo.GetAllAsync());

  // GET /api/especialidades/5
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id)
      => (await _repo.GetByIdAsync(id)) is { } s ? Ok(s) : NotFound();

  // POST /api/especialidades
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] SpecialityDto dto)
  {
    var id = await _repo.CreateAsync(dto.Name, dto.Description, "api");
    return CreatedAtAction(nameof(GetById), new { id }, new { specialityId = id, dto.Name, dto.Description });
  }

  // PUT /api/especialidades/5
  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(int id, [FromBody] SpecialityDto dto)
  {
    var affected = await _repo.UpdateAsync(id, dto.Name, dto.Description, "api");
    return affected == 0 ? NotFound() : Ok(new { specialityId = id, dto.Name, dto.Description });
  }

  // DELETE /api/especialidades/5
  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id)
  {
    try
    {
      var affected = await _repo.DeleteAsync(id);
      return affected == 0 ? NotFound() : NoContent();
    }
    catch (KeyNotFoundException ex)
    {
      return NotFound(new { error = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
      // doctores asociados / FK → 409
      return Conflict(new { error = ex.Message });
    }
    catch (SqlException ex)
    {
      return Problem("Error SQL al eliminar especialidad", ex.Message, 500);
    }
  }

  public record SpecialityDto(string Name, string? Description);
}
