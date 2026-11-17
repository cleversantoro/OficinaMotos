using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServicoDTO;
using OficinaMotos.Application.Interfaces.OrdemServico;

namespace OficinaMotos.API.Controllers.OrdemServico
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class OrdemServicosController : ControllerBase
    {
        private readonly IOrdemServicoService _service;
        private readonly IPdfService _pdfService;

        public OrdemServicosController(IOrdemServicoService service, IPdfService pdfService)
        {
            _service = service;
            _pdfService = pdfService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<OrdemServicoResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrdemServicoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Ordem de serviço não encontrada." });
            return Ok(item);
        }

        [HttpGet("{id:long}/pdf")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPdf(long id)
        {
            var pdfBytes = await _pdfService.GerarOrdemServicoPdfAsync(id);
            if (pdfBytes == null || pdfBytes.Length == 0)
            {
                return NotFound(new { message = "Ordem de serviço não encontrada." });
            }

            return File(pdfBytes, "application/pdf", $"os-{id}.pdf");
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrdemServicoResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateOrdemServicoDTO request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Gerente,Recepcionista,Mecânico")]
        [ProducesResponseType(typeof(OrdemServicoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateOrdemServicoDTO request)
        {
            var updated = await _service.UpdateAsync(id, request);
            if (updated == null) return NotFound(new { message = "Ordem de serviço não encontrada." });
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador,Gerente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Ordem de serviço não encontrada." });
            return NoContent();
        }
    }
}
