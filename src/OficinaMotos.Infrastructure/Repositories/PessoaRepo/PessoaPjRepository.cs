using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PessoaRepo;
using OficinaMotos.Infrastructure.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Infrastructure.Repositories.PessoaRepo
{
    public class PessoaPjRepository : IPessoaPjRepository
    {
        private readonly OficinaContext _context;

        public PessoaPjRepository(OficinaContext context)
        {
            _context = context;
        }

        public async Task<List<PessoaPj>> GetAllAsync()
        {
            return await _context.PessoasPj.AsNoTracking().ToListAsync();
        }

        public async Task<PessoaPj?> GetByPessoaIdAsync(long pessoaId)
        {
            return await _context.PessoasPj.FirstOrDefaultAsync(x => x.PessoaId == pessoaId);
        }

        public async Task<PessoaPj> AddAsync(PessoaPj entity)
        {
            await _context.PessoasPj.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(PessoaPj entity)
        {
            _context.PessoasPj.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long pessoaId)
        {
            var entity = await _context.PessoasPj.FirstOrDefaultAsync(x => x.PessoaId == pessoaId);
            if (entity != null)
            {
                _context.PessoasPj.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(long pessoaId)
        {
            return await _context.PessoasPj.AnyAsync(x => x.PessoaId == pessoaId);
        }
    }
}
