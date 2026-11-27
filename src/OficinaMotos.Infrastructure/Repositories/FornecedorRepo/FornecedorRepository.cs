using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.Fornecedor;
using OficinaMotos.Infrastructure.Context;
using FornecedorEntity = OficinaMotos.Domain.Entities.Fornecedor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Infrastructure.Repositories.Fornecedor
{
    public class FornecedorRepository : Repository<FornecedorEntity>, IFornecedorRepository
    {
        public FornecedorRepository(OficinaContext context) : base(context)
        {
        }

        public override async Task<List<FornecedorEntity>> GetAllAsync()
        {
            return await _dbSet
                .Include(f => f.Pessoa)
                .Include(f => f.SegmentoPrincipal)
                .Include(f => f.Segmentos)
                .Include(f => f.Representantes)
                .Include(f => f.Bancos)
                .Include(f => f.Certificacoes)
                .Include(f => f.Avaliacoes)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<FornecedorEntity?> GetByIdAsync(long id)
        {
            return await _dbSet
                .Include(f => f.Pessoa)
                .Include(f => f.SegmentoPrincipal)
                .Include(f => f.Segmentos)
                .Include(f => f.Representantes)
                .Include(f => f.Bancos)
                .Include(f => f.Certificacoes)
                .Include(f => f.Avaliacoes)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
