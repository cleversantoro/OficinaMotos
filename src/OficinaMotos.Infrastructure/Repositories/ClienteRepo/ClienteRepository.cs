using Microsoft.EntityFrameworkCore;
using OficinaMotos.Application.DTOs.Responses.Cliente;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.ClienteRepo;
using OficinaMotos.Infrastructure.Context;
using ClienteEntity = OficinaMotos.Domain.Entities.Cliente;

namespace OficinaMotos.Infrastructure.Repositories.Cliente
{
    public class ClienteRepository : Repository<ClienteEntity>, IClienteRepository
    {
        public ClienteRepository(OficinaContext context) : base(context)
        {
        }

        public override async Task<List<ClienteEntity>> GetAllAsync()
        {
            // Carrega todas as dependencias para evitar listas vazias no retorno da API
            return await _dbSet
                .Include(c => c.Pessoa)
                .Include(c => c.Pessoa)
                .Include(c => c.Origem)
                .Include(c => c.Anexos)
                .Include(c => c.Contatos)
                .Include(c => c.Documentos)
                .Include(c => c.Enderecos)
                .Include(c => c.Indicacoes)
                .Include(c => c.ConsentimentosLgpd)
                .Include(c => c.Veiculos)
                .Include(c => c.OrdensServico)
                .Include(c => c.PessoaFisica)
                .Include(c => c.PessoaJuridica)
                .Include(c => c.Financeiro)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<ClienteEntity?> GetByIdAsync(long id)
        {
            return await _dbSet
                .Include(c => c.Origem)
                .Include(c => c.Anexos)
                .Include(c => c.Contatos)
                .Include(c => c.Documentos)
                .Include(c => c.Enderecos)
                .Include(c => c.Indicacoes)
                .Include(c => c.ConsentimentosLgpd)
                .Include(c => c.Veiculos)
                .Include(c => c.OrdensServico)
                .Include(c => c.PessoaFisica)
                .Include(c => c.PessoaJuridica)
                .Include(c => c.Financeiro)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ClienteEntity?> GetByCpfAsync(string cpf)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Documento == cpf);
        }

        public async Task<ClienteEntity?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<List<ClienteEntity>> GetAllForTableAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
