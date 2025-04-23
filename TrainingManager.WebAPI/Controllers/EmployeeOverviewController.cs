using Microsoft.AspNetCore.Mvc;
using TrainingManager.DataAccess.DAOs;
using TrainingManager.DataAccess.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainingManager.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeOverviewController : ControllerBase
    {
        private IEmployeeDAO _employeeDAO;

        public EmployeeOverviewController(IEmployeeDAO EmployeeDAO)
        {
            _employeeDAO = EmployeeDAO;
        }

        // GET: api/<EmployeeOverviewController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAsync()
        {
            IEnumerable<Employee> employees = await _employeeDAO.GetAllEmployeesAndStatusesAsync();
            return Ok(employees);
        }

        // GET api/<EmployeeOverviewController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EmployeeOverviewController>
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Employee entity)
        {
            if (entity.Initials != null && entity.Email != null)
            {
                int employeeId = await _employeeDAO.CreateAsync(entity);
                return Ok(employeeId);
            } else
            {
                return NotFound();
            }
            
        }

        // PUT api/<EmployeeOverviewController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Employee entity)
        {
            _employeeDAO.UpdateAsync(entity);
        }

        // DELETE api/<EmployeeOverviewController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
