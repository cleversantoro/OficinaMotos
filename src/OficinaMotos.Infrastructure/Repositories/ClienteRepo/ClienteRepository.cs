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
                    .ThenInclude(p => p!.Contatos)
                .Include(c => c.Origem)
                .Include(c => c.Veiculos)
                .Include(c => c.OrdensServico)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<ClienteEntity?> GetByIdAsync(long id)
        {
            return await _dbSet
                .Include(c => c.Pessoa)
                    .ThenInclude(p => p!.Contatos)
                .Include(c => c.Origem)
                .Include(c => c.Veiculos)
                .Include(c => c.OrdensServico)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ClienteEntity?> GetByCpfAsync(string cpf)
        {
            var pessoa = await _context.Pessoas.AsNoTracking().FirstOrDefaultAsync(p => p.Documento == cpf);
            if (pessoa == null) return null;
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.PessoaId == pessoa.Id);
        }

        public async Task<ClienteEntity?> GetByEmailAsync(string email)
        {
            var pessoaContato = await _context.PessoaContatos.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Tipo == "EMAIL" && c.Valor == email);
            if (pessoaContato == null) return null;
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.PessoaId == pessoaContato.PessoaId);
        }

        public async Task SyncPessoaContatosAsync(long pessoaId, string? email, string? telefone)
        {
            var existing = await _context.PessoaContatos
                .Where(c => c.PessoaId == pessoaId &&
                    (c.Tipo == "EMAIL" || c.Tipo == "TELEFONE" || c.Tipo == "CELULAR"))
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(email))
            {
                var item = existing.FirstOrDefault(c => c.Tipo == "EMAIL");
                if (item == null)
                {
                    _context.PessoaContatos.Add(new PessoaContato
                    {
                        PessoaId = pessoaId,
                        Tipo = "EMAIL",
                        Valor = email,
                        Principal = true
                    });
                }
                else item.Valor = email;
            }

            if (!string.IsNullOrWhiteSpace(telefone))
            {
                var item = existing.FirstOrDefault(c => c.Tipo == "TELEFONE" || c.Tipo == "CELULAR");
                if (item == null)
                {
                    _context.PessoaContatos.Add(new PessoaContato
                    {
                        PessoaId = pessoaId,
                        Tipo = "TELEFONE",
                        Valor = telefone,
                        Principal = true
                    });
                }
                else item.Valor = telefone;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<ClienteEntity>> GetAllForTableAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
