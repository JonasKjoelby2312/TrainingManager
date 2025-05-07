using Microsoft.AspNetCore.Mvc;
using TrainingManager.DataAccess.DAOs;
using TrainingManager.DataAccess.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainingManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesRequiredTrainingController : ControllerBase
    {
        private IRolesRequiredTrainingDAO _rolesRequiredTraining;

        public RolesRequiredTrainingController(IRolesRequiredTrainingDAO rolesRequiredTrainingDAO)
        {
            _rolesRequiredTraining = rolesRequiredTrainingDAO;
        }

        // GET: api/<RolesRequiredTrainingController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolesRequiredTraining>>> Get()
        {
            IEnumerable<RolesRequiredTraining> res = await _rolesRequiredTraining.GetAllRolesRequiredTraining();
            return Ok(res);
        }

        // GET api/<RolesRequiredTrainingController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RolesRequiredTrainingController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RolesRequiredTrainingController>/5
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UpdateTrainingRequirementDto dto)
        {
           await _rolesRequiredTraining.UpdateTrainingRequirementAsync(dto);
            return Ok();
        }

        // DELETE api/<RolesRequiredTrainingController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
