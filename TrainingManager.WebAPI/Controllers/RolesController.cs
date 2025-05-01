using Microsoft.AspNetCore.Mvc;
using TrainingManager.DataAccess.DAOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainingManager.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    IRoleDAO _roleDAO;

    public RolesController(IRoleDAO roleDAO)
    {
        _roleDAO = roleDAO;
    }

    // GET: api/<RolesController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> Get()
    {
        IEnumerable<string> res = await _roleDAO.GetAllRolesAsync();
        return Ok(res);
    }

    // GET api/<RolesController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<RolesController>
    [HttpPost]
    public void Post([FromBody]string value)
    {
    }

    // PUT api/<RolesController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/<RolesController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
