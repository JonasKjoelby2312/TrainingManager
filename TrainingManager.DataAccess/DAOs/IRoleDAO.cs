using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public interface IRoleDAO
{
    Task<int> CreateAsync(Role entity);
    Task<IEnumerable<Role>> GetAllRolesForEmployeeAsync(int id);
    Task<Role> GetRoleByIdAsync(int id);
    Task<int> UpdateAsync(Role entity);
}
