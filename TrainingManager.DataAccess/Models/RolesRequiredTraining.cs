using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.DataAccess.Models;

public class RolesRequiredTraining
{
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public Dictionary<string, int> TrainingRequiredTypes { get; set; }

    public RolesRequiredTraining(int roleId, string roleName)
    {
        RoleId = roleId;
        RoleName = roleName;
        TrainingRequiredTypes = new Dictionary<string, int>();
    }

  
}
