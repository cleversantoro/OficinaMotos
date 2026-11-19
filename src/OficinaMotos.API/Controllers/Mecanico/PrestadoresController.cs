using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests.Mecanico;
using OficinaMotos.Application.DTOs.Responses.Mecanico;
using OficinaMotos.Application.Interfaces.Mecanico;

namespace OficinaMotos.API.Controllers.Mecanico
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class PrestadoresController : ControllerBase
    {
        private readonly IPrestadorService _service;

        public PrestadoresController(IPrestadorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(PrestadorResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item == null ? NotFound(new { message = "Prestador não encontrado." }) : Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PrestadorResponseDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreatePrestadorDTO request)
        {
            var item = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdatePrestadorDTO request)
        {
            var item = await _service.UpdateAsync(id, request);
            return item == null ? NotFound(new { message = "Prestador não encontrado." }) : Ok(item);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            return await _service.DeleteAsync(id) ? NoContent() : NotFound(new { message = "Prestador não encontrado." });
        }
    }
}
