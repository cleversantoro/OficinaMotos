using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Requests.Cliente;
using OficinaMotos.Application.DTOs.Responses.Cliente;
using OficinaMotos.Application.Interfaces.Cliente;

namespace OficinaMotos.API.Controllers.Cliente
{
    [ApiController]
    [Route("api/v1/[controller]")] // Ex: api/v1/clientes
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        // Injeção de Dependência via Construtor
        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Retorna todos os clientes ativos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ClienteResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteService.GetAllAsync();
            return Ok(clientes);
        }

        /// <summary>
        /// Retorna todos os clientes para tabelas
        /// </summary>
        [HttpGet("table")]
        [ProducesResponseType(typeof(List<ClienteResponseTableDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllForTable()
        {
            var clientes = await _clienteService.GetAllForTableAsync();
            return Ok(clientes);
        }

        /// <summary>
        /// Busca um cliente pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);

            if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado." });
            }

            return Ok(cliente);
        }

        /// <summary>
        /// Cadastra um novo cliente
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateClienteDTO request)
        {
            // A validação (FluentValidation) ocorre automaticamente aqui.
            // Se o request for inválido, o [ApiController] retorna 400 antes mesmo de entrar no método.

            try
            {
                var novoCliente = await _clienteService.CreateAsync(request);

                // Retorna 201 Created e o Header "Location" com a URL para consultar o novo recurso
                return CreatedAtAction(nameof(GetById), new { id = novoCliente.Id }, novoCliente);
            }
            catch (Exception ex)
            {
                // Em um cenário real, o "Global Exception Handler" capturaria isso.
                // Aqui retornamos 400 caso o Service lance erro (ex: CPF duplicado)
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um cliente
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Gerente,Recepcionista")]
        [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateClienteDTO request)
        {
            var updated = await _clienteService.UpdateAsync(id, request);
            if (updated == null) return NotFound(new { message = "Cliente nÆo encontrado." });
            return Ok(updated);
        }

        /// <summary>
        /// Remove um cliente
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _clienteService.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Cliente nÆo encontrado." });
            return NoContent();
        }
    }
}
