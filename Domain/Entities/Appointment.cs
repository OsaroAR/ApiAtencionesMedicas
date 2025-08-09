using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Appointment
{
  public int AppointmentId { get; set; }
  public int PatientId { get; set; }
  public int DoctorId { get; set; }
  public DateTime StartUtc { get; set; }
  public DateTime EndUtc { get; set; }
  public string? Diagnosis { get; set; }
  public string? Room { get; set; }
  public string? Status { get; set; } // Scheduled/Completed/Cancelled
}

// Para resultados de búsqueda
public class AppointmentSearchResult
{
  public int AppointmentId { get; set; }
  public int PatientId { get; set; }
  public string PatientFirstName { get; set; } = default!;
  public string PatientLastName { get; set; } = default!;
  public int DoctorId { get; set; }
  public string DoctorFirstName { get; set; } = default!;
  public string DoctorLastName { get; set; } = default!;
  public int SpecialityId { get; set; }
  public string SpecialityName { get; set; } = default!;
  public DateTime StartUtc { get; set; }
  public DateTime EndUtc { get; set; }
  public int DurationMinutes { get; set; }
  public string? Diagnosis { get; set; }
  public string? Room { get; set; }
  public string? Status { get; set; }
}

// Para resultado de duración promedio de atencion (min)
public class AverageDurationResult
{
  public int SpecialityId { get; set; }
  public string SpecialityName { get; set; } = default!;
  public decimal AvgDurationMinutes { get; set; }
  public int TotalAppointments { get; set; }
}