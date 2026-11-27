using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Infrastructure.Context;
using System.Threading.Tasks;
using System.Collections.Generic;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;

namespace OficinaMotos.Infrastructure.Repositories.OrdemServicoRepo
{
    public class OrdemServicoRepository : Repository<OrdemServico>, IOrdemServicoRepository
    {
        public OrdemServicoRepository(OficinaContext context) : base(context)
        {
        }

        public override async Task<List<OrdemServico>> GetAllAsync()
        {
            return await _dbSet
                .Include(o => o.Cliente)
                .Include(o => o.Veiculo)
                    .ThenInclude(v => v!.Modelo)
                .Include(o => o.Anexos)
                .Include(o => o.Avaliacoes)
                .Include(o => o.Checklists)
                .Include(o => o.Itens)
                    .ThenInclude(i => i.Prestador)
                .Include(o => o.Itens)
                    .ThenInclude(i => i.AtividadeProfissional)
                .Include(o => o.ListaObservacoes)
                .Include(o => o.Historico)
                .Include(o => o.Pagamentos)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();
        }

        public override async Task<OrdemServico?> GetByIdAsync(long id)
        {
            return await _dbSet
                .Include(o => o.Cliente)
                .Include(o => o.Veiculo)
                    .ThenInclude(v => v!.Modelo)
                .Include(o => o.Anexos)
                .Include(o => o.Avaliacoes)
                .Include(o => o.Checklists)
                .Include(o => o.Itens)
                    .ThenInclude(i => i.Prestador)
                .Include(o => o.Itens)
                    .ThenInclude(i => i.AtividadeProfissional)
                .Include(o => o.ListaObservacoes)
                .Include(o => o.Historico)
                .Include(o => o.Pagamentos)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
