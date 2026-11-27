using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PessoaRepo;
using OficinaMotos.Infrastructure.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Infrastructure.Repositories.PessoaRepo
{
    public class PessoaPfRepository : IPessoaPfRepository
    {
        private readonly OficinaContext _context;

        public PessoaPfRepository(OficinaContext context)
        {
            _context = context;
        }

        public async Task<List<PessoaPf>> GetAllAsync()
        {
            return await _context.PessoasPf.AsNoTracking().ToListAsync();
        }

        public async Task<PessoaPf?> GetByPessoaIdAsync(long pessoaId)
        {
            return await _context.PessoasPf.FirstOrDefaultAsync(x => x.PessoaId == pessoaId);
        }

        public async Task<PessoaPf> AddAsync(PessoaPf entity)
        {
            await _context.PessoasPf.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(PessoaPf entity)
        {
            _context.PessoasPf.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long pessoaId)
        {
            var entity = await _context.PessoasPf.FirstOrDefaultAsync(x => x.PessoaId == pessoaId);
            if (entity != null)
            {
                _context.PessoasPf.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(long pessoaId)
        {
            return await _context.PessoasPf.AnyAsync(x => x.PessoaId == pessoaId);
        }
    }
}
