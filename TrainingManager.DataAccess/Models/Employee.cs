using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.DataAccess.Models;

public class Employee
{
    public int Id { get; set; }
    public string Initials { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public Dictionary<Procedure, string>? EmployeeTrainingStatuses { get; set; }
    public bool IsActive { get; set; }

    public Employee()
    {
        
    }
}
