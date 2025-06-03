using Microsoft.AspNetCore.Mvc;
using TrainingManager.DataAccess.DAOs;
using TrainingManager.DataAccess.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainingManager.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminComplianceMatrixController : ControllerBase
{
    IMatrixDAO _matrixDAO;

    public AdminComplianceMatrixController(IMatrixDAO matrixDAO)
    {
        _matrixDAO = matrixDAO;
    }

    // GET: api/<AdminComplianceMatrix>
    [HttpGet]
    public async Task<MatrixParent> Get()
    {
        MatrixParent res = await _matrixDAO.GetAdminComplianceMatrixAsync();
        return res;
    }

    // GET api/<AdminComplianceMatrix>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<AdminComplianceMatrix>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<AdminComplianceMatrix>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<AdminComplianceMatrix>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}