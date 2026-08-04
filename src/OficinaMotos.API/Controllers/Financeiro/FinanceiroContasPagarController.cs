using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests.Financeiro;
using OficinaMotos.Application.DTOs.Responses.Financeiro;
using OficinaMotos.Application.Interfaces.Financeiro;

namespace OficinaMotos.API.Controllers.Financeiro
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class FinanceiroContasPagarController : ControllerBase
    {
        private readonly IFinanceiroContaPagarService _service;

        public FinanceiroContasPagarController(IFinanceiroContaPagarService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<FinanceiroContaPagarResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FinanceiroContaPagarResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { message = "Conta a pagar nao encontrada." });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FinanceiroContaPagarResponseDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateFinanceiroContaPagarDTO request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Gerente,Financeiro")]
        [ProducesResponseType(typeof(FinanceiroContaPagarResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateFinanceiroContaPagarDTO request)
        {
            var updated = await _service.UpdateAsync(id, request);
            if (updated == null) return NotFound(new { message = "Conta a pagar nao encontrada." });
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador,Gerente,Financeiro")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Conta a pagar nao encontrada." });
            return NoContent();
        }
    }
}
