using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Infrastructure.Context
{
    public class OficinaContext : DbContext
    {
        public OficinaContext(DbContextOptions<OficinaContext> options) : base(options)
        { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ClienteOrigem> ClienteOrigens { get; set; }
        public DbSet<ClienteAnexo> ClienteAnexos { get; set; }
        public DbSet<ClienteContato> ClienteContatos { get; set; }
        public DbSet<ClienteDocumento> ClienteDocumentos { get; set; }
        public DbSet<ClienteEndereco> ClienteEnderecos { get; set; }
        public DbSet<ClienteFinanceiro> ClienteFinanceiro { get; set; }
        public DbSet<ClienteIndicacao> ClienteIndicacoes { get; set; }
        public DbSet<ClienteLgpdConsentimento> ClienteLgpdConsentimentos { get; set; }
        public DbSet<ClientePf> ClientePfs { get; set; }
        public DbSet<ClientePj> ClientePjs { get; set; }
        public DbSet<EstoqueCategoria> EstoqueCategorias { get; set; }
        public DbSet<EstoqueFabricante> EstoqueFabricantes { get; set; }
        public DbSet<EstoqueLocalizacao> EstoqueLocalizacoes { get; set; }
        public DbSet<EstoquePeca> EstoquePecas { get; set; }
        public DbSet<EstoqueMovimentacao> EstoqueMovimentacoes { get; set; }
        public DbSet<EstoquePecaAnexo> EstoquePecaAnexos { get; set; }
        public DbSet<EstoquePecaFornecedor> EstoquePecaFornecedores { get; set; }
        public DbSet<EstoquePecaHistorico> EstoquePecaHistoricos { get; set; }
        public DbSet<FinanceiroMetodoPagamento> FinanceiroMetodosPagamento { get; set; }
        public DbSet<FinanceiroPagamento> FinanceiroPagamentos { get; set; }
        public DbSet<FinanceiroAnexo> FinanceiroAnexos { get; set; }
        public DbSet<FinanceiroContaPagar> FinanceiroContasPagar { get; set; }
        public DbSet<FinanceiroContaReceber> FinanceiroContasReceber { get; set; }
        public DbSet<FinanceiroHistorico> FinanceiroHistoricos { get; set; }
        public DbSet<FinanceiroLancamento> FinanceiroLancamentos { get; set; }
        public DbSet<FornecedorSegmento> FornecedorSegmentos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<FornecedorSegmentoRel> FornecedorSegmentosRel { get; set; }
        public DbSet<FornecedorEndereco> FornecedorEnderecos { get; set; }
        public DbSet<FornecedorContato> FornecedorContatos { get; set; }
        public DbSet<FornecedorRepresentante> FornecedorRepresentantes { get; set; }
        public DbSet<FornecedorBanco> FornecedorBancos { get; set; }
        public DbSet<FornecedorDocumento> FornecedorDocumentos { get; set; }
        public DbSet<FornecedorCertificacao> FornecedorCertificacoes { get; set; }
        public DbSet<FornecedorAvaliacao> FornecedorAvaliacoes { get; set; }
        public DbSet<MecanicoEspecialidade> MecanicoEspecialidades { get; set; }
        public DbSet<Mecanico> Mecanicos { get; set; }
        public DbSet<MecanicoCertificacao> MecanicoCertificacoes { get; set; }
        public DbSet<MecanicoContato> MecanicoContatos { get; set; }
        public DbSet<MecanicoDisponibilidade> MecanicoDisponibilidades { get; set; }
        public DbSet<MecanicoDocumento> MecanicoDocumentos { get; set; }
        public DbSet<MecanicoEndereco> MecanicoEnderecos { get; set; }
        public DbSet<MecanicoEspecialidadeRel> MecanicoEspecialidadesRel { get; set; }
        public DbSet<MecanicoExperiencia> MecanicoExperiencias { get; set; }
        public DbSet<OrdemServico> OrdensServico { get; set; }
        public DbSet<OrdemServicoAnexo> OrdemServicoAnexos { get; set; }
        public DbSet<OrdemServicoAvaliacao> OrdemServicoAvaliacoes { get; set; }
        public DbSet<OrdemServicoChecklist> OrdemServicoChecklists { get; set; }
        public DbSet<OrdemServicoItem> OrdemServicoItens { get; set; }
        public DbSet<OrdemServicoObservacao> OrdemServicoObservacoes { get; set; }
        public DbSet<OrdemServicoHistorico> OrdemServicoHistoricos { get; set; }
        public DbSet<OrdemServicoPagamento> OrdemServicoPagamentos { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<VeiculoMarca> VeiculoMarcas { get; set; }
        public DbSet<VeiculoModelo> VeiculoModelos { get; set; }

        // Segurança
        public DbSet<SegModulo> SegModulos { get; set; }
        public DbSet<SegPerfil> SegPerfis { get; set; }
        public DbSet<SegPermissao> SegPermissoes { get; set; }
        public DbSet<SegPerfilPermissao> SegPerfisPermissoes { get; set; }
        public DbSet<SegUsuario> SegUsuarios { get; set; }
        public DbSet<SegUsuarioPerfil> SegUsuariosPerfis { get; set; }
        public DbSet<SegAuditLog> SegAuditLogs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // DICA PROFISSIONAL:
            // Isso varre o projeto atual e aplica TODAS as configurações (ClienteConfiguration, etc)
            // de uma vez só. Você não precisa adicionar uma por uma.
            builder.ApplyConfigurationsFromAssembly(typeof(OficinaContext).Assembly);
        }
    }
}
