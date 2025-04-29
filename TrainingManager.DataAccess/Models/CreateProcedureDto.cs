using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.DataAccess.Models
{
    public class CreateProcedureDto
    {
        [Required]
        public string ProcedureName { get; set; }
        public decimal RevisionNumber { get; set; } = 1;
        public bool IsActive { get; set; } = true;

        [Required]
        public string HistoryText { get; set; }
        public List<TrainingRequirementDto> RolesRequiredTrainingList { get; set; }
    }

}
