using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.DataAccess.Models
{
    public class UpdateTrainingRequirementDto
    {
        public int RoleId { get; set; }
        public string ProcedureName { get; set; }
        public int RequiredType { get; set; }
    }
}
