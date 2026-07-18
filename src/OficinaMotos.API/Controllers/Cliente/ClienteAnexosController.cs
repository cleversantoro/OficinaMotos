using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests;
using OficinaMotos.Application.DTOs.Responses;
using OficinaMotos.Application.Interfaces.Cliente;

namespace OficinaMotos.API.Controllers.Cliente
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClienteAnexosController : ControllerBase
    {
        private readonly IClienteAnexoService _service;
        private readonly IWebHostEnvironment _env;

        public ClienteAnexosController(IClienteAnexoService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ClienteAnexoResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] long? clienteId = null)
        {
            var result = await _service.GetAllAsync();
            if (clienteId.HasValue)
                result = result.Where(a => a.ClienteId == clienteId.Value).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClienteAnexoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Anexo nao encontrado." });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ClienteAnexoResponseDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateClienteAnexoDTO request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPost("upload")]
        [ProducesResponseType(typeof(ClienteAnexoResponseDTO), StatusCodes.Status201Created)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] long clienteId, IFormFile file, [FromForm] string? observacao = null)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Nenhum arquivo enviado." });

            const long maxSize = 10 * 1024 * 1024; // 10 MB
            if (file.Length > maxSize)
                return BadRequest(new { message = "Arquivo excede o limite de 10 MB." });

            var allowedTypes = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx", ".xls", ".xlsx" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedTypes.Contains(ext))
                return BadRequest(new { message = "Tipo de arquivo não permitido." });

            var uploadsDir = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "clientes", clienteId.ToString());
            Directory.CreateDirectory(uploadsDir);

            var uniqueName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsDir, uniqueName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativeUrl = $"/uploads/clientes/{clienteId}/{uniqueName}";

            var dto = new CreateClienteAnexoDTO
            {
                ClienteId = clienteId,
                Nome = file.FileName,
                Tipo = ext.TrimStart('.').ToUpperInvariant(),
                Url = relativeUrl,
                Observacao = observacao
            };

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ClienteAnexoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateClienteAnexoDTO request)
        {
            var updated = await _service.UpdateAsync(id, request);
            if (updated == null) return NotFound(new { message = "Anexo nao encontrado." });
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Anexo nao encontrado." });
            return NoContent();
        }
    }
}


//        [HttpGet("{id}")]
//        [ProducesResponseType(typeof(ClienteAnexoResponseDTO), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> GetById(long id)
//        {
//            var item = await _service.GetByIdAsync(id);
//            if (item == null) return NotFound(new { message = "Anexo nao encontrado." });
//            return Ok(item);
//        }

//        [HttpPost]
//        [ProducesResponseType(typeof(ClienteAnexoResponseDTO), StatusCodes.Status201Created)]
//        public async Task<IActionResult> Create([FromBody] CreateClienteAnexoDTO request)
//        {
//            var created = await _service.CreateAsync(request);
//            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
//        }

//        [HttpPut("{id}")]
//        [ProducesResponseType(typeof(ClienteAnexoResponseDTO), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> Update(long id, [FromBody] UpdateClienteAnexoDTO request)
//        {
//            var updated = await _service.UpdateAsync(id, request);
//            if (updated == null) return NotFound(new { message = "Anexo nao encontrado." });
//            return Ok(updated);
//        }

//        [HttpDelete("{id}")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public async Task<IActionResult> Delete(long id)
//        {
//            var deleted = await _service.DeleteAsync(id);
//            if (!deleted) return NotFound(new { message = "Anexo nao encontrado." });
//            return NoContent();
//        }
//    }
//}
