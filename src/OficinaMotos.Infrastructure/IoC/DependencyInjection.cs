using OficinaMotos.Domain.Interfaces.Repositories.SegurancaRepo;
using OficinaMotos.Infrastructure.Repositories.SegurancaRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OficinaMotos.Domain.Interfaces.Repositories;
using OficinaMotos.Domain.Interfaces.Repositories.ClienteRepo;
using OficinaMotos.Domain.Interfaces.Repositories.Estoque;
using OficinaMotos.Domain.Interfaces.Repositories.FinanceiroRepo;
using OficinaMotos.Domain.Interfaces.Repositories.Fornecedor;
using OficinaMotos.Domain.Interfaces.Repositories.Mecanico;
using OficinaMotos.Domain.Interfaces.Repositories.OrdemServicoRepo;
using OficinaMotos.Domain.Interfaces.Repositories.VeiculoRepo;
using OficinaMotos.Infrastructure.Context;
using OficinaMotos.Infrastructure.Repositories;
using OficinaMotos.Infrastructure.Repositories.Cliente;
using OficinaMotos.Infrastructure.Repositories.Estoque;
using OficinaMotos.Infrastructure.Repositories.FinanceiroRepo;
using OficinaMotos.Infrastructure.Repositories.Fornecedor;
using OficinaMotos.Infrastructure.Repositories.OrdemServicoRepo;
using OficinaMotos.Infrastructure.Repositories.Veiculo;

namespace OficinaMotos.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Configurar Entity Framework com MySQL
            var connectionString = configuration.GetConnectionString("OficinaDb");
            services.AddDbContext<OficinaContext>(options =>
                options.UseMySql(connectionString,
                    ServerVersion.AutoDetect(connectionString),
                    b => b.MigrationsAssembly(typeof(OficinaContext).Assembly.FullName)));

            // 2. Injetar Repositórios (Vida útil: Scoped = 1 por requisição HTTP)
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IClienteOrigemRepository, ClienteOrigemRepository>();
            services.AddScoped<IClienteAnexoRepository, ClienteAnexoRepository>();
            services.AddScoped<IClienteContatoRepository, ClienteContatoRepository>();
            services.AddScoped<IClienteDocumentoRepository, ClienteDocumentoRepository>();
            services.AddScoped<IClienteEnderecoRepository, ClienteEnderecoRepository>();
            services.AddScoped<IClienteFinanceiroRepository, ClienteFinanceiroRepository>();
            services.AddScoped<IClienteIndicacaoRepository, ClienteIndicacaoRepository>();
            services.AddScoped<IClienteLgpdConsentimentoRepository, ClienteLgpdConsentimentoRepository>();
            services.AddScoped<IClientePfRepository, ClientePfRepository>();
            services.AddScoped<IClientePjRepository, ClientePjRepository>();
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
            services.AddScoped<IMecanicoEspecialidadeRepository, MecanicoEspecialidadeRepository>();
            services.AddScoped<IMecanicoCertificacaoRepository, MecanicoCertificacaoRepository>();
            services.AddScoped<IMecanicoContatoRepository, MecanicoContatoRepository>();
            services.AddScoped<IMecanicoDisponibilidadeRepository, MecanicoDisponibilidadeRepository>();
            services.AddScoped<IMecanicoDocumentoRepository, MecanicoDocumentoRepository>();
            services.AddScoped<IMecanicoEnderecoRepository, MecanicoEnderecoRepository>();
            services.AddScoped<IMecanicoEspecialidadeRelRepository, MecanicoEspecialidadeRelRepository>();
            services.AddScoped<IMecanicoExperienciaRepository, MecanicoExperienciaRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IFornecedorSegmentoRepository, FornecedorSegmentoRepository>();
            services.AddScoped<IFornecedorSegmentoRelRepository, FornecedorSegmentoRelRepository>();
            services.AddScoped<IFornecedorEnderecoRepository, FornecedorEnderecoRepository>();
            services.AddScoped<IFornecedorContatoRepository, FornecedorContatoRepository>();
            services.AddScoped<IFornecedorRepresentanteRepository, FornecedorRepresentanteRepository>();
            services.AddScoped<IFornecedorBancoRepository, FornecedorBancoRepository>();
            services.AddScoped<IFornecedorDocumentoRepository, FornecedorDocumentoRepository>();
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
