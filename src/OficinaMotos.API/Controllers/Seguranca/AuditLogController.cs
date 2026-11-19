using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Responses.Seguranca;
using OficinaMotos.Application.Interfaces.Seguranca;

namespace OficinaMotos.API.Controllers.Seguranca
{
    [ApiController]
    [Route("api/v1/audit-log")]
    [Tags("Seguranca Audit")]
    [Authorize]
    public class AuditLogController : ControllerBase
    {
        private readonly ISegAuditLogService _service;

        public AuditLogController(ISegAuditLogService service) => _service = service;

        [HttpGet]
        [ProducesResponseType(typeof(SegAuditLogPagedResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] long? usuarioId = null,
            [FromQuery] string? acao = null,
            [FromQuery] string? modulo = null,
            [FromQuery] DateTime? de = null,
            [FromQuery] DateTime? ate = null)
        {
            var result = await _service.GetPagedAsync(page, pageSize, usuarioId, acao, modulo, de, ate);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(SegAuditLogResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Registro de auditoria não encontrado." });
            return Ok(item);
        }

        [HttpGet("usuario/{usuarioId:long}")]
        [ProducesResponseType(typeof(List<SegAuditLogResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUsuario(long usuarioId, [FromQuery] int take = 50)
            => Ok(await _service.GetByUsuarioAsync(usuarioId, take));
    }
}
