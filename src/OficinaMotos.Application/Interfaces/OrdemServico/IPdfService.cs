using System;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Interfaces.OrdemServico
{
    public interface IPdfService
    {
        Task<byte[]?> GerarOrdemServicoPdfAsync(long id);
        byte[]? GerarOrdemServicoPdf(long id);
        byte[]? GerarOrdemServicoPdf(Guid id);
    }
}
