using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;
using OficinaMotos.Infrastructure.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Infrastructure.Repositories.MecanicoRepo
{
    public class MecanicoRepository : Repository<Mecanico>, IMecanicoRepository
    {
        public MecanicoRepository(OficinaContext context) : base(context)
        {
        }

        public override async Task<List<Mecanico>> GetAllAsync()
        {
            return await _dbSet
                .Include(m => m.EspecialidadePrincipal)
                .Include(m => m.Certificacoes)
                .Include(m => m.Contatos)
                .Include(m => m.Disponibilidades)
                .Include(m => m.Documentos)
                .Include(m => m.Enderecos)
                .Include(m => m.Especialidades)
                .Include(m => m.Experiencias)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<Mecanico?> GetByIdAsync(long id)
        {
            return await _dbSet
                .Include(m => m.EspecialidadePrincipal)
                .Include(m => m.Certificacoes)
                .Include(m => m.Contatos)
                .Include(m => m.Disponibilidades)
                .Include(m => m.Documentos)
                .Include(m => m.Enderecos)
                .Include(m => m.Especialidades)
                .Include(m => m.Experiencias)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
