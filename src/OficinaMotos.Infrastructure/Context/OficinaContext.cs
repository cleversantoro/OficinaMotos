using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OficinaMotos.Domain.Common;
using OficinaMotos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OficinaMotos.Infrastructure.Context
{
    public class OficinaContext : DbContext
    {
        public OficinaContext(DbContextOptions<OficinaContext> options) : base(options)
        { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ClienteOrigem> ClienteOrigens { get; set; }
        public DbSet<ClienteFinanceiro> ClienteFinanceiro { get; set; }
        public DbSet<ClienteIndicacao> ClienteIndicacoes { get; set; }
        public DbSet<ClienteLgpdConsentimento> ClienteLgpdConsentimentos { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<PessoaPf> PessoasPf { get; set; }
        public DbSet<PessoaPj> PessoasPj { get; set; }
        public DbSet<PessoaContato> PessoaContatos { get; set; }
        public DbSet<PessoaEndereco> PessoaEnderecos { get; set; }
        public DbSet<PessoaDocumento> PessoaDocumentos { get; set; }
        public DbSet<PessoaAnexo> PessoaAnexos { get; set; }
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
        public DbSet<FornecedorRepresentante> FornecedorRepresentantes { get; set; }
        public DbSet<FornecedorBanco> FornecedorBancos { get; set; }
        public DbSet<FornecedorCertificacao> FornecedorCertificacoes { get; set; }
        public DbSet<FornecedorAvaliacao> FornecedorAvaliacoes { get; set; }
        public DbSet<Mecanico> Mecanicos { get; set; }
        public DbSet<Prestador> Prestadores { get; set; }
        public DbSet<AtividadeProfissional> AtividadesProfissionais { get; set; }
        public DbSet<PrestadorAtividade> PrestadoresAtividades { get; set; }
        public DbSet<PrestadorDisponibilidade> PrestadoresDisponibilidades { get; set; }
        public DbSet<PrestadorExperiencia> PrestadoresExperiencias { get; set; }
        public DbSet<PrestadorCertificacao> PrestadoresCertificacoes { get; set; }
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


            // Todas as tabelas cujas entidades herdam BaseEntity possuem created_at, updated_at e deleted_at.
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;
                if (clrType == null || !typeof(BaseEntity).IsAssignableFrom(clrType))
                {
                    continue;
                }

                var entity = builder.Entity(clrType);
                entity.Property(nameof(BaseEntity.CreatedAt)).HasColumnName("created_at");
                entity.Property(nameof(BaseEntity.UpdatedAt)).HasColumnName("updated_at");
                entity.Property(nameof(BaseEntity.DeletedAt)).HasColumnName("deleted_at");

                var parameter = Expression.Parameter(clrType, "e");
                var deletedAtProperty = Expression.Call(
                    typeof(EF),
                    nameof(EF.Property),
                    new[] { typeof(DateTime?) },
                    parameter,
                    Expression.Constant(nameof(BaseEntity.DeletedAt))
                );
                var filter = Expression.Equal(deletedAtProperty, Expression.Constant(null, typeof(DateTime?)));
                var lambda = Expression.Lambda(filter, parameter);

                entity.HasQueryFilter(lambda);
            }
        }
    }
}
