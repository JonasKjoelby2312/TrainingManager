using Microsoft.AspNetCore.Mvc;
using TrainingManager.DataAccess.DAOs;
using TrainingManager.DataAccess.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainingManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeComplianceController : ControllerBase
    {
        private IEmployeeComplianceDAO _employeeComplianceDAO;

        public EmployeeComplianceController(IEmployeeComplianceDAO employeeComplianceDAO)
        {
            _employeeComplianceDAO = employeeComplianceDAO;
        }

        // GET: api/<EmployeeComplianceController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EmployeeComplianceController>/5
        [HttpGet("{initials}")]
        public async Task<ActionResult<EmployeeCompliance>> Get(string initials)
        {
            EmployeeCompliance res = await _employeeComplianceDAO.GetEmployeeComplianceAsync(initials);
            return Ok(res);
        }

        // POST api/<EmployeeComplianceController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<EmployeeComplianceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EmployeeComplianceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
