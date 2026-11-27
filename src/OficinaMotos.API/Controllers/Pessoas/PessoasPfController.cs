using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using OficinaMotos.Application.Interfaces.Pessoas;

namespace OficinaMotos.API.Controllers.Pessoas
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PessoasPfController : ControllerBase
    {
        private readonly IPessoaPfService _service;

        public PessoasPfController(IPessoaPfService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PessoaPfResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PessoaPfResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByPessoaIdAsync(id);
            if (item == null) return NotFound(new { message = "Registro PF nao encontrado." });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PessoaPfResponseDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreatePessoaPfDTO request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.PessoaId }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PessoaPfResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdatePessoaPfDTO request)
        {
            var updated = await _service.UpdateAsync(id, request);
            if (updated == null) return NotFound(new { message = "Registro PF nao encontrado." });
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Registro PF nao encontrado." });
            return NoContent();
        }
    }
}
