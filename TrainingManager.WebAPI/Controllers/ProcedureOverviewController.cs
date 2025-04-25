using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TrainingManager.DataAccess.DAOs;
using TrainingManager.DataAccess.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainingManager.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProcedureOverviewController : ControllerBase
{
    private IProcedureDAO _procedureDAO;

    public ProcedureOverviewController(IProcedureDAO procedureDAO)
    {
        _procedureDAO = procedureDAO;
    }
    // GET: api/<ProcedureOverviewController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Procedure>>> GetAllPorceduresWithRevisionsAsync()
    {
        var procedures = await _procedureDAO.GetAllProceduresWithRevisionsAsync();
        return Ok(procedures);
    }

    [HttpGet("revisions/{ProcedureName}")]
    public async Task<IActionResult> GetRevisionsForProcedureAsync(string procedureName)
    {
        var revisions = await _procedureDAO.GetRevisionsForProcedureAsync(procedureName);
        return Ok(revisions);
    }

    // GET api/<ProcedureOverviewController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<ProcedureOverviewController>
    [HttpPost]
    public async Task<ActionResult<int>> Post([FromBody] CreateProcedureDto dto)
    {
        try
        {
            var procedure = new Procedure
            {
                ProcedureName = dto.ProcedureName,
                RevisionNumber = dto.RevisionNumber,
                IsActive = dto.IsActive,
                HistoryText = dto.HistoryText,

                RolesRequiredTrainingList = dto.RolesRequiredTrainingList
                    .Select(r =>
                    {
                        var model = new RolesRequiredTraining(r.RoleId, null);
                        model.TrainingRequiredTypes.Add("default", r.RequiredType);
                        return model;
                    })
                    .ToList()   
            };

            return Ok(await _procedureDAO.CreateAsync(procedure));
        }
        catch (Exception ex)
        {
            return BadRequest($"Error creating procedure: {ex.Message}");
        }
    }



    // PUT api/<ProcedureOverviewController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ProcedureOverviewController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
