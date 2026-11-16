using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OficinaMotos.Application.DTOs.Responses.Dashboard;
using OficinaMotos.Application.Interfaces.Dashboard;
using System.Threading.Tasks;

namespace OficinaMotos.API.Controllers.Dashboard
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("kpis")]
        [ProducesResponseType(typeof(DashboardKpisResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetKpis([FromQuery] string? periodo = null)
        {
            var result = await _dashboardService.GetKpisAsync(periodo);
            return Ok(result);
        }
    }
}
