using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.Mecanico;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.Mecanico
{
    public class PrestadorRepository : Repository<Prestador>, IPrestadorRepository
    {
        public PrestadorRepository(OficinaContext context) : base(context) { }

        public override async Task<List<Prestador>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Pessoa).Include(x => x.Mecanico).Include(x => x.Atividades).ThenInclude(x => x.AtividadeProfissional).Include(x => x.Disponibilidades).Include(x => x.Experiencias).Include(x => x.Certificacoes).AsNoTracking().ToListAsync();
        }

        public override async Task<Prestador?> GetByIdAsync(long id)
        {
            return await _dbSet.Include(x => x.Pessoa).Include(x => x.Mecanico).Include(x => x.Atividades).ThenInclude(x => x.AtividadeProfissional).Include(x => x.Disponibilidades).Include(x => x.Experiencias).Include(x => x.Certificacoes).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Prestador?> GetByPessoaIdAsync(long pessoaId) =>
            _dbSet.FirstOrDefaultAsync(x => x.PessoaId == pessoaId);

        public Task<Prestador?> GetByCodigoAsync(string codigo) =>
            _dbSet.FirstOrDefaultAsync(x => x.Codigo == codigo);
    }

    public class AtividadeProfissionalRepository : Repository<AtividadeProfissional>, IAtividadeProfissionalRepository
    {
        public AtividadeProfissionalRepository(OficinaContext context) : base(context) { }
    }
}
