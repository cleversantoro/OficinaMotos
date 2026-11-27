using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.ClienteRepo;
using OficinaMotos.Infrastructure.Context;
using OficinaMotos.Infrastructure.Repositories.PessoaRepo;

namespace OficinaMotos.Infrastructure.Repositories.ClienteRepo
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(OficinaContext context) : base(context)
        {
        }

        public override async Task<List<Cliente>> GetAllAsync()
        {
            // Carrega todas as dependencias para evitar listas vazias no retorno da API
            var clientes = await _dbSet
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.Contatos)
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.Enderecos)
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.Documentos)
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.Anexos)
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.PessoaFisica)
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.PessoaJuridica)
                .Include(c => c.Origem)
                .Include(c => c.Financeiro)
                .Include(c => c.Indicacoes)
                .Include(c => c.ConsentimentosLgpd)
                .Include(c => c.Veiculos)
                .Include(c => c.OrdensServico)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            foreach (var cliente in clientes)
            {
                PessoaQueryHelper.AplicarRegraTipoPessoa(cliente.Pessoa);
            }

            return clientes;
        }

        public override async Task<Cliente?> GetByIdAsync(long id)
        {
            var cliente = await _dbSet
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.Contatos)
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.Enderecos)
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.Documentos)
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.Anexos)
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.PessoaFisica)
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.PessoaJuridica)
                .Include(c => c.Origem)
                .Include(c => c.Financeiro)
                .Include(c => c.Indicacoes)
                .Include(c => c.ConsentimentosLgpd)
                .Include(c => c.Veiculos)
                .Include(c => c.OrdensServico)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id);

            PessoaQueryHelper.AplicarRegraTipoPessoa(cliente?.Pessoa);

            return cliente;
        }

        public async Task<List<Cliente>> GetAllForTableAsync()
        {
            return await _dbSet
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.Contatos)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();
        }
    }
}
