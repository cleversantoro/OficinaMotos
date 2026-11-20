using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests.Fornecedor;
using OficinaMotos.Application.DTOs.Responses.Fornecedor;
using OficinaMotos.Application.Interfaces.Fornecedor;

namespace OficinaMotos.API.Controllers.Fornecedor
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FornecedorSegmentosRelController : ControllerBase
    {
        private readonly IFornecedorSegmentoRelService _service;

        public FornecedorSegmentosRelController(IFornecedorSegmentoRelService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<FornecedorSegmentoRelResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{fornecedorId:long}/{segmentoId:long}")]
        [ProducesResponseType(typeof(FornecedorSegmentoRelResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByKeys(long fornecedorId, long segmentoId)
        {
            var item = await _service.GetByKeysAsync(fornecedorId, segmentoId);
            if (item == null) return NotFound(new { message = "Segmento/fornecedor não encontrado." });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FornecedorSegmentoRelResponseDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateFornecedorSegmentoRelDTO request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetByKeys), new { fornecedorId = created.FornecedorId, segmentoId = created.SegmentoId }, created);
        }

        [HttpDelete("{fornecedorId:long}/{segmentoId:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long fornecedorId, long segmentoId)
        {
            var deleted = await _service.DeleteAsync(fornecedorId, segmentoId);
            if (!deleted) return NotFound(new { message = "Segmento/fornecedor não encontrado." });
            return NoContent();
        }
    }
}
