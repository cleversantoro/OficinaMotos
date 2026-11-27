using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests.Prestador;
using OficinaMotos.Application.DTOs.Responses.Prestador;
using OficinaMotos.Application.Interfaces.Prestador;

namespace OficinaMotos.API.Controllers.Prestador
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PrestadoresAtividadesController : ControllerBase
    {
        private readonly IPrestadorAtividadeService _service;

        public PrestadoresAtividadesController(IPrestadorAtividadeService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PrestadorAtividadeResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{prestadorId:long}/{atividadeProfissionalId:long}")]
        [ProducesResponseType(typeof(PrestadorAtividadeResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long prestadorId, long atividadeProfissionalId)
        {
            var item = await _service.GetByIdAsync(prestadorId, atividadeProfissionalId);
            if (item == null) return NotFound(new { message = "Atividade do prestador não encontrada." });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PrestadorAtividadeResponseDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreatePrestadorAtividadeDTO request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { prestadorId = created.PrestadorId, atividadeProfissionalId = created.AtividadeProfissionalId }, created);
        }

        [HttpPut("{prestadorId:long}/{atividadeProfissionalId:long}")]
        [ProducesResponseType(typeof(PrestadorAtividadeResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long prestadorId, long atividadeProfissionalId, [FromBody] UpdatePrestadorAtividadeDTO request)
        {
            var updated = await _service.UpdateAsync(prestadorId, atividadeProfissionalId, request);
            if (updated == null) return NotFound(new { message = "Atividade do prestador não encontrada." });
            return Ok(updated);
        }

        [HttpDelete("{prestadorId:long}/{atividadeProfissionalId:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long prestadorId, long atividadeProfissionalId)
        {
            var deleted = await _service.DeleteAsync(prestadorId, atividadeProfissionalId);
            if (!deleted) return NotFound(new { message = "Atividade do prestador não encontrada." });
            return NoContent();
        }
    }
}
