using OficinaMotos.Domain.Interfaces.Repositories.SegurancaRepo;
using OficinaMotos.Infrastructure.Repositories.SegurancaRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OficinaMotos.Infrastructure.Context;
using OficinaMotos.Infrastructure.Repositories;
using OficinaMotos.Domain.Interfaces.Repositories;
using OficinaMotos.Domain.Interfaces.Repositories.Estoque;
using OficinaMotos.Domain.Interfaces.Repositories.PessoaRepo;
using OficinaMotos.Domain.Interfaces.Repositories.Fornecedor;
using OficinaMotos.Domain.Interfaces.Repositories.ClienteRepo;
using OficinaMotos.Domain.Interfaces.Repositories.VeiculoRepo;
using OficinaMotos.Domain.Interfaces.Repositories.MecanicoRepo;
using OficinaMotos.Domain.Interfaces.Repositories.PrestadorRepo;
using OficinaMotos.Domain.Interfaces.Repositories.FinanceiroRepo;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;

using OficinaMotos.Infrastructure.Repositories.Cliente;
using OficinaMotos.Infrastructure.Repositories.Estoque;
using OficinaMotos.Infrastructure.Repositories.Fornecedor;
using OficinaMotos.Infrastructure.Repositories.PessoaRepo;
using OficinaMotos.Infrastructure.Repositories.VeiculoRepo;
using OficinaMotos.Infrastructure.Repositories.MecanicoRepo;
using OficinaMotos.Infrastructure.Repositories.PrestadorRepo;
using OficinaMotos.Infrastructure.Repositories.FinanceiroRepo;
using OficinaMotos.Infrastructure.Repositories.OrdemServicoRepo;

namespace OficinaMotos.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Configurar Entity Framework com MySQL
            var connectionString = configuration.GetConnectionString("OficinaDb");
            if (!string.Equals(configuration["UseInMemoryDatabase"], "true", System.StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(connectionString))
            {
                services.AddDbContext<OficinaContext>(options =>
                    options.UseMySql(connectionString,
                        ServerVersion.AutoDetect(connectionString),
                        b => b.MigrationsAssembly(typeof(OficinaContext).Assembly.FullName)));
            }

            // 2. Injetar Repositórios (Vida útil: Scoped = 1 por requisição HTTP)
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IClienteOrigemRepository, ClienteOrigemRepository>();
            services.AddScoped<IClienteFinanceiroRepository, ClienteFinanceiroRepository>();
            services.AddScoped<IClienteIndicacaoRepository, ClienteIndicacaoRepository>();
            services.AddScoped<IClienteLgpdConsentimentoRepository, ClienteLgpdConsentimentoRepository>();
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<IEstoqueCategoriaRepository, EstoqueCategoriaRepository>();
            services.AddScoped<IEstoqueFabricanteRepository, EstoqueFabricanteRepository>();
            services.AddScoped<IEstoqueLocalizacaoRepository, EstoqueLocalizacaoRepository>();
            services.AddScoped<IEstoquePecaRepository, EstoquePecaRepository>();
            services.AddScoped<IEstoqueMovimentacaoRepository, EstoqueMovimentacaoRepository>();
            services.AddScoped<IEstoquePecaAnexoRepository, EstoquePecaAnexoRepository>();
            services.AddScoped<IEstoquePecaFornecedorRepository, EstoquePecaFornecedorRepository>();
            services.AddScoped<IEstoquePecaHistoricoRepository, EstoquePecaHistoricoRepository>();
            services.AddScoped<IFinanceiroMetodoPagamentoRepository, FinanceiroMetodoPagamentoRepository>();
            services.AddScoped<IFinanceiroPagamentoRepository, FinanceiroPagamentoRepository>();
            services.AddScoped<IFinanceiroAnexoRepository, FinanceiroAnexoRepository>();
            services.AddScoped<IFinanceiroContaPagarRepository, FinanceiroContaPagarRepository>();
            services.AddScoped<IFinanceiroContaReceberRepository, FinanceiroContaReceberRepository>();
            services.AddScoped<IFinanceiroHistoricoRepository, FinanceiroHistoricoRepository>();
            services.AddScoped<IFinanceiroLancamentoRepository, FinanceiroLancamentoRepository>();
            services.AddScoped<IOrdemServicoRepository, OrdemServicoRepository>();
            services.AddScoped<IOrdemServicoAnexoRepository, OrdemServicoAnexoRepository>();
            services.AddScoped<IOrdemServicoAvaliacaoRepository, OrdemServicoAvaliacaoRepository>();
            services.AddScoped<IOrdemServicoChecklistRepository, OrdemServicoChecklistRepository>();
            services.AddScoped<IOrdemServicoItemRepository, OrdemServicoItemRepository>();
            services.AddScoped<IOrdemServicoObservacaoRepository, OrdemServicoObservacaoRepository>();
            services.AddScoped<IOrdemServicoHistoricoRepository, OrdemServicoHistoricoRepository>();
            services.AddScoped<IOrdemServicoPagamentoRepository, OrdemServicoPagamentoRepository>();
            services.AddScoped<IMecanicoRepository, MecanicoRepository>();
            services.AddScoped<IPrestadorRepository, PrestadorRepository>();
            services.AddScoped<IAtividadeProfissionalRepository, AtividadeProfissionalRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IFornecedorSegmentoRepository, FornecedorSegmentoRepository>();
            services.AddScoped<IFornecedorSegmentoRelRepository, FornecedorSegmentoRelRepository>();
            services.AddScoped<IFornecedorRepresentanteRepository, FornecedorRepresentanteRepository>();
            services.AddScoped<IFornecedorBancoRepository, FornecedorBancoRepository>();
            services.AddScoped<IFornecedorCertificacaoRepository, FornecedorCertificacaoRepository>();
            services.AddScoped<IFornecedorAvaliacaoRepository, FornecedorAvaliacaoRepository>();
            services.AddScoped<IVeiculoRepository, VeiculoRepository>();
            services.AddScoped<IVeiculoMarcaRepository, VeiculoMarcaRepository>();
            services.AddScoped<IVeiculoModeloRepository, VeiculoModeloRepository>();

            // Segurança
            services.AddScoped<ISegModuloRepository, SegModuloRepository>();
            services.AddScoped<ISegPerfilRepository, SegPerfilRepository>();
            services.AddScoped<ISegPermissaoRepository, SegPermissaoRepository>();
            services.AddScoped<ISegPerfilPermissaoRepository, SegPerfilPermissaoRepository>();
            services.AddScoped<ISegUsuarioRepository, SegUsuarioRepository>();
            services.AddScoped<ISegUsuarioPerfilRepository, SegUsuarioPerfilRepository>();
            services.AddScoped<ISegAuditLogRepository, SegAuditLogRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }
    }
}
