using OficinaMotos.Application.Services.Seguranca;
using OficinaMotos.Application.Services.Dashboard;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OficinaMotos.Application.Interfaces;
using OficinaMotos.Application.Mappings;
using OficinaMotos.Application.Services.Cliente;
using OficinaMotos.Application.Services.Estoque;
using OficinaMotos.Application.Services.Financeiro;
using OficinaMotos.Application.Services.Fornecedor;
using OficinaMotos.Application.Services.Mecanico;
using OficinaMotos.Application.Services.OrdemServicoRepo;
using OficinaMotos.Application.Services.Veiculo;
using QuestPDF.Infrastructure;
using System.Reflection;

namespace OficinaMotos.Application.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // 0. QuestPDF Community License
            QuestPDF.Settings.License = LicenseType.Community;

            // 1. AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<DomainToDTOMappingProfile>());

            // 2. Services
            services.AddScoped<Interfaces.Cliente.IClienteService, ClienteService>();
            services.AddScoped<Interfaces.Cliente.IClienteOrigemService, ClienteOrigemService>();
            services.AddScoped<Interfaces.Cliente.IClienteAnexoService, ClienteAnexoService>();
            services.AddScoped<Interfaces.Cliente.IClienteContatoService, ClienteContatoService>();
            services.AddScoped<Interfaces.Cliente.IClienteDocumentoService, ClienteDocumentoService>();
            services.AddScoped<Interfaces.Cliente.IClienteEnderecoService, ClienteEnderecoService>();
            services.AddScoped<Interfaces.Cliente.IClienteFinanceiroService, ClienteFinanceiroService>();
            services.AddScoped<Interfaces.Cliente.IClienteIndicacaoService, ClienteIndicacaoService>();
            services.AddScoped<Interfaces.Cliente.IClienteLgpdConsentimentoService, ClienteLgpdConsentimentoService>();
            services.AddScoped<Interfaces.Cliente.IClientePfService, ClientePfService>();
            services.AddScoped<Interfaces.Cliente.IClientePjService, ClientePjService>();
            services.AddScoped<Interfaces.Estoque.IEstoqueCategoriaService, EstoqueCategoriaService>();
            services.AddScoped<Interfaces.Estoque.IEstoqueFabricanteService, EstoqueFabricanteService>();
            services.AddScoped<Interfaces.Estoque.IEstoqueLocalizacaoService, EstoqueLocalizacaoService>();
            services.AddScoped<Interfaces.Estoque.IEstoquePecaService, EstoquePecaService>();
            services.AddScoped<Interfaces.Estoque.IEstoqueMovimentacaoService, EstoqueMovimentacaoService>();
            services.AddScoped<Interfaces.Estoque.IEstoquePecaAnexoService, EstoquePecaAnexoService>();
            services.AddScoped<Interfaces.Estoque.IEstoquePecaFornecedorService, EstoquePecaFornecedorService>();
            services.AddScoped<Interfaces.Estoque.IEstoquePecaHistoricoService, EstoquePecaHistoricoService>();
            services.AddScoped<Interfaces.Financeiro.IFinanceiroMetodoPagamentoService, OficinaMotos.Application.Services.Financeiro.FinanceiroMetodoPagamentoService>();
            services.AddScoped<Interfaces.Financeiro.IFinanceiroPagamentoService, OficinaMotos.Application.Services.Financeiro.FinanceiroPagamentoService>();
            services.AddScoped<Interfaces.Financeiro.IFinanceiroAnexoService, OficinaMotos.Application.Services.Financeiro.FinanceiroAnexoService>();
            services.AddScoped<Interfaces.Financeiro.IFinanceiroContaPagarService, OficinaMotos.Application.Services.Financeiro.FinanceiroContaPagarService>();
            services.AddScoped<Interfaces.Financeiro.IFinanceiroContaReceberService, OficinaMotos.Application.Services.Financeiro.FinanceiroContaReceberService>();
            services.AddScoped<Interfaces.Financeiro.IFinanceiroHistoricoService, OficinaMotos.Application.Services.Financeiro.FinanceiroHistoricoService>();
            services.AddScoped<Interfaces.Financeiro.IFinanceiroLancamentoService, OficinaMotos.Application.Services.Financeiro.FinanceiroLancamentoService>();
            services.AddScoped<Interfaces.OrdemServico.IOrdemServicoService, OrdemServicoService>();
            services.AddScoped<Interfaces.OrdemServico.IPdfService, PdfService>();
            services.AddScoped<Interfaces.OrdemServico.IOrdemServicoAnexoService, OrdemServicoAnexoService>();
            services.AddScoped<Interfaces.OrdemServico.IOrdemServicoAvaliacaoService, OrdemServicoAvaliacaoService>();
            services.AddScoped<Interfaces.OrdemServico.IOrdemServicoChecklistService, OrdemServicoChecklistService>();
            services.AddScoped<Interfaces.OrdemServico.IOrdemServicoItemService, OrdemServicoItemService>();
            services.AddScoped<Interfaces.OrdemServico.IOrdemServicoObservacaoService, OrdemServicoObservacaoService>();
            services.AddScoped<Interfaces.OrdemServico.IOrdemServicoHistoricoService, OrdemServicoHistoricoService>();
            services.AddScoped<Interfaces.OrdemServico.IOrdemServicoPagamentoService, OrdemServicoPagamentoService>();
            services.AddScoped<Interfaces.Mecanico.IMecanicoService, MecanicoService>();
            services.AddScoped<Interfaces.Mecanico.IMecanicoEspecialidadeService, MecanicoEspecialidadeService>();
            services.AddScoped<Interfaces.Mecanico.IMecanicoCertificacaoService, MecanicoCertificacaoService>();
            services.AddScoped<Interfaces.Mecanico.IMecanicoContatoService, MecanicoContatoService>();
            services.AddScoped<Interfaces.Mecanico.IMecanicoDisponibilidadeService, MecanicoDisponibilidadeService>();
            services.AddScoped<Interfaces.Mecanico.IMecanicoDocumentoService, MecanicoDocumentoService>();
            services.AddScoped<Interfaces.Mecanico.IMecanicoEnderecoService, MecanicoEnderecoService>();
            services.AddScoped<Interfaces.Mecanico.IMecanicoEspecialidadeRelService, MecanicoEspecialidadeRelService>();
            services.AddScoped<Interfaces.Mecanico.IMecanicoExperienciaService, MecanicoExperienciaService>();
            services.AddScoped<Interfaces.Fornecedor.IFornecedorService, FornecedorService>();
            services.AddScoped<Interfaces.Fornecedor.IFornecedorSegmentoService, FornecedorSegmentoService>();
            services.AddScoped<Interfaces.Fornecedor.IFornecedorSegmentoRelService, FornecedorSegmentoRelService>();
            services.AddScoped<Interfaces.Fornecedor.IFornecedorEnderecoService, FornecedorEnderecoService>();
            services.AddScoped<Interfaces.Fornecedor.IFornecedorContatoService, FornecedorContatoService>();
            services.AddScoped<Interfaces.Fornecedor.IFornecedorRepresentanteService, FornecedorRepresentanteService>();
            services.AddScoped<Interfaces.Fornecedor.IFornecedorBancoService, FornecedorBancoService>();
            services.AddScoped<Interfaces.Fornecedor.IFornecedorDocumentoService, FornecedorDocumentoService>();
            services.AddScoped<Interfaces.Fornecedor.IFornecedorCertificacaoService, FornecedorCertificacaoService>();
            services.AddScoped<Interfaces.Fornecedor.IFornecedorAvaliacaoService, FornecedorAvaliacaoService>();
            services.AddScoped<Interfaces.Veiculo.IVeiculoService, VeiculoService>();
            services.AddScoped<Interfaces.Veiculo.IVeiculoMarcaService, VeiculoMarcaService>();
            services.AddScoped<Interfaces.Veiculo.IVeiculoModeloService, VeiculoModeloService>();

            // Segurança
            services.AddScoped<Interfaces.Seguranca.ISegModuloService, SegModuloService>();
            services.AddScoped<Interfaces.Seguranca.ISegPerfilService, SegPerfilService>();
            services.AddScoped<Interfaces.Seguranca.ISegPermissaoService, SegPermissaoService>();
            services.AddScoped<Interfaces.Seguranca.ISegUsuarioService, SegUsuarioService>();
            services.AddScoped<Interfaces.Seguranca.ISegAuditLogService, SegAuditLogService>();
            services.AddScoped<Interfaces.Seguranca.IAuthService, AuthService>();
            services.AddScoped<Interfaces.Dashboard.IDashboardService, DashboardService>();

            // 3. FluentValidation (Registra todos os validadores automaticamente)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
