using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.PrestadorRepo;
using OficinaMotos.Infrastructure.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OficinaMotos.Infrastructure.Repositories.PrestadorRepo
{
    public class PrestadorAtividadeRepository : IPrestadorAtividadeRepository
    {
        private readonly OficinaContext _context;

        public PrestadorAtividadeRepository(OficinaContext context)
        {
            _context = context;
        }

        public async Task<List<PrestadorAtividade>> GetAllAsync()
        {
            return await _context.PrestadoresAtividades
                .Include(x => x.AtividadeProfissional)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PrestadorAtividade?> GetByIdAsync(long prestadorId, long atividadeProfissionalId)
        {
            return await _context.PrestadoresAtividades
                .Include(x => x.AtividadeProfissional)
                .FirstOrDefaultAsync(x => x.PrestadorId == prestadorId && x.AtividadeProfissionalId == atividadeProfissionalId);
        }

        public async Task<PrestadorAtividade> AddAsync(PrestadorAtividade entity)
        {
            await _context.PrestadoresAtividades.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(PrestadorAtividade entity)
        {
            _context.PrestadoresAtividades.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long prestadorId, long atividadeProfissionalId)
        {
            var entity = await _context.PrestadoresAtividades
                .FirstOrDefaultAsync(x => x.PrestadorId == prestadorId && x.AtividadeProfissionalId == atividadeProfissionalId);
            if (entity != null)
            {
                _context.PrestadoresAtividades.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(long prestadorId, long atividadeProfissionalId)
        {
            return await _context.PrestadoresAtividades
                .AnyAsync(x => x.PrestadorId == prestadorId && x.AtividadeProfissionalId == atividadeProfissionalId);
        }
    }
}
