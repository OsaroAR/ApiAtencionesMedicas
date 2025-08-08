using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

internal class Appointment
{
  public int AppointmentId { get; set; }
  public int PatientId { get; set; }
  public int DoctorId { get; set; }
  public DateTime StartUtc { get; set; }
  public DateTime EndUtc { get; set; }
  public string? Diagnosis { get; set; }
  public string? Room { get; set; }
  public string? Status { get; set; }
}
