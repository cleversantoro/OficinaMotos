using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PrestadorRepo;
using OficinaMotos.Infrastructure.Context;
using OficinaMotos.Infrastructure.Repositories.PessoaRepo;

namespace OficinaMotos.Infrastructure.Repositories.PrestadorRepo
{
    public class PrestadorRepository : Repository<Prestador>, IPrestadorRepository
    {
        public PrestadorRepository(OficinaContext context) : base(context) { }

        public override async Task<List<Prestador>> GetAllAsync()
        {
            var prestadores = await _dbSet
                .Include(x => x.Pessoa)
                    .ThenInclude(p => p!.Contatos)
                .Include(x => x.Pessoa)
                    .ThenInclude(p => p!.Enderecos)
                .Include(x => x.Pessoa)
                    .ThenInclude(p => p!.Documentos)
                .Include(x => x.Pessoa)
                    .ThenInclude(p => p!.Anexos)
                .Include(x => x.Pessoa)
                    .ThenInclude(p => p!.PessoaFisica)
                .Include(x => x.Pessoa)
                    .ThenInclude(p => p!.PessoaJuridica)
                .Include(x => x.Mecanico)
                .Include(x => x.Atividades)
                    .ThenInclude(x => x.AtividadeProfissional)
                .Include(x => x.Disponibilidades)
                .Include(x => x.Experiencias)
                .Include(x => x.Certificacoes)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            foreach (var prestador in prestadores)
            {
                PessoaQueryHelper.AplicarRegraTipoPessoa(prestador.Pessoa);
            }

            return prestadores;
        }

        public override async Task<Prestador?> GetByIdAsync(long id)
        {
            var prestador = await _dbSet
                .Include(x => x.Pessoa)
                    .ThenInclude(p => p!.Contatos)
                .Include(x => x.Pessoa)
                    .ThenInclude(p => p!.Enderecos)
                .Include(x => x.Pessoa)
                    .ThenInclude(p => p!.Documentos)
                .Include(x => x.Pessoa)
                    .ThenInclude(p => p!.Anexos)
                .Include(x => x.Pessoa)
                    .ThenInclude(p => p!.PessoaFisica)
                .Include(x => x.Pessoa)
                    .ThenInclude(p => p!.PessoaJuridica)
                .Include(x => x.Mecanico)
                .Include(x => x.Atividades)
                    .ThenInclude(x => x.AtividadeProfissional)
                .Include(x => x.Disponibilidades)
                .Include(x => x.Experiencias)
                .Include(x => x.Certificacoes)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == id);

            PessoaQueryHelper.AplicarRegraTipoPessoa(prestador?.Pessoa);

            return prestador;
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
