using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests.Seguranca;
using OficinaMotos.Application.DTOs.Responses.Seguranca;
using OficinaMotos.Application.Interfaces.Seguranca;

namespace OficinaMotos.API.Controllers.Seguranca
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Tags("Seguranca Modulos")]
    [Authorize]
    public class ModulosController : ControllerBase
    {
        private readonly ISegModuloService _service;

        public ModulosController(ISegModuloService service) => _service = service;

        [HttpGet]
        [ProducesResponseType(typeof(List<SegModuloResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("ativos")]
        [ProducesResponseType(typeof(List<SegModuloResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAtivos()
            => Ok(await _service.GetAtivosAsync());

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(SegModuloResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Módulo não encontrado." });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SegModuloResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSegModuloDTO request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(SegModuloResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateSegModuloDTO request)
        {
            var updated = await _service.UpdateAsync(id, request);
            if (updated == null) return NotFound(new { message = "Módulo não encontrado." });
            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Módulo não encontrado." });
            return NoContent();
        }
    }
}
