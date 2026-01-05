using Microsoft.AspNetCore.Mvc;
using MyMedia.Domain.Entities;
using RESTfulAPI.Repositories;
namespace RESTfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrinhoController : ControllerBase
    {
        private readonly ICarrinhoRepository _repo;
        public CarrinhoController(ICarrinhoRepository repo) => _repo = repo;

        [HttpGet("{clienteId}")]
        public async Task<ActionResult<IEnumerable<Carrinho>>> GetByCliente(string clienteId) =>
            Ok(await _repo.GetByClienteAsync(clienteId));

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Carrinho item)
        {
            var existing = await _repo.GetByClienteAndProdutoAsync(item.ClienteId, item.ProdutoId);
            if (existing != null)
            {
                existing.Quantidade += item.Quantidade;
                await _repo.UpdateAsync(existing);
                return NoContent();
            }
            await _repo.AddAsync(item);
            return CreatedAtAction(nameof(GetByCliente), new { clienteId = item.ClienteId }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Carrinho item)
        {
            if (id != item.Id) return BadRequest();
            await _repo.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("limpar/{clienteId}")]
        public async Task<ActionResult> Clear(string clienteId)
        {
            await _repo.ClearByClienteAsync(clienteId);
            return NoContent();
        }
    }
}
