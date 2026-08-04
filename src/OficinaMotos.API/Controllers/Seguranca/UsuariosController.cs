using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests.Seguranca;
using OficinaMotos.Application.DTOs.Responses.Seguranca;
using OficinaMotos.Application.Interfaces.Seguranca;

namespace OficinaMotos.API.Controllers.Seguranca
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly ISegUsuarioService _service;

        public UsuariosController(ISegUsuarioService service) => _service = service;

        [HttpGet]
        [ProducesResponseType(typeof(List<SegUsuarioTableDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllForTableAsync());

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(SegUsuarioResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Usuário não encontrado." });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SegUsuarioResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateSegUsuarioDTO request)
        {
            try
            {
                var created = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = "Administrador,Gerente")]
        [ProducesResponseType(typeof(SegUsuarioResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateSegUsuarioDTO request)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, request);
                if (updated == null) return NotFound(new { message = "Usuário não encontrado." });
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:long}/senha")]
        [Authorize(Roles = "Administrador,Gerente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarSenha(long id, [FromBody] AlterarSenhaDTO request)
        {
            try
            {
                var ok = await _service.AlterarSenhaAsync(id, request);
                if (!ok) return NotFound(new { message = "Usuário não encontrado." });
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Usuário não encontrado." });
            return NoContent();
        }
    }
}
