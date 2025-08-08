using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security;

public class ApiKeySettings
{
  public string HeaderName { get; set; } = "X-API-Key";
  public List<string> ApiKeys { get; set; } = new();
  public bool EnableForSwagger { get; set; } = false;
}
