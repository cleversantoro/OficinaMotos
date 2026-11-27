using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.SegurancaRepo;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Infrastructure.Repositories.SegurancaRepo
{
    public class SegUsuarioRepository : Repository<SegUsuario>, ISegUsuarioRepository
    {
        public SegUsuarioRepository(OficinaContext context) : base(context) { }

        public override async Task<List<SegUsuario>> GetAllAsync()
            => await _dbSet
                .Include(u => u.UsuariosPerfis.Where(up => up.Ativo))
                    .ThenInclude(up => up.Perfil)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

        public async Task<SegUsuario?> GetByEmailAsync(string email)
            => await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<SegUsuario?> GetByLoginAsync(string login)
            => await _dbSet.FirstOrDefaultAsync(u => u.Login == login);

        public async Task<SegUsuario?> GetByEmailOrLoginAsync(string emailOrLogin)
            => await _dbSet.FirstOrDefaultAsync(u => u.Email == emailOrLogin || u.Login == emailOrLogin);

        public async Task<SegUsuario?> GetWithPerfisAsync(long id)
            => await _dbSet
                .Include(u => u.UsuariosPerfis.Where(up => up.Ativo))
                    .ThenInclude(up => up.Perfil)
                        .ThenInclude(p => p!.PerfisPermissoes)
                            .ThenInclude(pp => pp.Permissao)
                                .ThenInclude(perm => perm!.Modulo)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

        public async Task<bool> EmailExistsAsync(string email, long? excludeId = null)
            => await _dbSet.AnyAsync(u => u.Email == email && (excludeId == null || u.Id != excludeId));

        public async Task<bool> LoginExistsAsync(string login, long? excludeId = null)
            => await _dbSet.AnyAsync(u => u.Login == login && (excludeId == null || u.Id != excludeId));
    }
}
