using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.DataAccess.Models;

public class EmployeeCompliance
{
    public List<string[]> Rows { get; set; }

    public EmployeeCompliance()
    {
        Rows = new List<string[]>();
    }
}
