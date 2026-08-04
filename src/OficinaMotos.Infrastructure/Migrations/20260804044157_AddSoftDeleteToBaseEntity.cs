using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OficinaMotos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "seg_usuarios_perfis",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "seg_usuarios_perfis",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "seg_usuarios",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "seg_usuarios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "seg_refresh_tokens",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "seg_refresh_tokens",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "seg_permissoes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "seg_permissoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "seg_perfis_permissoes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "seg_perfis_permissoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "seg_perfis",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "seg_perfis",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "seg_modulos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "seg_modulos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "seg_audit_log",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "seg_audit_log",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "os_pagamentos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "os_pagamentos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "os_ordens_historico",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "os_ordens_historico",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "os_ordens",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "os_ordens",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "os_observacoes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "os_observacoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "os_itens",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "os_itens",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "os_checklists",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "os_checklists",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "os_avaliacoes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "os_avaliacoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "os_anexos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "os_anexos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "fin_pagamentos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "fin_pagamentos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "fin_metodos_pagamento",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "fin_metodos_pagamento",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "fin_lancamentos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "fin_lancamentos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "fin_historico",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "fin_historico",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "fin_contas_receber",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "fin_contas_receber",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "fin_contas_pagar",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "fin_contas_pagar",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "fin_anexos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "fin_anexos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "est_pecas_historico",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "est_pecas_historico",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "est_pecas_fornecedores",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "est_pecas_fornecedores",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "est_pecas_anexos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "est_pecas_anexos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "est_pecas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "est_pecas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "est_movimentacoes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "est_movimentacoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "est_localizacoes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "est_localizacoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "est_fabricantes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "est_fabricantes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "est_categorias",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "est_categorias",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_veiculos_modelos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_veiculos_modelos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_veiculos_marcas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_veiculos_marcas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_veiculos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_veiculos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_mecanicos_experiencias",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_mecanicos_experiencias",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_mecanicos_especialidades_rel",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_mecanicos_especialidades_rel",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_mecanicos_especialidades",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_mecanicos_especialidades",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_mecanicos_enderecos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_mecanicos_enderecos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_mecanicos_documentos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_mecanicos_documentos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_mecanicos_disponibilidades",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_mecanicos_disponibilidades",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_mecanicos_contatos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_mecanicos_contatos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_mecanicos_certificacoes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_mecanicos_certificacoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_mecanicos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_mecanicos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_fornecedores_segmentos_rel",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_fornecedores_segmentos_rel",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_fornecedores_segmentos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_fornecedores_segmentos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_fornecedores_representantes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_fornecedores_representantes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_fornecedores_enderecos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_fornecedores_enderecos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_fornecedores_documentos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_fornecedores_documentos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_fornecedores_contatos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_fornecedores_contatos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_fornecedores_certificacoes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_fornecedores_certificacoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_fornecedores_bancos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_fornecedores_bancos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_fornecedores_avaliacoes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_fornecedores_avaliacoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_fornecedores",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_fornecedores",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_clientes_pj",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_clientes_pj",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_clientes_pf",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_clientes_pf",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_clientes_origens",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_clientes_origens",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_clientes_lgpd_consentimentos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_clientes_lgpd_consentimentos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_clientes_indicacoes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_clientes_indicacoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_clientes_financeiro",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_clientes_financeiro",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_clientes_enderecos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_clientes_enderecos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_clientes_documentos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_clientes_documentos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_clientes_contatos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_clientes_contatos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_clientes_anexos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_clientes_anexos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "cad_clientes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "cad_clientes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "seg_usuarios_perfis");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "seg_usuarios_perfis");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "seg_usuarios");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "seg_usuarios");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "seg_refresh_tokens");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "seg_refresh_tokens");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "seg_permissoes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "seg_permissoes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "seg_perfis_permissoes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "seg_perfis_permissoes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "seg_perfis");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "seg_perfis");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "seg_modulos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "seg_modulos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "seg_audit_log");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "seg_audit_log");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "os_pagamentos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "os_pagamentos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "os_ordens_historico");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "os_ordens_historico");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "os_ordens");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "os_ordens");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "os_observacoes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "os_observacoes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "os_itens");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "os_itens");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "os_checklists");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "os_checklists");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "os_avaliacoes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "os_avaliacoes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "os_anexos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "os_anexos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "fin_pagamentos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "fin_pagamentos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "fin_metodos_pagamento");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "fin_metodos_pagamento");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "fin_lancamentos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "fin_lancamentos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "fin_historico");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "fin_historico");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "fin_contas_receber");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "fin_contas_receber");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "fin_contas_pagar");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "fin_contas_pagar");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "fin_anexos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "fin_anexos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "est_pecas_historico");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "est_pecas_historico");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "est_pecas_fornecedores");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "est_pecas_fornecedores");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "est_pecas_anexos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "est_pecas_anexos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "est_pecas");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "est_pecas");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "est_movimentacoes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "est_movimentacoes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "est_localizacoes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "est_localizacoes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "est_fabricantes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "est_fabricantes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "est_categorias");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "est_categorias");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_veiculos_modelos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_veiculos_modelos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_veiculos_marcas");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_veiculos_marcas");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_veiculos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_veiculos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_mecanicos_experiencias");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_mecanicos_experiencias");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_mecanicos_especialidades_rel");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_mecanicos_especialidades_rel");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_mecanicos_especialidades");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_mecanicos_especialidades");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_mecanicos_enderecos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_mecanicos_enderecos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_mecanicos_documentos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_mecanicos_documentos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_mecanicos_disponibilidades");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_mecanicos_disponibilidades");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_mecanicos_contatos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_mecanicos_contatos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_mecanicos_certificacoes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_mecanicos_certificacoes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_mecanicos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_mecanicos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_fornecedores_segmentos_rel");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_fornecedores_segmentos_rel");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_fornecedores_segmentos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_fornecedores_segmentos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_fornecedores_representantes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_fornecedores_representantes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_fornecedores_enderecos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_fornecedores_enderecos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_fornecedores_documentos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_fornecedores_documentos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_fornecedores_contatos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_fornecedores_contatos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_fornecedores_certificacoes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_fornecedores_certificacoes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_fornecedores_bancos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_fornecedores_bancos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_fornecedores_avaliacoes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_fornecedores_avaliacoes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_fornecedores");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_fornecedores");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_clientes_pj");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_clientes_pj");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_clientes_pf");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_clientes_pf");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_clientes_origens");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_clientes_origens");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_clientes_lgpd_consentimentos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_clientes_lgpd_consentimentos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_clientes_indicacoes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_clientes_indicacoes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_clientes_financeiro");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_clientes_financeiro");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_clientes_enderecos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_clientes_enderecos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_clientes_documentos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_clientes_documentos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_clientes_contatos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_clientes_contatos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_clientes_anexos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_clientes_anexos");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "cad_clientes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "cad_clientes");
        }
    }
}
