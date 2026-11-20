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
        private static readonly HashSet<string> TablesWithoutCreatedAt = new(StringComparer.Ordinal)
        {
            "fin_lancamentos",
            "os_avaliacoes",
            "os_ordens_historico"
        };

        private static readonly HashSet<string> TablesWithoutUpdatedAt = new(StringComparer.Ordinal)
        {
            "cad_fornecedores_segmentos_rel",
            "cad_prestadores_atividades",
            "cad_prestadores_disponibilidades",
            "cad_veiculos_marcas",
            "cad_veiculos_modelos",
            "est_localizacoes",
            "est_movimentacoes",
            "est_pecas_anexos",
            "est_pecas_historico",
            "fin_anexos",
            "fin_historico",
            "fin_lancamentos",
            "fin_pagamentos",
            "os_anexos",
            "os_avaliacoes",
            "os_observacoes",
            "os_ordens_historico",
            "seg_audit_log",
            "seg_perfis_permissoes",
            "seg_permissoes",
            "seg_usuarios_perfis"
        };

        private static readonly HashSet<string> TablesWithDeletedAt = new(StringComparer.Ordinal)
        {
            "cad_clientes",
            "cad_clientes_financeiro",
            "cad_clientes_indicacoes",
            "cad_clientes_lgpd_consentimentos",
            "cad_fornecedores",
            "cad_fornecedores_avaliacoes",
            "cad_fornecedores_bancos",
            "cad_fornecedores_certificacoes",
            "cad_fornecedores_representantes",
            "cad_mecanicos",
            "cad_pessoas",
            "cad_pessoas_anexos",
            "cad_pessoas_contatos",
            "cad_pessoas_documentos",
            "cad_pessoas_enderecos",
            "cad_prestadores",
            "cad_veiculos",
            "est_pecas",
            "est_pecas_anexos",
            "fin_contas_pagar",
            "fin_contas_receber",
            "os_anexos",
            "os_itens",
            "os_observacoes",
            "os_ordens",
            "seg_perfis",
            "seg_usuarios"
        };

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

            builder.Ignore<ClienteAnexo>();
            builder.Ignore<ClienteContato>();
            builder.Ignore<ClienteDocumento>();
            builder.Ignore<ClienteEndereco>();
            builder.Ignore<ClientePf>();
            builder.Ignore<ClientePj>();
            builder.Ignore<ClienteFinanceiro>();
            builder.Ignore<ClienteIndicacao>();
            builder.Ignore<ClienteLgpdConsentimento>();

            builder.Ignore<FornecedorEndereco>();
            builder.Ignore<FornecedorContato>();
            builder.Ignore<FornecedorDocumento>();

            builder.Ignore<MecanicoEspecialidade>();
            builder.Ignore<MecanicoEspecialidadeRel>();
            builder.Ignore<MecanicoContato>();
            builder.Ignore<MecanicoDocumento>();
            builder.Ignore<MecanicoEndereco>();
            builder.Ignore<MecanicoExperiencia>();
            builder.Ignore<MecanicoCertificacao>();
            builder.Ignore<MecanicoDisponibilidade>();

            // BaseEntity columns vary by table in DDL V2.
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;
                if (clrType == null || !typeof(BaseEntity).IsAssignableFrom(clrType))
                {
                    continue;
                }

                var tableName = entityType.GetTableName();
                if (tableName == null)
                {
                    continue;
                }

                var entity = builder.Entity(clrType);

                ConfigureBaseColumn(entity, nameof(BaseEntity.CreatedAt), "created_at", !TablesWithoutCreatedAt.Contains(tableName));
                ConfigureBaseColumn(entity, nameof(BaseEntity.UpdatedAt), "updated_at", !TablesWithoutUpdatedAt.Contains(tableName));
                ConfigureBaseColumn(entity, nameof(BaseEntity.DeletedAt), "deleted_at", TablesWithDeletedAt.Contains(tableName));

                if (!TablesWithDeletedAt.Contains(tableName))
                {
                    continue;
                }

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

                builder.Entity(clrType).HasQueryFilter(lambda);
            }
        }

        private static void ConfigureBaseColumn(
            EntityTypeBuilder entity,
            string propertyName,
            string columnName,
            bool columnExists)
        {
            if (columnExists)
            {
                entity.Property(propertyName).HasColumnName(columnName);
            }
            else
            {
                entity.Ignore(propertyName);
            }
        }
    }
}
