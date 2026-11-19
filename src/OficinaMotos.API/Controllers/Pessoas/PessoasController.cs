using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using OficinaMotos.Application.Interfaces.Pessoas;

namespace OficinaMotos.API.Controllers.Pessoas
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class PessoasController : ControllerBase
    {
        private readonly IPessoaService _service;

        public PessoasController(IPessoaService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PessoaResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(PessoaResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var pessoa = await _service.GetByIdAsync(id);
            return pessoa == null ? NotFound(new { message = "Pessoa não encontrada." }) : Ok(pessoa);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PessoaResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePessoaDTO request)
        {
            var pessoa = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = pessoa.Id }, pessoa);
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(PessoaResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdatePessoaDTO request)
        {
            var pessoa = await _service.UpdateAsync(id, request);
            return pessoa == null ? NotFound(new { message = "Pessoa não encontrada." }) : Ok(pessoa);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound(new { message = "Pessoa não encontrada." });
        }
    }
}
