﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.DataAccess.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public string Initials { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public Dictionary<string, string>? EmployeeTrainingStatuses { get; set; }
    public bool IsActive { get; set; }

    public Employee()
    {
        EmployeeTrainingStatuses = new Dictionary<string, string>();
    }

    public Employee(int id, string initials, string email, bool isActive)
    {
        EmployeeId = id;
        Initials = initials;
        Email = email;
        EmployeeTrainingStatuses = new Dictionary<string, string>();
        IsActive = isActive;
    }
}
