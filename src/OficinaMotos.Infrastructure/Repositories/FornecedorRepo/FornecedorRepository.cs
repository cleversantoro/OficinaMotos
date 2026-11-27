using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.FornecedorRepo;
using OficinaMotos.Infrastructure.Context;
using OficinaMotos.Infrastructure.Repositories.PessoaRepo;
using FornecedorEntity = OficinaMotos.Domain.Entities.Fornecedor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Infrastructure.Repositories.FornecedorRepo
{
    public class FornecedorRepository : Repository<FornecedorEntity>, IFornecedorRepository
    {
        public FornecedorRepository(OficinaContext context) : base(context)
        {
        }

        public override async Task<List<FornecedorEntity>> GetAllAsync()
        {
            var fornecedores = await _dbSet
                .Include(f => f.Pessoa)
                    .ThenInclude(p => p!.Contatos)
                .Include(f => f.Pessoa)
                    .ThenInclude(p => p!.Enderecos)
                .Include(f => f.Pessoa)
                    .ThenInclude(p => p!.Documentos)
                .Include(f => f.Pessoa)
                    .ThenInclude(p => p!.Anexos)
                .Include(f => f.Pessoa)
                    .ThenInclude(p => p!.PessoaFisica)
                .Include(f => f.Pessoa)
                    .ThenInclude(p => p!.PessoaJuridica)
                .Include(f => f.SegmentoPrincipal)
                .Include(f => f.Segmentos)
                .Include(f => f.Representantes)
                .Include(f => f.Bancos)
                .Include(f => f.Certificacoes)
                .Include(f => f.Avaliacoes)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            foreach (var fornecedor in fornecedores)
            {
                PessoaQueryHelper.AplicarRegraTipoPessoa(fornecedor.Pessoa);
            }

            return fornecedores;
        }

        public override async Task<FornecedorEntity?> GetByIdAsync(long id)
        {
            var fornecedor = await _dbSet
                .Include(f => f.Pessoa)
                    .ThenInclude(p => p!.Contatos)
                .Include(f => f.Pessoa)
                    .ThenInclude(p => p!.Enderecos)
                .Include(f => f.Pessoa)
                    .ThenInclude(p => p!.Documentos)
                .Include(f => f.Pessoa)
                    .ThenInclude(p => p!.Anexos)
                .Include(f => f.Pessoa)
                    .ThenInclude(p => p!.PessoaFisica)
                .Include(f => f.Pessoa)
                    .ThenInclude(p => p!.PessoaJuridica)
                .Include(f => f.SegmentoPrincipal)
                .Include(f => f.Segmentos)
                .Include(f => f.Representantes)
                .Include(f => f.Bancos)
                .Include(f => f.Certificacoes)
                .Include(f => f.Avaliacoes)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(f => f.Id == id);

            PessoaQueryHelper.AplicarRegraTipoPessoa(fornecedor?.Pessoa);

            return fornecedor;
        }
    }
}
