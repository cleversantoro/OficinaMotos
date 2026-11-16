using OficinaMotos.Application.DTOs.Requests.Financeiro;
using OficinaMotos.Application.DTOs.Responses.Financeiro;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.Financeiro
{
    public interface IFinanceiroContaReceberService
    {
        Task<List<FinanceiroContaReceberResponseDTO>> GetAllAsync();
        Task<FinanceiroContaReceberResponseDTO?> GetByIdAsync(long id);
        Task<FinanceiroContaReceberResponseDTO> CreateAsync(CreateFinanceiroContaReceberDTO request);
        Task<FinanceiroContaReceberResponseDTO?> UpdateAsync(long id, UpdateFinanceiroContaReceberDTO request);
        Task<bool> DeleteAsync(long id);
        Task<ContaReceberPagamentoResponseDTO> RegistrarBaixaAsync(CreateContaReceberPagamentoDTO request);
    }
}
