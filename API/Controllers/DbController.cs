using Microsoft.AspNetCore.Mvc;
using Persistence.Diagnostics;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DbController : ControllerBase
{
  private readonly DbHealthChecker _health;
  public DbController(DbHealthChecker health) => _health = health;

  [HttpGet("ping")]
  public async Task<IActionResult> Ping()
  {
    var (ok, error, server, database) = await _health.PingAsync();
    if (!ok) return StatusCode(500, new { ok, error });
    return Ok(new { ok, database, server });
  }
}
