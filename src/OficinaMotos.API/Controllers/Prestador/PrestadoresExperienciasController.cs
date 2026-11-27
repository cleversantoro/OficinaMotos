using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests.Prestador;
using OficinaMotos.Application.DTOs.Responses.Prestador;
using OficinaMotos.Application.Interfaces.Prestador;

namespace OficinaMotos.API.Controllers.Prestador
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PrestadoresExperienciasController : ControllerBase
    {
        private readonly IPrestadorExperienciaService _service;

        public PrestadoresExperienciasController(IPrestadorExperienciaService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PrestadorExperienciaResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PrestadorExperienciaResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Experiência não encontrada." });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PrestadorExperienciaResponseDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreatePrestadorExperienciaDTO request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PrestadorExperienciaResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdatePrestadorExperienciaDTO request)
        {
            var updated = await _service.UpdateAsync(id, request);
            if (updated == null) return NotFound(new { message = "Experiência não encontrada." });
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Experiência não encontrada." });
            return NoContent();
        }
    }
}
