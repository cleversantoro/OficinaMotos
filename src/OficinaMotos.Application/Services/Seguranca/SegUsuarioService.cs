using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Seguranca;
using OficinaMotos.Application.DTOs.Responses.Seguranca;
using OficinaMotos.Application.Interfaces.Seguranca;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.SegurancaRepo;

namespace OficinaMotos.Application.Services.Seguranca
{
    public class SegUsuarioService : ISegUsuarioService
    {
        private readonly ISegUsuarioRepository _usuarioRepository;
        private readonly ISegUsuarioPerfilRepository _usuarioPerfilRepository;
        private readonly IMapper _mapper;

        public SegUsuarioService(
            ISegUsuarioRepository usuarioRepository,
            ISegUsuarioPerfilRepository usuarioPerfilRepository,
            IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioPerfilRepository = usuarioPerfilRepository;
            _mapper = mapper;
        }

        public async Task<List<SegUsuarioResponseDTO>> GetAllAsync()
        {
            var items = await _usuarioRepository.GetAllAsync();
            return _mapper.Map<List<SegUsuarioResponseDTO>>(items);
        }

        public async Task<List<SegUsuarioTableDTO>> GetAllForTableAsync()
        {
            var items = await _usuarioRepository.GetAllAsync();
            return _mapper.Map<List<SegUsuarioTableDTO>>(items);
        }

        public async Task<SegUsuarioResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _usuarioRepository.GetWithPerfisAsync(id);
            return item == null ? null : _mapper.Map<SegUsuarioResponseDTO>(item);
        }

        public async Task<SegUsuarioResponseDTO> CreateAsync(CreateSegUsuarioDTO request, long? criadoPorId = null)
        {
            if (await _usuarioRepository.EmailExistsAsync(request.Email))
                throw new InvalidOperationException("Já existe um usuário com este e-mail.");

            if (await _usuarioRepository.LoginExistsAsync(request.Login))
                throw new InvalidOperationException("Já existe um usuário com este login.");

            var entity = _mapper.Map<SegUsuario>(request);
            entity.Senha = BCrypt.Net.BCrypt.HashPassword(request.Senha, workFactor: 12);
            entity.CriadoPor = criadoPorId;

            var created = await _usuarioRepository.AddAsync(entity);

            if (request.PerfilId > 0)
            {
                var vinculo = new SegUsuarioPerfil { UsuarioId = created.Id, PerfilId = request.PerfilId };
                await _usuarioPerfilRepository.AddAsync(vinculo);
            }

            var withPerfis = await _usuarioRepository.GetWithPerfisAsync(created.Id);
            return _mapper.Map<SegUsuarioResponseDTO>(withPerfis!);
        }

        public async Task<SegUsuarioResponseDTO?> UpdateAsync(long id, UpdateSegUsuarioDTO request)
        {
            var entity = await _usuarioRepository.GetByIdAsync(id);
            if (entity == null) return null;

            if (await _usuarioRepository.EmailExistsAsync(request.Email, id))
                throw new InvalidOperationException("Já existe outro usuário com este e-mail.");

            if (await _usuarioRepository.LoginExistsAsync(request.Login, id))
                throw new InvalidOperationException("Já existe outro usuário com este login.");

            entity.Nome = request.Nome;
            entity.Email = request.Email;
            entity.Login = request.Login;
            entity.Telefone = request.Telefone;
            entity.Status = request.Status;
            entity.SetUpdated();

            await _usuarioRepository.UpdateAsync(entity);

            if (request.PerfilId.HasValue && request.PerfilId.Value > 0)
            {
                var perfisAtuais = await _usuarioPerfilRepository.GetByUsuarioAsync(id);
                var jaPossui = perfisAtuais.Any(p => p.PerfilId == request.PerfilId.Value);
                if (!jaPossui)
                {
                    foreach (var antigo in perfisAtuais)
                    {
                        await _usuarioPerfilRepository.DeleteAsync(antigo.Id);
                    }
                    await _usuarioPerfilRepository.AddAsync(new SegUsuarioPerfil
                    {
                        UsuarioId = id,
                        PerfilId = request.PerfilId.Value
                    });
                }
            }

            var withPerfis = await _usuarioRepository.GetWithPerfisAsync(id);
            return _mapper.Map<SegUsuarioResponseDTO>(withPerfis!);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _usuarioRepository.ExistsAsync(id)) return false;
            await _usuarioRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> AlterarSenhaAsync(long id, AlterarSenhaDTO request)
        {
            var entity = await _usuarioRepository.GetByIdAsync(id);
            if (entity == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(request.SenhaAtual, entity.Senha))
                throw new UnauthorizedAccessException("Senha atual incorreta.");

            entity.Senha = BCrypt.Net.BCrypt.HashPassword(request.NovaSenha, workFactor: 12);
            entity.SetUpdated();
            await _usuarioRepository.UpdateAsync(entity);
            return true;
        }
    }
}
