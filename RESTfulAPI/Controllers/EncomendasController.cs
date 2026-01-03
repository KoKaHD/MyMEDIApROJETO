using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTfulAPI.Entities;
using RESTfulAPI.Repositories;

namespace RESTfulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncomendasController : ControllerBase
    {
        private readonly IEncomendaRepository _repo;
        public EncomendasController(IEncomendaRepository repo) => _repo = repo;

        [HttpGet("{clienteId}")]
        public async Task<ActionResult<IEnumerable<Encomenda>>> GetByCliente(string clienteId) =>
            Ok(await _repo.GetByClienteAsync(clienteId));

        [HttpGet("detalhe/{id}")]
        public async Task<ActionResult<Encomenda>> Get(int id) => Ok(await _repo.GetByIdAsync(id));

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Encomenda encomenda)
        {
            await _repo.AddAsync(encomenda);
            return CreatedAtAction(nameof(Get), new { id = encomenda.Id }, encomenda);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Encomenda encomenda)
        {
            if (id != encomenda.Id) return BadRequest();
            await _repo.UpdateAsync(encomenda);
            return NoContent();
        }
    }
}
