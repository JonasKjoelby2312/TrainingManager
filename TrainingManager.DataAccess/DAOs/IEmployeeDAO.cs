using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public interface IEmployeeDAO
{
    Task<int> CreateAsync(Employee entity);
    Task<IEnumerable<Employee>> GetAllEmployeesAndStatusesAsync();
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee> GetByIdAsync(int id);
    Task<int> UpdateAsync(Employee entity);
}
