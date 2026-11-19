using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests.Seguranca;
using OficinaMotos.Application.DTOs.Responses.Seguranca;
using OficinaMotos.Application.Interfaces.Seguranca;

namespace OficinaMotos.API.Controllers.Seguranca
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Tags("Seguranca Perfis")]
    [Authorize]
    public class PerfisController : ControllerBase
    {
        private readonly ISegPerfilService _service;

        public PerfisController(ISegPerfilService service) => _service = service;

        [HttpGet]
        [ProducesResponseType(typeof(List<SegPerfilResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(SegPerfilResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Perfil não encontrado." });
            return Ok(item);
        }

        [HttpGet("{id:long}/permissoes")]
        [ProducesResponseType(typeof(SegPerfilResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetComPermissoes(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Perfil não encontrado." });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SegPerfilResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSegPerfilDTO request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(SegPerfilResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateSegPerfilDTO request)
        {
            var updated = await _service.UpdateAsync(id, request);
            if (updated == null) return NotFound(new { message = "Perfil não encontrado." });
            return Ok(updated);
        }

        [HttpPut("{id:long}/permissoes")]
        [ProducesResponseType(typeof(SegPerfilResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetPermissoes(long id, [FromBody] SetPerfilPermissoesDTO request)
        {
            var result = await _service.SetPermissoesAsync(id, request);
            if (result == null) return NotFound(new { message = "Perfil não encontrado." });
            return Ok(result);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Perfil não encontrado." });
            return NoContent();
        }
    }
}
