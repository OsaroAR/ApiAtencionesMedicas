using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

internal class Patient
{
  public int PacientId { get; set; }
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string RUT { get; set; } = string.Empty;
  public DateTime DateOfBirth { get; set; }
  public string? Gender { get; set; }
  public string? Phone { get; set; }
  public string? Email { get; set; }
  public string? AddressLine1 { get; set; }
  public string? AddressLine2 { get; set; }
  public string? City { get; set; }
  public string? State { get; set; }
  public string? PostalCode { get; set; }
}
