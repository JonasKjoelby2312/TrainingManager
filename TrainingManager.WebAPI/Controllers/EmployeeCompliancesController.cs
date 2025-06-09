using Microsoft.AspNetCore.Mvc;
using TrainingManager.DataAccess.DAOs;
using TrainingManager.DataAccess.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainingManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeCompliancesController : ControllerBase
    {
        private IEmployeeDAO _employeeDAO;

        public EmployeeCompliancesController(IEmployeeDAO employeeDAO)
        {
            _employeeDAO = employeeDAO;
        }

        // GET: api/<EmployeeComplianceController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EmployeeComplianceController>/5
        [HttpGet("{initials}")]
        public async Task<ActionResult<MatrixParent>> Get(string initials)
        {
            MatrixParent res = await _employeeDAO.GetEmployeeComplianceByInitialsAsync(initials);
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
