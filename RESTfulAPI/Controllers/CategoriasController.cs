using Microsoft.AspNetCore.Mvc;
using MyMedia.Domain.Entities;
using RESTfulAPI.Repositories;
namespace RESTfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _repo;

        public CategoriasController(ICategoriaRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> Get(int id) => Ok(await _repo.GetByIdAsync(id));

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Categoria categoria)
        {
            await _repo.AddAsync(categoria);
            return CreatedAtAction(nameof(Get), new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.Id) return BadRequest();
            await _repo.UpdateAsync(categoria);
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
