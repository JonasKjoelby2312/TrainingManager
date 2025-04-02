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
    public List<Procedure> MandatoryTraining { get; set; }
    public List<Procedure> IfPerformingTraining { get; set; }
    public List<Procedure> OptionalTraining { get; set; }
}
