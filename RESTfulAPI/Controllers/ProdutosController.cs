using Microsoft.AspNetCore.Mvc;
using MyMedia.Domain.Entities;
using RESTfulAPI.Repositories;
namespace RESTfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _repo;

        public ProdutosController(IProdutoRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> Get(int id) => Ok(await _repo.GetByIdAsync(id));

        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetByCategoria(int categoriaId) =>
            Ok(await _repo.GetByCategoriaAsync(categoriaId));

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Produto produto)
        {
            await _repo.AddAsync(produto);
            return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.Id) return BadRequest();
            await _repo.UpdateAsync(produto);
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

