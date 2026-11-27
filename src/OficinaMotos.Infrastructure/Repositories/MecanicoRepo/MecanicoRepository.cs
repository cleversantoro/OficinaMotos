using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;
using OficinaMotos.Infrastructure.Context;
using OficinaMotos.Infrastructure.Repositories.PessoaRepo;
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
            var mecanicos = await _dbSet
                .Include(m => m.Prestador)
                    .ThenInclude(pr => pr!.Pessoa)
                        .ThenInclude(p => p!.Contatos)
                .Include(m => m.Prestador)
                    .ThenInclude(pr => pr!.Pessoa)
                        .ThenInclude(p => p!.Enderecos)
                .Include(m => m.Prestador)
                    .ThenInclude(pr => pr!.Pessoa)
                        .ThenInclude(p => p!.Documentos)
                .Include(m => m.Prestador)
                    .ThenInclude(pr => pr!.Pessoa)
                        .ThenInclude(p => p!.Anexos)
                .Include(m => m.Prestador)
                    .ThenInclude(pr => pr!.Pessoa)
                        .ThenInclude(p => p!.PessoaFisica)
                .Include(m => m.Prestador)
                    .ThenInclude(pr => pr!.Pessoa)
                        .ThenInclude(p => p!.PessoaJuridica)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            foreach (var mecanico in mecanicos)
            {
                PessoaQueryHelper.AplicarRegraTipoPessoa(mecanico.Prestador?.Pessoa);
            }

            return mecanicos;
        }

        public override async Task<Mecanico?> GetByIdAsync(long id)
        {
            var mecanico = await _dbSet
                .Include(m => m.Prestador)
                    .ThenInclude(pr => pr!.Pessoa)
                        .ThenInclude(p => p!.Contatos)
                .Include(m => m.Prestador)
                    .ThenInclude(pr => pr!.Pessoa)
                        .ThenInclude(p => p!.Enderecos)
                .Include(m => m.Prestador)
                    .ThenInclude(pr => pr!.Pessoa)
                        .ThenInclude(p => p!.Documentos)
                .Include(m => m.Prestador)
                    .ThenInclude(pr => pr!.Pessoa)
                        .ThenInclude(p => p!.Anexos)
                .Include(m => m.Prestador)
                    .ThenInclude(pr => pr!.Pessoa)
                        .ThenInclude(p => p!.PessoaFisica)
                .Include(m => m.Prestador)
                    .ThenInclude(pr => pr!.Pessoa)
                        .ThenInclude(p => p!.PessoaJuridica)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(m => m.Id == id);

            PessoaQueryHelper.AplicarRegraTipoPessoa(mecanico?.Prestador?.Pessoa);

            return mecanico;
        }
    }
}
