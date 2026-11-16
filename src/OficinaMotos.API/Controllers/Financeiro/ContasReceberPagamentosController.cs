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
    public class ContasReceberPagamentosController : ControllerBase
    {
        private readonly IFinanceiroContaReceberService _contaReceberService;

        public ContasReceberPagamentosController(IFinanceiroContaReceberService contaReceberService)
        {
            _contaReceberService = contaReceberService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ContaReceberPagamentoResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateContaReceberPagamentoDTO request)
        {
            try
            {
                var result = await _contaReceberService.RegistrarBaixaAsync(request);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
