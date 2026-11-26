using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests;
using OficinaMotos.Application.DTOs.Responses;
using OficinaMotos.Application.Interfaces.Cliente;

namespace OficinaMotos.API.Controllers.Pessoas
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClienteDocumentosController : ControllerBase
    {
        private readonly IClienteDocumentoService _service;

        public ClienteDocumentosController(IClienteDocumentoService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ClienteDocumentoResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClienteDocumentoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Documento nao encontrado." });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ClienteDocumentoResponseDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateClienteDocumentoDTO request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ClienteDocumentoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateClienteDocumentoDTO request)
        {
            var updated = await _service.UpdateAsync(id, request);
            if (updated == null) return NotFound(new { message = "Documento nao encontrado." });
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Documento nao encontrado." });
            return NoContent();
        }
    }
}
