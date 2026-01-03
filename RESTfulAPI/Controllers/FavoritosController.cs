using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTfulAPI.Entities;
using RESTfulAPI.Repositories;

namespace RESTfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritosController : ControllerBase
    {
        private readonly IFavoritoRepository _repo;
        public FavoritosController(IFavoritoRepository repo) => _repo = repo;

        [HttpGet("{clienteId}")]
        public async Task<ActionResult<IEnumerable<Favorito>>> GetByCliente(string clienteId) =>
            Ok(await _repo.GetByClienteAsync(clienteId));

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Favorito favorito)
        {
            var existing = await _repo.GetByClienteAndProdutoAsync(favorito.ClienteId, favorito.ProdutoId);
            if (existing != null) return Conflict("Já está nos favoritos.");
            await _repo.AddAsync(favorito);
            return CreatedAtAction(nameof(GetByCliente), new { clienteId = favorito.ClienteId }, favorito);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
