using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.DataAccess.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public Dictionary<string, int> ProceduresRequiredTypes { get; set; }

    public Role()
    {
        ProceduresRequiredTypes = new Dictionary<string, int>();
    }
}
