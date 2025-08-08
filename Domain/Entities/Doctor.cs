using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

internal class Doctor
{
  public int DoctorId { get; set; }
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string? Email { get; set; }
  public string? Phone { get; set; }
  public string LicenseNumber { get; set; } = string.Empty;
  public int SpecialityId { get; set; }
}
