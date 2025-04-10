using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.DataAccess.Models;

public class EmployeeCompliance
{
    public Revision Revision { get; set; }
    public Dictionary<string, string> TrainingStatus { get; set; }
    public int CompletedRevision { get; set; }

    public EmployeeCompliance()
    {
        TrainingStatus = new Dictionary<string, string>();
    }
}
