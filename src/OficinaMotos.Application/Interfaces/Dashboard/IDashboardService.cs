using OficinaMotos.Application.DTOs.Responses.Dashboard;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Dashboard
{
    public interface IDashboardService
    {
        Task<DashboardKpisResponseDTO> GetKpisAsync(string? periodo = null);
    }
}
