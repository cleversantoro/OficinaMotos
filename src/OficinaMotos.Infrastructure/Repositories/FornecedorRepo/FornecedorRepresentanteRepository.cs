using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.FornecedorRepo;
using OficinaMotos.Infrastructure.Context;
using OficinaMotos.Infrastructure.Repositories.PessoaRepo;

namespace OficinaMotos.Infrastructure.Repositories.FornecedorRepo
{
    public class FornecedorRepresentanteRepository : Repository<FornecedorRepresentante>, IFornecedorRepresentanteRepository
    {
        public FornecedorRepresentanteRepository(OficinaContext context) : base(context)
        {
        }

        public override async Task<List<FornecedorRepresentante>> GetAllAsync()
        {
            var representantes = await _dbSet
                .Include(r => r.Pessoa)
                    .ThenInclude(p => p!.Contatos)
                .Include(r => r.Pessoa)
                    .ThenInclude(p => p!.Enderecos)
                .Include(r => r.Pessoa)
                    .ThenInclude(p => p!.Documentos)
                .Include(r => r.Pessoa)
                    .ThenInclude(p => p!.Anexos)
                .Include(r => r.Pessoa)
                    .ThenInclude(p => p!.PessoaFisica)
                .Include(r => r.Pessoa)
                    .ThenInclude(p => p!.PessoaJuridica)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            foreach (var representante in representantes)
            {
                PessoaQueryHelper.AplicarRegraTipoPessoa(representante.Pessoa);
            }

            return representantes;
        }

        public override async Task<FornecedorRepresentante?> GetByIdAsync(long id)
        {
            var representante = await _dbSet
                .Include(r => r.Pessoa)
                    .ThenInclude(p => p!.Contatos)
                .Include(r => r.Pessoa)
                    .ThenInclude(p => p!.Enderecos)
                .Include(r => r.Pessoa)
                    .ThenInclude(p => p!.Documentos)
                .Include(r => r.Pessoa)
                    .ThenInclude(p => p!.Anexos)
                .Include(r => r.Pessoa)
                    .ThenInclude(p => p!.PessoaFisica)
                .Include(r => r.Pessoa)
                    .ThenInclude(p => p!.PessoaJuridica)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(r => r.Id == id);

            PessoaQueryHelper.AplicarRegraTipoPessoa(representante?.Pessoa);

            return representante;
        }
    }
}
