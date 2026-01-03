using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTfulAPI.Entities;
using RESTfulAPI.Repositories;

namespace RESTfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModosEntregaController : ControllerBase
    {
        private readonly IModoEntregaRepository _repo;
        public ModosEntregaController(IModoEntregaRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModoEntrega>>> Get() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<ModoEntrega>> Get(int id) => Ok(await _repo.GetByIdAsync(id));

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ModoEntrega modo)
        {
            await _repo.AddAsync(modo);
            return CreatedAtAction(nameof(Get), new { id = modo.Id }, modo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ModoEntrega modo)
        {
            if (id != modo.Id) return BadRequest();
            await _repo.UpdateAsync(modo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
