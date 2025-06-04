using System.ComponentModel.DataAnnotations;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.WebAPI.DTOs;

public class CreateProcedureDTO
{
    [Required]
    public string ProcedureName { get; set; }
    public decimal RevisionNumber { get; set; } = 1;
    public bool IsActive { get; set; } = true;

    [Required]
    public string HistoryText { get; set; }
    public List<TrainingRequirementDTO> RolesRequiredTrainingList { get; set; }
}
