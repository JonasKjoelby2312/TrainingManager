﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public interface IEmployeeComplianceDAO
{
    Task<EmployeeComplianceDAO> GetEmployeeCompliance(string inits);
}
