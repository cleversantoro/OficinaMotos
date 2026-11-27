using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PessoaRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.PessoaRepo
{
    public class PessoaRepository : Repository<Pessoa>, IPessoaRepository
    {
        public PessoaRepository(OficinaContext context) : base(context)
        {
        }

        public override async Task<List<Pessoa>> GetAllAsync()
        {
            var pessoas = await _dbSet
                .Include(x => x.PessoaFisica)
                .Include(x => x.PessoaJuridica)
                .Include(x => x.Contatos)
                .Include(x => x.Enderecos)
                .Include(x => x.Documentos)
                .Include(x => x.Anexos)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            foreach (var pessoa in pessoas)
            {
                PessoaQueryHelper.AplicarRegraTipoPessoa(pessoa);
            }

            return pessoas;
        }

        public override async Task<Pessoa?> GetByIdAsync(long id)
        {
            var pessoa = await _dbSet
                .Include(x => x.PessoaFisica)
                .Include(x => x.PessoaJuridica)
                .Include(x => x.Contatos)
                .Include(x => x.Enderecos)
                .Include(x => x.Documentos)
                .Include(x => x.Anexos)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == id);

            PessoaQueryHelper.AplicarRegraTipoPessoa(pessoa);

            return pessoa;
        }

        public async Task<Pessoa?> GetByDocumentoAsync(string documento)
        {
            var digits = new string(documento.Where(char.IsDigit).ToArray());
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Documento == documento || x.Documento == digits);
        }

        public async Task<long> GetNextCodigoAsync()
        {
            var max = await _dbSet.MaxAsync(x => (long?)x.Codigo);
            return (max ?? 0) + 1;
        }
    }
}
