using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using OficinaMotos.Application.Interfaces.OrdemServico;

namespace OficinaMotos.API.Controllers.OrdemServico
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdemServicoChecklistsController : ControllerBase
    {
        private readonly IOrdemServicoChecklistService _service;

        public OrdemServicoChecklistsController(IOrdemServicoChecklistService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<OrdemServicoChecklistResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrdemServicoChecklistResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Checklist não encontrado." });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrdemServicoChecklistResponseDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateOrdemServicoChecklistDTO request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrdemServicoChecklistResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateOrdemServicoChecklistDTO request)
        {
            var updated = await _service.UpdateAsync(id, request);
            if (updated == null) return NotFound(new { message = "Checklist não encontrado." });
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Checklist não encontrado." });
            return NoContent();
        }
    }
}
