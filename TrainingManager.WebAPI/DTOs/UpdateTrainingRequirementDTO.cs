namespace TrainingManager.WebAPI.DTOs;

public class UpdateTrainingRequirementDTO
{
    public int RoleId { get; set; }
    public string ProcedureName { get; set; }
    public int RequiredType { get; set; }
}
