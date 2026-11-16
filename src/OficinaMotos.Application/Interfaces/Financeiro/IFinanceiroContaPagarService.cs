using OficinaMotos.Application.DTOs.Requests.Financeiro;
using OficinaMotos.Application.DTOs.Responses.Financeiro;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Financeiro
{
    public interface IFinanceiroContaPagarService
    {
        Task<List<FinanceiroContaPagarResponseDTO>> GetAllAsync();
        Task<FinanceiroContaPagarResponseDTO?> GetByIdAsync(long id);
        Task<FinanceiroContaPagarResponseDTO> CreateAsync(CreateFinanceiroContaPagarDTO request);
        Task<FinanceiroContaPagarResponseDTO?> UpdateAsync(long id, UpdateFinanceiroContaPagarDTO request);
        Task<bool> DeleteAsync(long id);
        Task<ContaPagarPagamentoResponseDTO> RegistrarBaixaAsync(CreateContaPagarPagamentoDTO request);
    }
}
