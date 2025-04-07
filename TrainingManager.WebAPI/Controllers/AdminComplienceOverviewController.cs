using Microsoft.AspNetCore.Mvc;
using TrainingManager.DataAccess.DAOs;
using TrainingManager.DataAccess.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainingManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminComplienceOverviewController : ControllerBase
    {
        private IEmployeeDAO _employeeDAO;

        public AdminComplienceOverviewController(IEmployeeDAO employeeDAO)
        {
            _employeeDAO = employeeDAO;
        }

        // GET: api/<AdminComplienceOverviewController>
        //Det er den her vi bruger pt
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAsync()
        {
            var employees = await _employeeDAO.GetAllAsync();
            return Ok(employees);
        }

        // GET api/<AdminComplienceOverviewController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AdminComplienceOverviewController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AdminComplienceOverviewController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AdminComplienceOverviewController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
