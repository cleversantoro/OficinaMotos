using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OficinaMotos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_est_movimentacoes_est_pecas_PecaId",
                table: "est_movimentacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_est_categorias_CategoriaId",
                table: "est_pecas");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_est_fabricantes_FabricanteId",
                table: "est_pecas");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_est_localizacoes_LocalizacaoId",
                table: "est_pecas");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_anexos_est_pecas_PecaId",
                table: "est_pecas_anexos");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_fornecedores_est_pecas_PecaId",
                table: "est_pecas_fornecedores");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_fornecedores_fornecedores_FornecedorId",
                table: "est_pecas_fornecedores");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_historico_est_pecas_PecaId",
                table: "est_pecas_historico");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_anexos_fin_contas_pagar_ContaPagarId",
                table: "fin_anexos");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_anexos_fin_contas_receber_ContaReceberId",
                table: "fin_anexos");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_anexos_fin_pagamentos_PagamentoId",
                table: "fin_anexos");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_contas_pagar_fin_metodos_pagamento_MetodoId",
                table: "fin_contas_pagar");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_contas_pagar_fornecedores_FornecedorId",
                table: "fin_contas_pagar");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_contas_receber_cad_clientes_ClienteId",
                table: "fin_contas_receber");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_contas_receber_fin_metodos_pagamento_MetodoId",
                table: "fin_contas_receber");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_pagamentos_cad_clientes_ClienteId",
                table: "fin_pagamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_pagamentos_fin_metodos_pagamento_MetodoId",
                table: "fin_pagamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_pagamentos_fornecedores_FornecedorId",
                table: "fin_pagamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_pagamentos_ordens_servico_OrdemServicoId",
                table: "fin_pagamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_fornecedor_avaliacoes_fornecedores_FornecedorId",
                table: "fornecedor_avaliacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_fornecedor_bancos_fornecedores_FornecedorId",
                table: "fornecedor_bancos");

            migrationBuilder.DropForeignKey(
                name: "FK_fornecedor_certificacoes_fornecedores_FornecedorId",
                table: "fornecedor_certificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_fornecedor_contatos_fornecedores_FornecedorId",
                table: "fornecedor_contatos");

            migrationBuilder.DropForeignKey(
                name: "FK_fornecedor_documentos_fornecedores_FornecedorId",
                table: "fornecedor_documentos");

            migrationBuilder.DropForeignKey(
                name: "FK_fornecedor_enderecos_fornecedores_FornecedorId",
                table: "fornecedor_enderecos");

            migrationBuilder.DropForeignKey(
                name: "FK_fornecedor_representantes_fornecedores_FornecedorId",
                table: "fornecedor_representantes");

            migrationBuilder.DropForeignKey(
                name: "FK_fornecedor_segmentos_rel_fornecedor_segmentos_SegmentoId",
                table: "fornecedor_segmentos_rel");

            migrationBuilder.DropForeignKey(
                name: "FK_fornecedor_segmentos_rel_fornecedores_FornecedorId",
                table: "fornecedor_segmentos_rel");

            migrationBuilder.DropForeignKey(
                name: "FK_fornecedores_fornecedor_segmentos_SegmentoPrincipalId",
                table: "fornecedores");

            migrationBuilder.DropForeignKey(
                name: "FK_mecanico_certificacoes_mecanico_especialidades_Especialidade~",
                table: "mecanico_certificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_mecanico_certificacoes_mecanicos_MecanicoId",
                table: "mecanico_certificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_mecanico_contatos_mecanicos_MecanicoId",
                table: "mecanico_contatos");

            migrationBuilder.DropForeignKey(
                name: "FK_mecanico_disponibilidades_mecanicos_MecanicoId",
                table: "mecanico_disponibilidades");

            migrationBuilder.DropForeignKey(
                name: "FK_mecanico_documentos_mecanicos_MecanicoId",
                table: "mecanico_documentos");

            migrationBuilder.DropForeignKey(
                name: "FK_mecanico_enderecos_mecanicos_MecanicoId",
                table: "mecanico_enderecos");

            migrationBuilder.DropForeignKey(
                name: "FK_mecanico_especialidades_rel_mecanico_especialidades_Especial~",
                table: "mecanico_especialidades_rel");

            migrationBuilder.DropForeignKey(
                name: "FK_mecanico_especialidades_rel_mecanicos_MecanicoId",
                table: "mecanico_especialidades_rel");

            migrationBuilder.DropForeignKey(
                name: "FK_mecanico_experiencias_mecanicos_MecanicoId",
                table: "mecanico_experiencias");

            migrationBuilder.DropForeignKey(
                name: "FK_mecanicos_mecanico_especialidades_EspecialidadePrincipalId",
                table: "mecanicos");

            migrationBuilder.DropForeignKey(
                name: "FK_ordens_servico_cad_clientes_ClienteId",
                table: "ordens_servico");

            migrationBuilder.DropForeignKey(
                name: "FK_ordens_servico_mecanicos_MecanicoId",
                table: "ordens_servico");

            migrationBuilder.DropForeignKey(
                name: "FK_ordens_servico_anexos_ordens_servico_OrdemServicoId",
                table: "ordens_servico_anexos");

            migrationBuilder.DropForeignKey(
                name: "FK_ordens_servico_avaliacoes_ordens_servico_OrdemServicoId",
                table: "ordens_servico_avaliacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_ordens_servico_checklists_ordens_servico_OrdemServicoId",
                table: "ordens_servico_checklists");

            migrationBuilder.DropForeignKey(
                name: "FK_ordens_servico_historico_ordens_servico_OrdemServicoId",
                table: "ordens_servico_historico");

            migrationBuilder.DropForeignKey(
                name: "FK_ordens_servico_itens_est_pecas_PecaId",
                table: "ordens_servico_itens");

            migrationBuilder.DropForeignKey(
                name: "FK_ordens_servico_itens_ordens_servico_OrdemServicoId",
                table: "ordens_servico_itens");

            migrationBuilder.DropForeignKey(
                name: "FK_ordens_servico_observacoes_ordens_servico_OrdemServicoId",
                table: "ordens_servico_observacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_ordens_servico_pagamentos_ordens_servico_OrdemServicoId",
                table: "ordens_servico_pagamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ordens_servico_pagamentos",
                table: "ordens_servico_pagamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ordens_servico_observacoes",
                table: "ordens_servico_observacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ordens_servico_itens",
                table: "ordens_servico_itens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ordens_servico_historico",
                table: "ordens_servico_historico");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ordens_servico_checklists",
                table: "ordens_servico_checklists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ordens_servico_avaliacoes",
                table: "ordens_servico_avaliacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ordens_servico_anexos",
                table: "ordens_servico_anexos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ordens_servico",
                table: "ordens_servico");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mecanicos",
                table: "mecanicos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mecanico_experiencias",
                table: "mecanico_experiencias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mecanico_especialidades_rel",
                table: "mecanico_especialidades_rel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mecanico_especialidades",
                table: "mecanico_especialidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mecanico_enderecos",
                table: "mecanico_enderecos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mecanico_documentos",
                table: "mecanico_documentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mecanico_disponibilidades",
                table: "mecanico_disponibilidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mecanico_contatos",
                table: "mecanico_contatos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mecanico_certificacoes",
                table: "mecanico_certificacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fornecedores",
                table: "fornecedores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fornecedor_segmentos_rel",
                table: "fornecedor_segmentos_rel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fornecedor_segmentos",
                table: "fornecedor_segmentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fornecedor_representantes",
                table: "fornecedor_representantes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fornecedor_enderecos",
                table: "fornecedor_enderecos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fornecedor_documentos",
                table: "fornecedor_documentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fornecedor_contatos",
                table: "fornecedor_contatos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fornecedor_certificacoes",
                table: "fornecedor_certificacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fornecedor_bancos",
                table: "fornecedor_bancos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fornecedor_avaliacoes",
                table: "fornecedor_avaliacoes");

            migrationBuilder.RenameTable(
                name: "ordens_servico_pagamentos",
                newName: "os_pagamentos");

            migrationBuilder.RenameTable(
                name: "ordens_servico_observacoes",
                newName: "os_observacoes");

            migrationBuilder.RenameTable(
                name: "ordens_servico_itens",
                newName: "os_itens");

            migrationBuilder.RenameTable(
                name: "ordens_servico_historico",
                newName: "os_ordens_historico");

            migrationBuilder.RenameTable(
                name: "ordens_servico_checklists",
                newName: "os_checklists");

            migrationBuilder.RenameTable(
                name: "ordens_servico_avaliacoes",
                newName: "os_avaliacoes");

            migrationBuilder.RenameTable(
                name: "ordens_servico_anexos",
                newName: "os_anexos");

            migrationBuilder.RenameTable(
                name: "ordens_servico",
                newName: "os_ordens");

            migrationBuilder.RenameTable(
                name: "mecanicos",
                newName: "cad_mecanicos");

            migrationBuilder.RenameTable(
                name: "mecanico_experiencias",
                newName: "cad_mecanicos_experiencias");

            migrationBuilder.RenameTable(
                name: "mecanico_especialidades_rel",
                newName: "cad_mecanicos_especialidades_rel");

            migrationBuilder.RenameTable(
                name: "mecanico_especialidades",
                newName: "cad_mecanicos_especialidades");

            migrationBuilder.RenameTable(
                name: "mecanico_enderecos",
                newName: "cad_mecanicos_enderecos");

            migrationBuilder.RenameTable(
                name: "mecanico_documentos",
                newName: "cad_mecanicos_documentos");

            migrationBuilder.RenameTable(
                name: "mecanico_disponibilidades",
                newName: "cad_mecanicos_disponibilidades");

            migrationBuilder.RenameTable(
                name: "mecanico_contatos",
                newName: "cad_mecanicos_contatos");

            migrationBuilder.RenameTable(
                name: "mecanico_certificacoes",
                newName: "cad_mecanicos_certificacoes");

            migrationBuilder.RenameTable(
                name: "fornecedores",
                newName: "cad_fornecedores");

            migrationBuilder.RenameTable(
                name: "fornecedor_segmentos_rel",
                newName: "cad_fornecedores_segmentos_rel");

            migrationBuilder.RenameTable(
                name: "fornecedor_segmentos",
                newName: "cad_fornecedores_segmentos");

            migrationBuilder.RenameTable(
                name: "fornecedor_representantes",
                newName: "cad_fornecedores_representantes");

            migrationBuilder.RenameTable(
                name: "fornecedor_enderecos",
                newName: "cad_fornecedores_enderecos");

            migrationBuilder.RenameTable(
                name: "fornecedor_documentos",
                newName: "cad_fornecedores_documentos");

            migrationBuilder.RenameTable(
                name: "fornecedor_contatos",
                newName: "cad_fornecedores_contatos");

            migrationBuilder.RenameTable(
                name: "fornecedor_certificacoes",
                newName: "cad_fornecedores_certificacoes");

            migrationBuilder.RenameTable(
                name: "fornecedor_bancos",
                newName: "cad_fornecedores_bancos");

            migrationBuilder.RenameTable(
                name: "fornecedor_avaliacoes",
                newName: "cad_fornecedores_avaliacoes");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "fin_pagamentos",
                newName: "valor");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "fin_pagamentos",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "fin_pagamentos",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Observacao",
                table: "fin_pagamentos",
                newName: "observacao");

            migrationBuilder.RenameColumn(
                name: "Data_Pagamento",
                table: "fin_pagamentos",
                newName: "data_pagamento");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "fin_pagamentos",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "OrdemServicoId",
                table: "fin_pagamentos",
                newName: "ordem_servico_id");

            migrationBuilder.RenameColumn(
                name: "MetodoId",
                table: "fin_pagamentos",
                newName: "metodo_id");

            migrationBuilder.RenameColumn(
                name: "FornecedorId",
                table: "fin_pagamentos",
                newName: "fornecedor_id");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "fin_pagamentos",
                newName: "cliente_id");

            migrationBuilder.RenameIndex(
                name: "IX_fin_pagamentos_OrdemServicoId",
                table: "fin_pagamentos",
                newName: "IX_fin_pagamentos_ordem_servico_id");

            migrationBuilder.RenameIndex(
                name: "IX_fin_pagamentos_MetodoId",
                table: "fin_pagamentos",
                newName: "IX_fin_pagamentos_metodo_id");

            migrationBuilder.RenameIndex(
                name: "IX_fin_pagamentos_FornecedorId",
                table: "fin_pagamentos",
                newName: "IX_fin_pagamentos_fornecedor_id");

            migrationBuilder.RenameIndex(
                name: "IX_fin_pagamentos_ClienteId",
                table: "fin_pagamentos",
                newName: "IX_fin_pagamentos_cliente_id");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "fin_metodos_pagamento",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "fin_metodos_pagamento",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "fin_metodos_pagamento",
                newName: "descricao");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "fin_metodos_pagamento",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "fin_lancamentos",
                newName: "valor");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "fin_lancamentos",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "fin_lancamentos",
                newName: "tipo");

            migrationBuilder.RenameColumn(
                name: "Referencia",
                table: "fin_lancamentos",
                newName: "referencia");

            migrationBuilder.RenameColumn(
                name: "Observacao",
                table: "fin_lancamentos",
                newName: "observacao");

            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "fin_lancamentos",
                newName: "descricao");

            migrationBuilder.RenameColumn(
                name: "Data_Lancamento",
                table: "fin_lancamentos",
                newName: "data_lancamento");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "fin_lancamentos",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Usuario",
                table: "fin_historico",
                newName: "usuario");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "fin_historico",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Entidade",
                table: "fin_historico",
                newName: "entidade");

            migrationBuilder.RenameColumn(
                name: "Data_Alteracao",
                table: "fin_historico",
                newName: "data_alteracao");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "fin_historico",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Campo",
                table: "fin_historico",
                newName: "campo");

            migrationBuilder.RenameColumn(
                name: "ValorNovo",
                table: "fin_historico",
                newName: "valor_novo");

            migrationBuilder.RenameColumn(
                name: "ValorAntigo",
                table: "fin_historico",
                newName: "valor_antigo");

            migrationBuilder.RenameColumn(
                name: "EntidadeId",
                table: "fin_historico",
                newName: "entidade_id");

            migrationBuilder.RenameColumn(
                name: "Vencimento",
                table: "fin_contas_receber",
                newName: "vencimento");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "fin_contas_receber",
                newName: "valor");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "fin_contas_receber",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "fin_contas_receber",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Observacao",
                table: "fin_contas_receber",
                newName: "observacao");

            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "fin_contas_receber",
                newName: "descricao");

            migrationBuilder.RenameColumn(
                name: "Data_Recebimento",
                table: "fin_contas_receber",
                newName: "data_recebimento");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "fin_contas_receber",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "MetodoId",
                table: "fin_contas_receber",
                newName: "metodo_id");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "fin_contas_receber",
                newName: "cliente_id");

            migrationBuilder.RenameIndex(
                name: "IX_fin_contas_receber_MetodoId",
                table: "fin_contas_receber",
                newName: "IX_fin_contas_receber_metodo_id");

            migrationBuilder.RenameIndex(
                name: "IX_fin_contas_receber_ClienteId",
                table: "fin_contas_receber",
                newName: "IX_fin_contas_receber_cliente_id");

            migrationBuilder.RenameColumn(
                name: "Vencimento",
                table: "fin_contas_pagar",
                newName: "vencimento");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "fin_contas_pagar",
                newName: "valor");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "fin_contas_pagar",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "fin_contas_pagar",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Observacao",
                table: "fin_contas_pagar",
                newName: "observacao");

            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "fin_contas_pagar",
                newName: "descricao");

            migrationBuilder.RenameColumn(
                name: "Data_Pagamento",
                table: "fin_contas_pagar",
                newName: "data_pagamento");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "fin_contas_pagar",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "MetodoId",
                table: "fin_contas_pagar",
                newName: "metodo_id");

            migrationBuilder.RenameColumn(
                name: "FornecedorId",
                table: "fin_contas_pagar",
                newName: "fornecedor_id");

            migrationBuilder.RenameIndex(
                name: "IX_fin_contas_pagar_MetodoId",
                table: "fin_contas_pagar",
                newName: "IX_fin_contas_pagar_metodo_id");

            migrationBuilder.RenameIndex(
                name: "IX_fin_contas_pagar_FornecedorId",
                table: "fin_contas_pagar",
                newName: "IX_fin_contas_pagar_fornecedor_id");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "fin_anexos",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "fin_anexos",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "fin_anexos",
                newName: "tipo");

            migrationBuilder.RenameColumn(
                name: "Observacao",
                table: "fin_anexos",
                newName: "observacao");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "fin_anexos",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Data_Upload",
                table: "fin_anexos",
                newName: "data_upload");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "fin_anexos",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "PagamentoId",
                table: "fin_anexos",
                newName: "pagamento_id");

            migrationBuilder.RenameColumn(
                name: "ContaReceberId",
                table: "fin_anexos",
                newName: "conta_receber_id");

            migrationBuilder.RenameColumn(
                name: "ContaPagarId",
                table: "fin_anexos",
                newName: "conta_pagar_id");

            migrationBuilder.RenameIndex(
                name: "IX_fin_anexos_PagamentoId",
                table: "fin_anexos",
                newName: "IX_fin_anexos_pagamento_id");

            migrationBuilder.RenameIndex(
                name: "IX_fin_anexos_ContaReceberId",
                table: "fin_anexos",
                newName: "IX_fin_anexos_conta_receber_id");

            migrationBuilder.RenameIndex(
                name: "IX_fin_anexos_ContaPagarId",
                table: "fin_anexos",
                newName: "IX_fin_anexos_conta_pagar_id");

            migrationBuilder.RenameColumn(
                name: "Usuario",
                table: "est_pecas_historico",
                newName: "usuario");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "est_pecas_historico",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Data_Alteracao",
                table: "est_pecas_historico",
                newName: "data_alteracao");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "est_pecas_historico",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Campo",
                table: "est_pecas_historico",
                newName: "campo");

            migrationBuilder.RenameColumn(
                name: "ValorNovo",
                table: "est_pecas_historico",
                newName: "valor_novo");

            migrationBuilder.RenameColumn(
                name: "ValorAntigo",
                table: "est_pecas_historico",
                newName: "valor_antigo");

            migrationBuilder.RenameColumn(
                name: "PecaId",
                table: "est_pecas_historico",
                newName: "peca_id");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_historico_PecaId",
                table: "est_pecas_historico",
                newName: "IX_est_pecas_historico_peca_id");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "est_pecas_fornecedores",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Preco",
                table: "est_pecas_fornecedores",
                newName: "preco");

            migrationBuilder.RenameColumn(
                name: "Observacao",
                table: "est_pecas_fornecedores",
                newName: "observacao");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "est_pecas_fornecedores",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "PrazoEntrega",
                table: "est_pecas_fornecedores",
                newName: "prazo_entrega");

            migrationBuilder.RenameColumn(
                name: "PecaId",
                table: "est_pecas_fornecedores",
                newName: "peca_id");

            migrationBuilder.RenameColumn(
                name: "FornecedorId",
                table: "est_pecas_fornecedores",
                newName: "fornecedor_id");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_fornecedores_PecaId",
                table: "est_pecas_fornecedores",
                newName: "IX_est_pecas_fornecedores_peca_id");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_fornecedores_FornecedorId",
                table: "est_pecas_fornecedores",
                newName: "IX_est_pecas_fornecedores_fornecedor_id");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "est_pecas_anexos",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "est_pecas_anexos",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "est_pecas_anexos",
                newName: "tipo");

            migrationBuilder.RenameColumn(
                name: "Observacao",
                table: "est_pecas_anexos",
                newName: "observacao");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "est_pecas_anexos",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Data_Upload",
                table: "est_pecas_anexos",
                newName: "data_upload");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "est_pecas_anexos",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "PecaId",
                table: "est_pecas_anexos",
                newName: "peca_id");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_anexos_PecaId",
                table: "est_pecas_anexos",
                newName: "IX_est_pecas_anexos_peca_id");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "est_pecas",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Unidade",
                table: "est_pecas",
                newName: "unidade");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "est_pecas",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Quantidade",
                table: "est_pecas",
                newName: "quantidade");

            migrationBuilder.RenameColumn(
                name: "Observacoes",
                table: "est_pecas",
                newName: "observacoes");

            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "est_pecas",
                newName: "descricao");

            migrationBuilder.RenameColumn(
                name: "Data_Cadastro",
                table: "est_pecas",
                newName: "data_cadastro");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "est_pecas",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Codigo",
                table: "est_pecas",
                newName: "codigo");

            migrationBuilder.RenameColumn(
                name: "PrecoUnitario",
                table: "est_pecas",
                newName: "preco_unitario");

            migrationBuilder.RenameColumn(
                name: "LocalizacaoId",
                table: "est_pecas",
                newName: "localizacao_id");

            migrationBuilder.RenameColumn(
                name: "FabricanteId",
                table: "est_pecas",
                newName: "fabricante_id");

            migrationBuilder.RenameColumn(
                name: "EstoqueMinimo",
                table: "est_pecas",
                newName: "estoque_minimo");

            migrationBuilder.RenameColumn(
                name: "EstoqueMaximo",
                table: "est_pecas",
                newName: "estoque_maximo");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "est_pecas",
                newName: "categoria_id");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_LocalizacaoId",
                table: "est_pecas",
                newName: "IX_est_pecas_localizacao_id");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_FabricanteId",
                table: "est_pecas",
                newName: "IX_est_pecas_fabricante_id");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_CategoriaId",
                table: "est_pecas",
                newName: "IX_est_pecas_categoria_id");

            migrationBuilder.RenameColumn(
                name: "Usuario",
                table: "est_movimentacoes",
                newName: "usuario");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "est_movimentacoes",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "est_movimentacoes",
                newName: "tipo");

            migrationBuilder.RenameColumn(
                name: "Referencia",
                table: "est_movimentacoes",
                newName: "referencia");

            migrationBuilder.RenameColumn(
                name: "Quantidade",
                table: "est_movimentacoes",
                newName: "quantidade");

            migrationBuilder.RenameColumn(
                name: "Data_Movimentacao",
                table: "est_movimentacoes",
                newName: "data_movimentacao");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "est_movimentacoes",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "PecaId",
                table: "est_movimentacoes",
                newName: "peca_id");

            migrationBuilder.RenameIndex(
                name: "IX_est_movimentacoes_PecaId",
                table: "est_movimentacoes",
                newName: "IX_est_movimentacoes_peca_id");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "est_localizacoes",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Prateleira",
                table: "est_localizacoes",
                newName: "prateleira");

            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "est_localizacoes",
                newName: "descricao");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "est_localizacoes",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Corredor",
                table: "est_localizacoes",
                newName: "corredor");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "est_fabricantes",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "est_fabricantes",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "est_fabricantes",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Contato",
                table: "est_fabricantes",
                newName: "contato");

            migrationBuilder.RenameColumn(
                name: "Cnpj",
                table: "est_fabricantes",
                newName: "cnpj");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "est_categorias",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "est_categorias",
                newName: "nome");

            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "est_categorias",
                newName: "descricao");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "est_categorias",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "os_pagamentos",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Data_Pagamento",
                table: "os_pagamentos",
                newName: "data_pagamento");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "os_pagamentos",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "OrdemServicoId",
                table: "os_pagamentos",
                newName: "ordem_servico_id");

            migrationBuilder.RenameIndex(
                name: "IX_ordens_servico_pagamentos_OrdemServicoId",
                table: "os_pagamentos",
                newName: "IX_os_pagamentos_ordem_servico_id");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "os_observacoes",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "os_observacoes",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "OrdemServicoId",
                table: "os_observacoes",
                newName: "ordem_servico_id");

            migrationBuilder.RenameIndex(
                name: "IX_ordens_servico_observacoes_OrdemServicoId",
                table: "os_observacoes",
                newName: "IX_os_observacoes_ordem_servico_id");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "os_itens",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Quantidade",
                table: "os_itens",
                newName: "quantidade");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "os_itens",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ValorUnitario",
                table: "os_itens",
                newName: "valor_unitario");

            migrationBuilder.RenameColumn(
                name: "PecaId",
                table: "os_itens",
                newName: "peca_id");

            migrationBuilder.RenameColumn(
                name: "OrdemServicoId",
                table: "os_itens",
                newName: "ordem_servico_id");

            migrationBuilder.RenameIndex(
                name: "IX_ordens_servico_itens_PecaId",
                table: "os_itens",
                newName: "IX_os_itens_peca_id");

            migrationBuilder.RenameIndex(
                name: "IX_ordens_servico_itens_OrdemServicoId",
                table: "os_itens",
                newName: "IX_os_itens_ordem_servico_id");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "os_ordens_historico",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Data_Alteracao",
                table: "os_ordens_historico",
                newName: "data_alteracao");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "os_ordens_historico",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ValorNovo",
                table: "os_ordens_historico",
                newName: "valor_novo");

            migrationBuilder.RenameColumn(
                name: "ValorAntigo",
                table: "os_ordens_historico",
                newName: "valor_antigo");

            migrationBuilder.RenameColumn(
                name: "OrdemServicoId",
                table: "os_ordens_historico",
                newName: "ordem_servico_id");

            migrationBuilder.RenameIndex(
                name: "IX_ordens_servico_historico_OrdemServicoId",
                table: "os_ordens_historico",
                newName: "IX_os_ordens_historico_ordem_servico_id");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "os_checklists",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "os_checklists",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "OrdemServicoId",
                table: "os_checklists",
                newName: "ordem_servico_id");

            migrationBuilder.RenameIndex(
                name: "IX_ordens_servico_checklists_OrdemServicoId",
                table: "os_checklists",
                newName: "IX_os_checklists_ordem_servico_id");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "os_avaliacoes",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "os_avaliacoes",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "OrdemServicoId",
                table: "os_avaliacoes",
                newName: "ordem_servico_id");

            migrationBuilder.RenameIndex(
                name: "IX_ordens_servico_avaliacoes_OrdemServicoId",
                table: "os_avaliacoes",
                newName: "IX_os_avaliacoes_ordem_servico_id");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "os_anexos",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Data_Upload",
                table: "os_anexos",
                newName: "data_upload");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "os_anexos",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "OrdemServicoId",
                table: "os_anexos",
                newName: "ordem_servico_id");

            migrationBuilder.RenameIndex(
                name: "IX_ordens_servico_anexos_OrdemServicoId",
                table: "os_anexos",
                newName: "IX_os_anexos_ordem_servico_id");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "os_ordens",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "os_ordens",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Data_Conclusao",
                table: "os_ordens",
                newName: "data_conclusao");

            migrationBuilder.RenameColumn(
                name: "Data_Abertura",
                table: "os_ordens",
                newName: "data_abertura");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "os_ordens",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "MecanicoId",
                table: "os_ordens",
                newName: "mecanico_id");

            migrationBuilder.RenameColumn(
                name: "DescricaoProblema",
                table: "os_ordens",
                newName: "descricao_problema");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "os_ordens",
                newName: "cliente_id");

            migrationBuilder.RenameIndex(
                name: "IX_ordens_servico_MecanicoId",
                table: "os_ordens",
                newName: "IX_os_ordens_mecanico_id");

            migrationBuilder.RenameIndex(
                name: "IX_ordens_servico_ClienteId",
                table: "os_ordens",
                newName: "IX_os_ordens_cliente_id");

            migrationBuilder.RenameColumn(
                name: "ValorHora",
                table: "cad_mecanicos",
                newName: "Valor_Hora");

            migrationBuilder.RenameColumn(
                name: "TipoDocumento",
                table: "cad_mecanicos",
                newName: "Tipo_Documento");

            migrationBuilder.RenameColumn(
                name: "NomeSocial",
                table: "cad_mecanicos",
                newName: "Nome_Social");

            migrationBuilder.RenameColumn(
                name: "EspecialidadePrincipalId",
                table: "cad_mecanicos",
                newName: "Especialidade_Principal_Id");

            migrationBuilder.RenameColumn(
                name: "DocumentoPrincipal",
                table: "cad_mecanicos",
                newName: "Documento_Principal");

            migrationBuilder.RenameColumn(
                name: "DataNascimento",
                table: "cad_mecanicos",
                newName: "Data_Nascimento");

            migrationBuilder.RenameColumn(
                name: "CargaHorariaSemanal",
                table: "cad_mecanicos",
                newName: "Carga_Horaria_Semanal");

            migrationBuilder.RenameIndex(
                name: "IX_mecanicos_EspecialidadePrincipalId",
                table: "cad_mecanicos",
                newName: "IX_cad_mecanicos_Especialidade_Principal_Id");

            migrationBuilder.RenameColumn(
                name: "ResumoAtividades",
                table: "cad_mecanicos_experiencias",
                newName: "Resumo_Atividades");

            migrationBuilder.RenameColumn(
                name: "MecanicoId",
                table: "cad_mecanicos_experiencias",
                newName: "Mecanico_Id");

            migrationBuilder.RenameIndex(
                name: "IX_mecanico_experiencias_MecanicoId",
                table: "cad_mecanicos_experiencias",
                newName: "IX_cad_mecanicos_experiencias_Mecanico_Id");

            migrationBuilder.RenameColumn(
                name: "MecanicoId",
                table: "cad_mecanicos_especialidades_rel",
                newName: "Mecanico_Id");

            migrationBuilder.RenameColumn(
                name: "EspecialidadeId",
                table: "cad_mecanicos_especialidades_rel",
                newName: "Especialidade_Id");

            migrationBuilder.RenameIndex(
                name: "IX_mecanico_especialidades_rel_MecanicoId",
                table: "cad_mecanicos_especialidades_rel",
                newName: "IX_cad_mecanicos_especialidades_rel_Mecanico_Id");

            migrationBuilder.RenameIndex(
                name: "IX_mecanico_especialidades_rel_EspecialidadeId",
                table: "cad_mecanicos_especialidades_rel",
                newName: "IX_cad_mecanicos_especialidades_rel_Especialidade_Id");

            migrationBuilder.RenameColumn(
                name: "MecanicoId",
                table: "cad_mecanicos_enderecos",
                newName: "Mecanico_Id");

            migrationBuilder.RenameIndex(
                name: "IX_mecanico_enderecos_MecanicoId",
                table: "cad_mecanicos_enderecos",
                newName: "IX_cad_mecanicos_enderecos_Mecanico_Id");

            migrationBuilder.RenameColumn(
                name: "OrgaoExpedidor",
                table: "cad_mecanicos_documentos",
                newName: "Orgao_Expedidor");

            migrationBuilder.RenameColumn(
                name: "MecanicoId",
                table: "cad_mecanicos_documentos",
                newName: "Mecanico_Id");

            migrationBuilder.RenameColumn(
                name: "ArquivoUrl",
                table: "cad_mecanicos_documentos",
                newName: "Arquivo_Url");

            migrationBuilder.RenameIndex(
                name: "IX_mecanico_documentos_MecanicoId",
                table: "cad_mecanicos_documentos",
                newName: "IX_cad_mecanicos_documentos_Mecanico_Id");

            migrationBuilder.RenameColumn(
                name: "MecanicoId",
                table: "cad_mecanicos_disponibilidades",
                newName: "Mecanico_Id");

            migrationBuilder.RenameColumn(
                name: "DiaSemana",
                table: "cad_mecanicos_disponibilidades",
                newName: "Dia_Semana");

            migrationBuilder.RenameColumn(
                name: "CapacidadeAtendimentos",
                table: "cad_mecanicos_disponibilidades",
                newName: "Capacidade_Atendimentos");

            migrationBuilder.RenameIndex(
                name: "IX_mecanico_disponibilidades_MecanicoId",
                table: "cad_mecanicos_disponibilidades",
                newName: "IX_cad_mecanicos_disponibilidades_Mecanico_Id");

            migrationBuilder.RenameColumn(
                name: "MecanicoId",
                table: "cad_mecanicos_contatos",
                newName: "Mecanico_Id");

            migrationBuilder.RenameIndex(
                name: "IX_mecanico_contatos_MecanicoId",
                table: "cad_mecanicos_contatos",
                newName: "IX_cad_mecanicos_contatos_Mecanico_Id");

            migrationBuilder.RenameColumn(
                name: "MecanicoId",
                table: "cad_mecanicos_certificacoes",
                newName: "Mecanico_Id");

            migrationBuilder.RenameColumn(
                name: "EspecialidadeId",
                table: "cad_mecanicos_certificacoes",
                newName: "Especialidade_Id");

            migrationBuilder.RenameColumn(
                name: "CodigoCertificacao",
                table: "cad_mecanicos_certificacoes",
                newName: "Codigo_Certificacao");

            migrationBuilder.RenameIndex(
                name: "IX_mecanico_certificacoes_MecanicoId",
                table: "cad_mecanicos_certificacoes",
                newName: "IX_cad_mecanicos_certificacoes_Mecanico_Id");

            migrationBuilder.RenameIndex(
                name: "IX_mecanico_certificacoes_EspecialidadeId",
                table: "cad_mecanicos_certificacoes",
                newName: "IX_cad_mecanicos_certificacoes_Especialidade_Id");

            migrationBuilder.RenameColumn(
                name: "TermosNegociados",
                table: "cad_fornecedores",
                newName: "Termos_Negociados");

            migrationBuilder.RenameColumn(
                name: "TelefonePrincipal",
                table: "cad_fornecedores",
                newName: "Telefone_Principal");

            migrationBuilder.RenameColumn(
                name: "SegmentoPrincipalId",
                table: "cad_fornecedores",
                newName: "Segmento_Principal_Id");

            migrationBuilder.RenameColumn(
                name: "RetiradaLocal",
                table: "cad_fornecedores",
                newName: "Retirada_Local");

            migrationBuilder.RenameColumn(
                name: "RazaoSocial",
                table: "cad_fornecedores",
                newName: "Razao_Social");

            migrationBuilder.RenameColumn(
                name: "RatingQualidade",
                table: "cad_fornecedores",
                newName: "Rating_Qualidade");

            migrationBuilder.RenameColumn(
                name: "RatingLogistica",
                table: "cad_fornecedores",
                newName: "Rating_Logistica");

            migrationBuilder.RenameColumn(
                name: "PrazoGarantiaPadrao",
                table: "cad_fornecedores",
                newName: "Prazo_Garantia_Padrao");

            migrationBuilder.RenameColumn(
                name: "PrazoEntregaMedio",
                table: "cad_fornecedores",
                newName: "Prazo_Entrega_Medio");

            migrationBuilder.RenameColumn(
                name: "NotaMedia",
                table: "cad_fornecedores",
                newName: "Nota_Media");

            migrationBuilder.RenameColumn(
                name: "NomeFantasia",
                table: "cad_fornecedores",
                newName: "Nome_Fantasia");

            migrationBuilder.RenameColumn(
                name: "InscricaoMunicipal",
                table: "cad_fornecedores",
                newName: "Inscricao_Municipal");

            migrationBuilder.RenameColumn(
                name: "InscricaoEstadual",
                table: "cad_fornecedores",
                newName: "Inscricao_Estadual");

            migrationBuilder.RenameColumn(
                name: "CondicaoPagamentoPadrao",
                table: "cad_fornecedores",
                newName: "Condicao_Pagamento_Padrao");

            migrationBuilder.RenameColumn(
                name: "AtendimentoPersonalizado",
                table: "cad_fornecedores",
                newName: "Atendimento_Personalizado");

            migrationBuilder.RenameIndex(
                name: "IX_fornecedores_SegmentoPrincipalId",
                table: "cad_fornecedores",
                newName: "IX_cad_fornecedores_Segmento_Principal_Id");

            migrationBuilder.RenameColumn(
                name: "SegmentoId",
                table: "cad_fornecedores_segmentos_rel",
                newName: "Segmento_Id");

            migrationBuilder.RenameColumn(
                name: "FornecedorId",
                table: "cad_fornecedores_segmentos_rel",
                newName: "Fornecedor_Id");

            migrationBuilder.RenameIndex(
                name: "IX_fornecedor_segmentos_rel_SegmentoId",
                table: "cad_fornecedores_segmentos_rel",
                newName: "IX_cad_fornecedores_segmentos_rel_Segmento_Id");

            migrationBuilder.RenameIndex(
                name: "IX_fornecedor_segmentos_rel_FornecedorId",
                table: "cad_fornecedores_segmentos_rel",
                newName: "IX_cad_fornecedores_segmentos_rel_Fornecedor_Id");

            migrationBuilder.RenameColumn(
                name: "PreferenciaContato",
                table: "cad_fornecedores_representantes",
                newName: "Preferencia_Contato");

            migrationBuilder.RenameColumn(
                name: "FornecedorId",
                table: "cad_fornecedores_representantes",
                newName: "Fornecedor_Id");

            migrationBuilder.RenameIndex(
                name: "IX_fornecedor_representantes_FornecedorId",
                table: "cad_fornecedores_representantes",
                newName: "IX_cad_fornecedores_representantes_Fornecedor_Id");

            migrationBuilder.RenameColumn(
                name: "FornecedorId",
                table: "cad_fornecedores_enderecos",
                newName: "Fornecedor_Id");

            migrationBuilder.RenameIndex(
                name: "IX_fornecedor_enderecos_FornecedorId",
                table: "cad_fornecedores_enderecos",
                newName: "IX_cad_fornecedores_enderecos_Fornecedor_Id");

            migrationBuilder.RenameColumn(
                name: "OrgaoExpedidor",
                table: "cad_fornecedores_documentos",
                newName: "Orgao_Expedidor");

            migrationBuilder.RenameColumn(
                name: "FornecedorId",
                table: "cad_fornecedores_documentos",
                newName: "Fornecedor_Id");

            migrationBuilder.RenameColumn(
                name: "ArquivoUrl",
                table: "cad_fornecedores_documentos",
                newName: "Arquivo_Url");

            migrationBuilder.RenameIndex(
                name: "IX_fornecedor_documentos_FornecedorId",
                table: "cad_fornecedores_documentos",
                newName: "IX_cad_fornecedores_documentos_Fornecedor_Id");

            migrationBuilder.RenameColumn(
                name: "FornecedorId",
                table: "cad_fornecedores_contatos",
                newName: "Fornecedor_Id");

            migrationBuilder.RenameIndex(
                name: "IX_fornecedor_contatos_FornecedorId",
                table: "cad_fornecedores_contatos",
                newName: "IX_cad_fornecedores_contatos_Fornecedor_Id");

            migrationBuilder.RenameColumn(
                name: "FornecedorId",
                table: "cad_fornecedores_certificacoes",
                newName: "Fornecedor_Id");

            migrationBuilder.RenameColumn(
                name: "CodigoCertificacao",
                table: "cad_fornecedores_certificacoes",
                newName: "Codigo_Certificacao");

            migrationBuilder.RenameIndex(
                name: "IX_fornecedor_certificacoes_FornecedorId",
                table: "cad_fornecedores_certificacoes",
                newName: "IX_cad_fornecedores_certificacoes_Fornecedor_Id");

            migrationBuilder.RenameColumn(
                name: "TipoConta",
                table: "cad_fornecedores_bancos",
                newName: "Tipo_Conta");

            migrationBuilder.RenameColumn(
                name: "PixChave",
                table: "cad_fornecedores_bancos",
                newName: "Pix_Chave");

            migrationBuilder.RenameColumn(
                name: "FornecedorId",
                table: "cad_fornecedores_bancos",
                newName: "Fornecedor_Id");

            migrationBuilder.RenameIndex(
                name: "IX_fornecedor_bancos_FornecedorId",
                table: "cad_fornecedores_bancos",
                newName: "IX_cad_fornecedores_bancos_Fornecedor_Id");

            migrationBuilder.RenameColumn(
                name: "FornecedorId",
                table: "cad_fornecedores_avaliacoes",
                newName: "Fornecedor_Id");

            migrationBuilder.RenameColumn(
                name: "AvaliadoPor",
                table: "cad_fornecedores_avaliacoes",
                newName: "Avaliado_Por");

            migrationBuilder.RenameIndex(
                name: "IX_fornecedor_avaliacoes_FornecedorId",
                table: "cad_fornecedores_avaliacoes",
                newName: "IX_cad_fornecedores_avaliacoes_Fornecedor_Id");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "fin_pagamentos",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "fin_lancamentos",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "fin_contas_receber",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "fin_contas_pagar",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "preco",
                table: "est_pecas_fornecedores",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "preco_unitario",
                table: "est_pecas",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<int>(
                name: "estoque_minimo",
                table: "est_pecas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "estoque_maximo",
                table: "est_pecas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "os_pagamentos",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_unitario",
                table: "os_itens",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddColumn<decimal>(
                name: "total",
                table: "os_itens",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<bool>(
                name: "Realizado",
                table: "os_checklists",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor_Hora",
                table: "cad_mecanicos",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<int>(
                name: "Carga_Horaria_Semanal",
                table: "cad_mecanicos",
                type: "int",
                nullable: false,
                defaultValue: 44,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "cad_mecanicos_especialidades_rel",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "Ativo",
                table: "cad_mecanicos_especialidades",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "cad_mecanicos_enderecos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<int>(
                name: "Capacidade_Atendimentos",
                table: "cad_mecanicos_disponibilidades",
                type: "int",
                nullable: false,
                defaultValue: 5,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "cad_mecanicos_contatos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "cad_fornecedores",
                type: "varchar(600)",
                maxLength: 600,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Termos_Negociados",
                table: "cad_fornecedores",
                type: "varchar(240)",
                maxLength: 240,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "Retirada_Local",
                table: "cad_fornecedores",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating_Qualidade",
                table: "cad_fornecedores",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating_Logistica",
                table: "cad_fornecedores",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Nota_Media",
                table: "cad_fornecedores",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Atendimento_Personalizado",
                table: "cad_fornecedores",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "cad_fornecedores_segmentos_rel",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "Ativo",
                table: "cad_fornecedores_segmentos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "cad_fornecedores_representantes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "cad_fornecedores_representantes",
                type: "varchar(240)",
                maxLength: 240,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "cad_fornecedores_enderecos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "Arquivo_Url",
                table: "cad_fornecedores_documentos",
                type: "varchar(240)",
                maxLength: 240,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "cad_fornecedores_contatos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "Escopo",
                table: "cad_fornecedores_certificacoes",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(240)",
                oldMaxLength: 240,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Codigo_Certificacao",
                table: "cad_fornecedores_certificacoes",
                type: "varchar(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(120)",
                oldMaxLength: 120,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "cad_fornecedores_bancos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Nota",
                table: "cad_fornecedores_avaliacoes",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "Comentarios",
                table: "cad_fornecedores_avaliacoes",
                type: "varchar(400)",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Categoria",
                table: "cad_fornecedores_avaliacoes",
                type: "varchar(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Avaliado_Por",
                table: "cad_fornecedores_avaliacoes",
                type: "varchar(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(160)",
                oldMaxLength: 160,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_os_pagamentos",
                table: "os_pagamentos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_os_observacoes",
                table: "os_observacoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_os_itens",
                table: "os_itens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_os_ordens_historico",
                table: "os_ordens_historico",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_os_checklists",
                table: "os_checklists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_os_avaliacoes",
                table: "os_avaliacoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_os_anexos",
                table: "os_anexos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_os_ordens",
                table: "os_ordens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_mecanicos",
                table: "cad_mecanicos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_mecanicos_experiencias",
                table: "cad_mecanicos_experiencias",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_mecanicos_especialidades_rel",
                table: "cad_mecanicos_especialidades_rel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_mecanicos_especialidades",
                table: "cad_mecanicos_especialidades",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_mecanicos_enderecos",
                table: "cad_mecanicos_enderecos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_mecanicos_documentos",
                table: "cad_mecanicos_documentos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_mecanicos_disponibilidades",
                table: "cad_mecanicos_disponibilidades",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_mecanicos_contatos",
                table: "cad_mecanicos_contatos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_mecanicos_certificacoes",
                table: "cad_mecanicos_certificacoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_fornecedores",
                table: "cad_fornecedores",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_fornecedores_segmentos_rel",
                table: "cad_fornecedores_segmentos_rel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_fornecedores_segmentos",
                table: "cad_fornecedores_segmentos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_fornecedores_representantes",
                table: "cad_fornecedores_representantes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_fornecedores_enderecos",
                table: "cad_fornecedores_enderecos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_fornecedores_documentos",
                table: "cad_fornecedores_documentos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_fornecedores_contatos",
                table: "cad_fornecedores_contatos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_fornecedores_certificacoes",
                table: "cad_fornecedores_certificacoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_fornecedores_bancos",
                table: "cad_fornecedores_bancos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cad_fornecedores_avaliacoes",
                table: "cad_fornecedores_avaliacoes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "seg_modulos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icone = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rota = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Modulo_Pai_Id = table.Column<long>(type: "bigint", nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seg_modulos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seg_modulos_seg_modulos_Modulo_Pai_Id",
                        column: x => x.Modulo_Pai_Id,
                        principalTable: "seg_modulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seg_perfis",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(240)", maxLength: 240, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seg_perfis", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seg_usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Login = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Foto_Url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Ultimo_Login = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Token_Reset = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Token_Reset_Expira_Em = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Tentativas_Login = table.Column<int>(type: "int", nullable: false),
                    Bloqueado_Ate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Criado_Por = table.Column<long>(type: "bigint", nullable: true),
                    Created_At = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seg_usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seg_usuarios_seg_usuarios_Criado_Por",
                        column: x => x.Criado_Por,
                        principalTable: "seg_usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seg_permissoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Modulo_Id = table.Column<long>(type: "bigint", nullable: false),
                    Acao = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created_At = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seg_permissoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seg_permissoes_seg_modulos_Modulo_Id",
                        column: x => x.Modulo_Id,
                        principalTable: "seg_modulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seg_audit_log",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Usuario_Id = table.Column<long>(type: "bigint", nullable: true),
                    Login = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Acao = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Modulo = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tabela = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Registro_Id = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dados_Antes = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dados_Depois = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ip = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    User_Agent = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created_At = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seg_audit_log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seg_audit_log_seg_usuarios_Usuario_Id",
                        column: x => x.Usuario_Id,
                        principalTable: "seg_usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seg_refresh_tokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Usuario_Id = table.Column<long>(type: "bigint", nullable: false),
                    Token_Hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Expira_Em = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Revogado_Em = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Motivo_Revogacao = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ip_Criacao = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    User_Agent_Criacao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ultimo_Uso_Em = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Created_At = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seg_refresh_tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seg_refresh_tokens_seg_usuarios_Usuario_Id",
                        column: x => x.Usuario_Id,
                        principalTable: "seg_usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seg_usuarios_perfis",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Usuario_Id = table.Column<long>(type: "bigint", nullable: false),
                    Perfil_Id = table.Column<long>(type: "bigint", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seg_usuarios_perfis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seg_usuarios_perfis_seg_perfis_Perfil_Id",
                        column: x => x.Perfil_Id,
                        principalTable: "seg_perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seg_usuarios_perfis_seg_usuarios_Usuario_Id",
                        column: x => x.Usuario_Id,
                        principalTable: "seg_usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seg_perfis_permissoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Perfil_Id = table.Column<long>(type: "bigint", nullable: false),
                    Permissao_Id = table.Column<long>(type: "bigint", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seg_perfis_permissoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seg_perfis_permissoes_seg_perfis_Perfil_Id",
                        column: x => x.Perfil_Id,
                        principalTable: "seg_perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seg_perfis_permissoes_seg_permissoes_Permissao_Id",
                        column: x => x.Permissao_Id,
                        principalTable: "seg_permissoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_seg_audit_log_Usuario_Id",
                table: "seg_audit_log",
                column: "Usuario_Id");

            migrationBuilder.CreateIndex(
                name: "IX_seg_modulos_Modulo_Pai_Id",
                table: "seg_modulos",
                column: "Modulo_Pai_Id");

            migrationBuilder.CreateIndex(
                name: "IX_seg_perfis_permissoes_Permissao_Id",
                table: "seg_perfis_permissoes",
                column: "Permissao_Id");

            migrationBuilder.CreateIndex(
                name: "UQ_seg_perfis_permissoes",
                table: "seg_perfis_permissoes",
                columns: new[] { "Perfil_Id", "Permissao_Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_seg_permissoes_modulo_acao",
                table: "seg_permissoes",
                columns: new[] { "Modulo_Id", "Acao" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seg_refresh_tokens_Usuario_Id",
                table: "seg_refresh_tokens",
                column: "Usuario_Id");

            migrationBuilder.CreateIndex(
                name: "UQ_seg_refresh_tokens_hash",
                table: "seg_refresh_tokens",
                column: "Token_Hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seg_usuarios_Criado_Por",
                table: "seg_usuarios",
                column: "Criado_Por");

            migrationBuilder.CreateIndex(
                name: "UQ_seg_usuarios_email",
                table: "seg_usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_seg_usuarios_login",
                table: "seg_usuarios",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seg_usuarios_perfis_Perfil_Id",
                table: "seg_usuarios_perfis",
                column: "Perfil_Id");

            migrationBuilder.CreateIndex(
                name: "UQ_seg_usuarios_perfis",
                table: "seg_usuarios_perfis",
                columns: new[] { "Usuario_Id", "Perfil_Id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_fornecedores_cad_fornecedores_segmentos_Segmento_Princip~",
                table: "cad_fornecedores",
                column: "Segmento_Principal_Id",
                principalTable: "cad_fornecedores_segmentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_fornecedores_avaliacoes_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_avaliacoes",
                column: "Fornecedor_Id",
                principalTable: "cad_fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_fornecedores_bancos_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_bancos",
                column: "Fornecedor_Id",
                principalTable: "cad_fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_fornecedores_certificacoes_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_certificacoes",
                column: "Fornecedor_Id",
                principalTable: "cad_fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_fornecedores_contatos_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_contatos",
                column: "Fornecedor_Id",
                principalTable: "cad_fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_fornecedores_documentos_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_documentos",
                column: "Fornecedor_Id",
                principalTable: "cad_fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_fornecedores_enderecos_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_enderecos",
                column: "Fornecedor_Id",
                principalTable: "cad_fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_fornecedores_representantes_cad_fornecedores_Fornecedor_~",
                table: "cad_fornecedores_representantes",
                column: "Fornecedor_Id",
                principalTable: "cad_fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_fornecedores_segmentos_rel_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_segmentos_rel",
                column: "Fornecedor_Id",
                principalTable: "cad_fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_fornecedores_segmentos_rel_cad_fornecedores_segmentos_Se~",
                table: "cad_fornecedores_segmentos_rel",
                column: "Segmento_Id",
                principalTable: "cad_fornecedores_segmentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_mecanicos_cad_mecanicos_especialidades_Especialidade_Pri~",
                table: "cad_mecanicos",
                column: "Especialidade_Principal_Id",
                principalTable: "cad_mecanicos_especialidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_mecanicos_certificacoes_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_certificacoes",
                column: "Mecanico_Id",
                principalTable: "cad_mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_mecanicos_certificacoes_cad_mecanicos_especialidades_Esp~",
                table: "cad_mecanicos_certificacoes",
                column: "Especialidade_Id",
                principalTable: "cad_mecanicos_especialidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_mecanicos_contatos_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_contatos",
                column: "Mecanico_Id",
                principalTable: "cad_mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_mecanicos_disponibilidades_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_disponibilidades",
                column: "Mecanico_Id",
                principalTable: "cad_mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_mecanicos_documentos_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_documentos",
                column: "Mecanico_Id",
                principalTable: "cad_mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_mecanicos_enderecos_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_enderecos",
                column: "Mecanico_Id",
                principalTable: "cad_mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_mecanicos_especialidades_rel_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_especialidades_rel",
                column: "Mecanico_Id",
                principalTable: "cad_mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_mecanicos_especialidades_rel_cad_mecanicos_especialidade~",
                table: "cad_mecanicos_especialidades_rel",
                column: "Especialidade_Id",
                principalTable: "cad_mecanicos_especialidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cad_mecanicos_experiencias_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_experiencias",
                column: "Mecanico_Id",
                principalTable: "cad_mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_est_movimentacoes_est_pecas_peca_id",
                table: "est_movimentacoes",
                column: "peca_id",
                principalTable: "est_pecas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_est_categorias_categoria_id",
                table: "est_pecas",
                column: "categoria_id",
                principalTable: "est_categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_est_fabricantes_fabricante_id",
                table: "est_pecas",
                column: "fabricante_id",
                principalTable: "est_fabricantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_est_localizacoes_localizacao_id",
                table: "est_pecas",
                column: "localizacao_id",
                principalTable: "est_localizacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_anexos_est_pecas_peca_id",
                table: "est_pecas_anexos",
                column: "peca_id",
                principalTable: "est_pecas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_fornecedores_cad_fornecedores_fornecedor_id",
                table: "est_pecas_fornecedores",
                column: "fornecedor_id",
                principalTable: "cad_fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_fornecedores_est_pecas_peca_id",
                table: "est_pecas_fornecedores",
                column: "peca_id",
                principalTable: "est_pecas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_historico_est_pecas_peca_id",
                table: "est_pecas_historico",
                column: "peca_id",
                principalTable: "est_pecas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_anexos_fin_contas_pagar_conta_pagar_id",
                table: "fin_anexos",
                column: "conta_pagar_id",
                principalTable: "fin_contas_pagar",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_anexos_fin_contas_receber_conta_receber_id",
                table: "fin_anexos",
                column: "conta_receber_id",
                principalTable: "fin_contas_receber",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_anexos_fin_pagamentos_pagamento_id",
                table: "fin_anexos",
                column: "pagamento_id",
                principalTable: "fin_pagamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_contas_pagar_cad_fornecedores_fornecedor_id",
                table: "fin_contas_pagar",
                column: "fornecedor_id",
                principalTable: "cad_fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_contas_pagar_fin_metodos_pagamento_metodo_id",
                table: "fin_contas_pagar",
                column: "metodo_id",
                principalTable: "fin_metodos_pagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_contas_receber_cad_clientes_cliente_id",
                table: "fin_contas_receber",
                column: "cliente_id",
                principalTable: "cad_clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_contas_receber_fin_metodos_pagamento_metodo_id",
                table: "fin_contas_receber",
                column: "metodo_id",
                principalTable: "fin_metodos_pagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_pagamentos_cad_clientes_cliente_id",
                table: "fin_pagamentos",
                column: "cliente_id",
                principalTable: "cad_clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_pagamentos_cad_fornecedores_fornecedor_id",
                table: "fin_pagamentos",
                column: "fornecedor_id",
                principalTable: "cad_fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_pagamentos_fin_metodos_pagamento_metodo_id",
                table: "fin_pagamentos",
                column: "metodo_id",
                principalTable: "fin_metodos_pagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_pagamentos_os_ordens_ordem_servico_id",
                table: "fin_pagamentos",
                column: "ordem_servico_id",
                principalTable: "os_ordens",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_os_anexos_os_ordens_ordem_servico_id",
                table: "os_anexos",
                column: "ordem_servico_id",
                principalTable: "os_ordens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_os_avaliacoes_os_ordens_ordem_servico_id",
                table: "os_avaliacoes",
                column: "ordem_servico_id",
                principalTable: "os_ordens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_os_checklists_os_ordens_ordem_servico_id",
                table: "os_checklists",
                column: "ordem_servico_id",
                principalTable: "os_ordens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_os_itens_est_pecas_peca_id",
                table: "os_itens",
                column: "peca_id",
                principalTable: "est_pecas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_os_itens_os_ordens_ordem_servico_id",
                table: "os_itens",
                column: "ordem_servico_id",
                principalTable: "os_ordens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_os_observacoes_os_ordens_ordem_servico_id",
                table: "os_observacoes",
                column: "ordem_servico_id",
                principalTable: "os_ordens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_os_ordens_cad_clientes_cliente_id",
                table: "os_ordens",
                column: "cliente_id",
                principalTable: "cad_clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_os_ordens_cad_mecanicos_mecanico_id",
                table: "os_ordens",
                column: "mecanico_id",
                principalTable: "cad_mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_os_ordens_historico_os_ordens_ordem_servico_id",
                table: "os_ordens_historico",
                column: "ordem_servico_id",
                principalTable: "os_ordens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_os_pagamentos_os_ordens_ordem_servico_id",
                table: "os_pagamentos",
                column: "ordem_servico_id",
                principalTable: "os_ordens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cad_fornecedores_cad_fornecedores_segmentos_Segmento_Princip~",
                table: "cad_fornecedores");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_fornecedores_avaliacoes_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_avaliacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_fornecedores_bancos_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_bancos");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_fornecedores_certificacoes_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_certificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_fornecedores_contatos_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_contatos");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_fornecedores_documentos_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_documentos");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_fornecedores_enderecos_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_enderecos");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_fornecedores_representantes_cad_fornecedores_Fornecedor_~",
                table: "cad_fornecedores_representantes");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_fornecedores_segmentos_rel_cad_fornecedores_Fornecedor_Id",
                table: "cad_fornecedores_segmentos_rel");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_fornecedores_segmentos_rel_cad_fornecedores_segmentos_Se~",
                table: "cad_fornecedores_segmentos_rel");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_mecanicos_cad_mecanicos_especialidades_Especialidade_Pri~",
                table: "cad_mecanicos");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_mecanicos_certificacoes_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_certificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_mecanicos_certificacoes_cad_mecanicos_especialidades_Esp~",
                table: "cad_mecanicos_certificacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_mecanicos_contatos_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_contatos");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_mecanicos_disponibilidades_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_disponibilidades");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_mecanicos_documentos_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_documentos");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_mecanicos_enderecos_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_enderecos");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_mecanicos_especialidades_rel_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_especialidades_rel");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_mecanicos_especialidades_rel_cad_mecanicos_especialidade~",
                table: "cad_mecanicos_especialidades_rel");

            migrationBuilder.DropForeignKey(
                name: "FK_cad_mecanicos_experiencias_cad_mecanicos_Mecanico_Id",
                table: "cad_mecanicos_experiencias");

            migrationBuilder.DropForeignKey(
                name: "FK_est_movimentacoes_est_pecas_peca_id",
                table: "est_movimentacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_est_categorias_categoria_id",
                table: "est_pecas");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_est_fabricantes_fabricante_id",
                table: "est_pecas");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_est_localizacoes_localizacao_id",
                table: "est_pecas");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_anexos_est_pecas_peca_id",
                table: "est_pecas_anexos");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_fornecedores_cad_fornecedores_fornecedor_id",
                table: "est_pecas_fornecedores");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_fornecedores_est_pecas_peca_id",
                table: "est_pecas_fornecedores");

            migrationBuilder.DropForeignKey(
                name: "FK_est_pecas_historico_est_pecas_peca_id",
                table: "est_pecas_historico");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_anexos_fin_contas_pagar_conta_pagar_id",
                table: "fin_anexos");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_anexos_fin_contas_receber_conta_receber_id",
                table: "fin_anexos");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_anexos_fin_pagamentos_pagamento_id",
                table: "fin_anexos");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_contas_pagar_cad_fornecedores_fornecedor_id",
                table: "fin_contas_pagar");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_contas_pagar_fin_metodos_pagamento_metodo_id",
                table: "fin_contas_pagar");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_contas_receber_cad_clientes_cliente_id",
                table: "fin_contas_receber");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_contas_receber_fin_metodos_pagamento_metodo_id",
                table: "fin_contas_receber");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_pagamentos_cad_clientes_cliente_id",
                table: "fin_pagamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_pagamentos_cad_fornecedores_fornecedor_id",
                table: "fin_pagamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_pagamentos_fin_metodos_pagamento_metodo_id",
                table: "fin_pagamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_fin_pagamentos_os_ordens_ordem_servico_id",
                table: "fin_pagamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_os_anexos_os_ordens_ordem_servico_id",
                table: "os_anexos");

            migrationBuilder.DropForeignKey(
                name: "FK_os_avaliacoes_os_ordens_ordem_servico_id",
                table: "os_avaliacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_os_checklists_os_ordens_ordem_servico_id",
                table: "os_checklists");

            migrationBuilder.DropForeignKey(
                name: "FK_os_itens_est_pecas_peca_id",
                table: "os_itens");

            migrationBuilder.DropForeignKey(
                name: "FK_os_itens_os_ordens_ordem_servico_id",
                table: "os_itens");

            migrationBuilder.DropForeignKey(
                name: "FK_os_observacoes_os_ordens_ordem_servico_id",
                table: "os_observacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_os_ordens_cad_clientes_cliente_id",
                table: "os_ordens");

            migrationBuilder.DropForeignKey(
                name: "FK_os_ordens_cad_mecanicos_mecanico_id",
                table: "os_ordens");

            migrationBuilder.DropForeignKey(
                name: "FK_os_ordens_historico_os_ordens_ordem_servico_id",
                table: "os_ordens_historico");

            migrationBuilder.DropForeignKey(
                name: "FK_os_pagamentos_os_ordens_ordem_servico_id",
                table: "os_pagamentos");

            migrationBuilder.DropTable(
                name: "seg_audit_log");

            migrationBuilder.DropTable(
                name: "seg_perfis_permissoes");

            migrationBuilder.DropTable(
                name: "seg_refresh_tokens");

            migrationBuilder.DropTable(
                name: "seg_usuarios_perfis");

            migrationBuilder.DropTable(
                name: "seg_permissoes");

            migrationBuilder.DropTable(
                name: "seg_perfis");

            migrationBuilder.DropTable(
                name: "seg_usuarios");

            migrationBuilder.DropTable(
                name: "seg_modulos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_os_pagamentos",
                table: "os_pagamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_os_ordens_historico",
                table: "os_ordens_historico");

            migrationBuilder.DropPrimaryKey(
                name: "PK_os_ordens",
                table: "os_ordens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_os_observacoes",
                table: "os_observacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_os_itens",
                table: "os_itens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_os_checklists",
                table: "os_checklists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_os_avaliacoes",
                table: "os_avaliacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_os_anexos",
                table: "os_anexos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_mecanicos_experiencias",
                table: "cad_mecanicos_experiencias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_mecanicos_especialidades_rel",
                table: "cad_mecanicos_especialidades_rel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_mecanicos_especialidades",
                table: "cad_mecanicos_especialidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_mecanicos_enderecos",
                table: "cad_mecanicos_enderecos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_mecanicos_documentos",
                table: "cad_mecanicos_documentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_mecanicos_disponibilidades",
                table: "cad_mecanicos_disponibilidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_mecanicos_contatos",
                table: "cad_mecanicos_contatos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_mecanicos_certificacoes",
                table: "cad_mecanicos_certificacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_mecanicos",
                table: "cad_mecanicos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_fornecedores_segmentos_rel",
                table: "cad_fornecedores_segmentos_rel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_fornecedores_segmentos",
                table: "cad_fornecedores_segmentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_fornecedores_representantes",
                table: "cad_fornecedores_representantes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_fornecedores_enderecos",
                table: "cad_fornecedores_enderecos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_fornecedores_documentos",
                table: "cad_fornecedores_documentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_fornecedores_contatos",
                table: "cad_fornecedores_contatos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_fornecedores_certificacoes",
                table: "cad_fornecedores_certificacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_fornecedores_bancos",
                table: "cad_fornecedores_bancos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_fornecedores_avaliacoes",
                table: "cad_fornecedores_avaliacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cad_fornecedores",
                table: "cad_fornecedores");

            migrationBuilder.DropColumn(
                name: "total",
                table: "os_itens");

            migrationBuilder.RenameTable(
                name: "os_pagamentos",
                newName: "ordens_servico_pagamentos");

            migrationBuilder.RenameTable(
                name: "os_ordens_historico",
                newName: "ordens_servico_historico");

            migrationBuilder.RenameTable(
                name: "os_ordens",
                newName: "ordens_servico");

            migrationBuilder.RenameTable(
                name: "os_observacoes",
                newName: "ordens_servico_observacoes");

            migrationBuilder.RenameTable(
                name: "os_itens",
                newName: "ordens_servico_itens");

            migrationBuilder.RenameTable(
                name: "os_checklists",
                newName: "ordens_servico_checklists");

            migrationBuilder.RenameTable(
                name: "os_avaliacoes",
                newName: "ordens_servico_avaliacoes");

            migrationBuilder.RenameTable(
                name: "os_anexos",
                newName: "ordens_servico_anexos");

            migrationBuilder.RenameTable(
                name: "cad_mecanicos_experiencias",
                newName: "mecanico_experiencias");

            migrationBuilder.RenameTable(
                name: "cad_mecanicos_especialidades_rel",
                newName: "mecanico_especialidades_rel");

            migrationBuilder.RenameTable(
                name: "cad_mecanicos_especialidades",
                newName: "mecanico_especialidades");

            migrationBuilder.RenameTable(
                name: "cad_mecanicos_enderecos",
                newName: "mecanico_enderecos");

            migrationBuilder.RenameTable(
                name: "cad_mecanicos_documentos",
                newName: "mecanico_documentos");

            migrationBuilder.RenameTable(
                name: "cad_mecanicos_disponibilidades",
                newName: "mecanico_disponibilidades");

            migrationBuilder.RenameTable(
                name: "cad_mecanicos_contatos",
                newName: "mecanico_contatos");

            migrationBuilder.RenameTable(
                name: "cad_mecanicos_certificacoes",
                newName: "mecanico_certificacoes");

            migrationBuilder.RenameTable(
                name: "cad_mecanicos",
                newName: "mecanicos");

            migrationBuilder.RenameTable(
                name: "cad_fornecedores_segmentos_rel",
                newName: "fornecedor_segmentos_rel");

            migrationBuilder.RenameTable(
                name: "cad_fornecedores_segmentos",
                newName: "fornecedor_segmentos");

            migrationBuilder.RenameTable(
                name: "cad_fornecedores_representantes",
                newName: "fornecedor_representantes");

            migrationBuilder.RenameTable(
                name: "cad_fornecedores_enderecos",
                newName: "fornecedor_enderecos");

            migrationBuilder.RenameTable(
                name: "cad_fornecedores_documentos",
                newName: "fornecedor_documentos");

            migrationBuilder.RenameTable(
                name: "cad_fornecedores_contatos",
                newName: "fornecedor_contatos");

            migrationBuilder.RenameTable(
                name: "cad_fornecedores_certificacoes",
                newName: "fornecedor_certificacoes");

            migrationBuilder.RenameTable(
                name: "cad_fornecedores_bancos",
                newName: "fornecedor_bancos");

            migrationBuilder.RenameTable(
                name: "cad_fornecedores_avaliacoes",
                newName: "fornecedor_avaliacoes");

            migrationBuilder.RenameTable(
                name: "cad_fornecedores",
                newName: "fornecedores");

            migrationBuilder.RenameColumn(
                name: "valor",
                table: "fin_pagamentos",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "fin_pagamentos",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "fin_pagamentos",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "observacao",
                table: "fin_pagamentos",
                newName: "Observacao");

            migrationBuilder.RenameColumn(
                name: "data_pagamento",
                table: "fin_pagamentos",
                newName: "Data_Pagamento");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "fin_pagamentos",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "ordem_servico_id",
                table: "fin_pagamentos",
                newName: "OrdemServicoId");

            migrationBuilder.RenameColumn(
                name: "metodo_id",
                table: "fin_pagamentos",
                newName: "MetodoId");

            migrationBuilder.RenameColumn(
                name: "fornecedor_id",
                table: "fin_pagamentos",
                newName: "FornecedorId");

            migrationBuilder.RenameColumn(
                name: "cliente_id",
                table: "fin_pagamentos",
                newName: "ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_fin_pagamentos_ordem_servico_id",
                table: "fin_pagamentos",
                newName: "IX_fin_pagamentos_OrdemServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_fin_pagamentos_metodo_id",
                table: "fin_pagamentos",
                newName: "IX_fin_pagamentos_MetodoId");

            migrationBuilder.RenameIndex(
                name: "IX_fin_pagamentos_fornecedor_id",
                table: "fin_pagamentos",
                newName: "IX_fin_pagamentos_FornecedorId");

            migrationBuilder.RenameIndex(
                name: "IX_fin_pagamentos_cliente_id",
                table: "fin_pagamentos",
                newName: "IX_fin_pagamentos_ClienteId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "fin_metodos_pagamento",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "fin_metodos_pagamento",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "descricao",
                table: "fin_metodos_pagamento",
                newName: "Descricao");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "fin_metodos_pagamento",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "valor",
                table: "fin_lancamentos",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "fin_lancamentos",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "tipo",
                table: "fin_lancamentos",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "referencia",
                table: "fin_lancamentos",
                newName: "Referencia");

            migrationBuilder.RenameColumn(
                name: "observacao",
                table: "fin_lancamentos",
                newName: "Observacao");

            migrationBuilder.RenameColumn(
                name: "descricao",
                table: "fin_lancamentos",
                newName: "Descricao");

            migrationBuilder.RenameColumn(
                name: "data_lancamento",
                table: "fin_lancamentos",
                newName: "Data_Lancamento");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "fin_lancamentos",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "usuario",
                table: "fin_historico",
                newName: "Usuario");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "fin_historico",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "entidade",
                table: "fin_historico",
                newName: "Entidade");

            migrationBuilder.RenameColumn(
                name: "data_alteracao",
                table: "fin_historico",
                newName: "Data_Alteracao");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "fin_historico",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "campo",
                table: "fin_historico",
                newName: "Campo");

            migrationBuilder.RenameColumn(
                name: "valor_novo",
                table: "fin_historico",
                newName: "ValorNovo");

            migrationBuilder.RenameColumn(
                name: "valor_antigo",
                table: "fin_historico",
                newName: "ValorAntigo");

            migrationBuilder.RenameColumn(
                name: "entidade_id",
                table: "fin_historico",
                newName: "EntidadeId");

            migrationBuilder.RenameColumn(
                name: "vencimento",
                table: "fin_contas_receber",
                newName: "Vencimento");

            migrationBuilder.RenameColumn(
                name: "valor",
                table: "fin_contas_receber",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "fin_contas_receber",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "fin_contas_receber",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "observacao",
                table: "fin_contas_receber",
                newName: "Observacao");

            migrationBuilder.RenameColumn(
                name: "descricao",
                table: "fin_contas_receber",
                newName: "Descricao");

            migrationBuilder.RenameColumn(
                name: "data_recebimento",
                table: "fin_contas_receber",
                newName: "Data_Recebimento");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "fin_contas_receber",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "metodo_id",
                table: "fin_contas_receber",
                newName: "MetodoId");

            migrationBuilder.RenameColumn(
                name: "cliente_id",
                table: "fin_contas_receber",
                newName: "ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_fin_contas_receber_metodo_id",
                table: "fin_contas_receber",
                newName: "IX_fin_contas_receber_MetodoId");

            migrationBuilder.RenameIndex(
                name: "IX_fin_contas_receber_cliente_id",
                table: "fin_contas_receber",
                newName: "IX_fin_contas_receber_ClienteId");

            migrationBuilder.RenameColumn(
                name: "vencimento",
                table: "fin_contas_pagar",
                newName: "Vencimento");

            migrationBuilder.RenameColumn(
                name: "valor",
                table: "fin_contas_pagar",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "fin_contas_pagar",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "fin_contas_pagar",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "observacao",
                table: "fin_contas_pagar",
                newName: "Observacao");

            migrationBuilder.RenameColumn(
                name: "descricao",
                table: "fin_contas_pagar",
                newName: "Descricao");

            migrationBuilder.RenameColumn(
                name: "data_pagamento",
                table: "fin_contas_pagar",
                newName: "Data_Pagamento");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "fin_contas_pagar",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "metodo_id",
                table: "fin_contas_pagar",
                newName: "MetodoId");

            migrationBuilder.RenameColumn(
                name: "fornecedor_id",
                table: "fin_contas_pagar",
                newName: "FornecedorId");

            migrationBuilder.RenameIndex(
                name: "IX_fin_contas_pagar_metodo_id",
                table: "fin_contas_pagar",
                newName: "IX_fin_contas_pagar_MetodoId");

            migrationBuilder.RenameIndex(
                name: "IX_fin_contas_pagar_fornecedor_id",
                table: "fin_contas_pagar",
                newName: "IX_fin_contas_pagar_FornecedorId");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "fin_anexos",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "fin_anexos",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "tipo",
                table: "fin_anexos",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "observacao",
                table: "fin_anexos",
                newName: "Observacao");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "fin_anexos",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "data_upload",
                table: "fin_anexos",
                newName: "Data_Upload");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "fin_anexos",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "pagamento_id",
                table: "fin_anexos",
                newName: "PagamentoId");

            migrationBuilder.RenameColumn(
                name: "conta_receber_id",
                table: "fin_anexos",
                newName: "ContaReceberId");

            migrationBuilder.RenameColumn(
                name: "conta_pagar_id",
                table: "fin_anexos",
                newName: "ContaPagarId");

            migrationBuilder.RenameIndex(
                name: "IX_fin_anexos_pagamento_id",
                table: "fin_anexos",
                newName: "IX_fin_anexos_PagamentoId");

            migrationBuilder.RenameIndex(
                name: "IX_fin_anexos_conta_receber_id",
                table: "fin_anexos",
                newName: "IX_fin_anexos_ContaReceberId");

            migrationBuilder.RenameIndex(
                name: "IX_fin_anexos_conta_pagar_id",
                table: "fin_anexos",
                newName: "IX_fin_anexos_ContaPagarId");

            migrationBuilder.RenameColumn(
                name: "usuario",
                table: "est_pecas_historico",
                newName: "Usuario");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "est_pecas_historico",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "data_alteracao",
                table: "est_pecas_historico",
                newName: "Data_Alteracao");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "est_pecas_historico",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "campo",
                table: "est_pecas_historico",
                newName: "Campo");

            migrationBuilder.RenameColumn(
                name: "valor_novo",
                table: "est_pecas_historico",
                newName: "ValorNovo");

            migrationBuilder.RenameColumn(
                name: "valor_antigo",
                table: "est_pecas_historico",
                newName: "ValorAntigo");

            migrationBuilder.RenameColumn(
                name: "peca_id",
                table: "est_pecas_historico",
                newName: "PecaId");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_historico_peca_id",
                table: "est_pecas_historico",
                newName: "IX_est_pecas_historico_PecaId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "est_pecas_fornecedores",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "preco",
                table: "est_pecas_fornecedores",
                newName: "Preco");

            migrationBuilder.RenameColumn(
                name: "observacao",
                table: "est_pecas_fornecedores",
                newName: "Observacao");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "est_pecas_fornecedores",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "prazo_entrega",
                table: "est_pecas_fornecedores",
                newName: "PrazoEntrega");

            migrationBuilder.RenameColumn(
                name: "peca_id",
                table: "est_pecas_fornecedores",
                newName: "PecaId");

            migrationBuilder.RenameColumn(
                name: "fornecedor_id",
                table: "est_pecas_fornecedores",
                newName: "FornecedorId");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_fornecedores_peca_id",
                table: "est_pecas_fornecedores",
                newName: "IX_est_pecas_fornecedores_PecaId");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_fornecedores_fornecedor_id",
                table: "est_pecas_fornecedores",
                newName: "IX_est_pecas_fornecedores_FornecedorId");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "est_pecas_anexos",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "est_pecas_anexos",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "tipo",
                table: "est_pecas_anexos",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "observacao",
                table: "est_pecas_anexos",
                newName: "Observacao");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "est_pecas_anexos",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "data_upload",
                table: "est_pecas_anexos",
                newName: "Data_Upload");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "est_pecas_anexos",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "peca_id",
                table: "est_pecas_anexos",
                newName: "PecaId");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_anexos_peca_id",
                table: "est_pecas_anexos",
                newName: "IX_est_pecas_anexos_PecaId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "est_pecas",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "unidade",
                table: "est_pecas",
                newName: "Unidade");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "est_pecas",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "quantidade",
                table: "est_pecas",
                newName: "Quantidade");

            migrationBuilder.RenameColumn(
                name: "observacoes",
                table: "est_pecas",
                newName: "Observacoes");

            migrationBuilder.RenameColumn(
                name: "descricao",
                table: "est_pecas",
                newName: "Descricao");

            migrationBuilder.RenameColumn(
                name: "data_cadastro",
                table: "est_pecas",
                newName: "Data_Cadastro");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "est_pecas",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "codigo",
                table: "est_pecas",
                newName: "Codigo");

            migrationBuilder.RenameColumn(
                name: "preco_unitario",
                table: "est_pecas",
                newName: "PrecoUnitario");

            migrationBuilder.RenameColumn(
                name: "localizacao_id",
                table: "est_pecas",
                newName: "LocalizacaoId");

            migrationBuilder.RenameColumn(
                name: "fabricante_id",
                table: "est_pecas",
                newName: "FabricanteId");

            migrationBuilder.RenameColumn(
                name: "estoque_minimo",
                table: "est_pecas",
                newName: "EstoqueMinimo");

            migrationBuilder.RenameColumn(
                name: "estoque_maximo",
                table: "est_pecas",
                newName: "EstoqueMaximo");

            migrationBuilder.RenameColumn(
                name: "categoria_id",
                table: "est_pecas",
                newName: "CategoriaId");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_localizacao_id",
                table: "est_pecas",
                newName: "IX_est_pecas_LocalizacaoId");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_fabricante_id",
                table: "est_pecas",
                newName: "IX_est_pecas_FabricanteId");

            migrationBuilder.RenameIndex(
                name: "IX_est_pecas_categoria_id",
                table: "est_pecas",
                newName: "IX_est_pecas_CategoriaId");

            migrationBuilder.RenameColumn(
                name: "usuario",
                table: "est_movimentacoes",
                newName: "Usuario");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "est_movimentacoes",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "tipo",
                table: "est_movimentacoes",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "referencia",
                table: "est_movimentacoes",
                newName: "Referencia");

            migrationBuilder.RenameColumn(
                name: "quantidade",
                table: "est_movimentacoes",
                newName: "Quantidade");

            migrationBuilder.RenameColumn(
                name: "data_movimentacao",
                table: "est_movimentacoes",
                newName: "Data_Movimentacao");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "est_movimentacoes",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "peca_id",
                table: "est_movimentacoes",
                newName: "PecaId");

            migrationBuilder.RenameIndex(
                name: "IX_est_movimentacoes_peca_id",
                table: "est_movimentacoes",
                newName: "IX_est_movimentacoes_PecaId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "est_localizacoes",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "prateleira",
                table: "est_localizacoes",
                newName: "Prateleira");

            migrationBuilder.RenameColumn(
                name: "descricao",
                table: "est_localizacoes",
                newName: "Descricao");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "est_localizacoes",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "corredor",
                table: "est_localizacoes",
                newName: "Corredor");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "est_fabricantes",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "est_fabricantes",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "est_fabricantes",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "contato",
                table: "est_fabricantes",
                newName: "Contato");

            migrationBuilder.RenameColumn(
                name: "cnpj",
                table: "est_fabricantes",
                newName: "Cnpj");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "est_categorias",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "est_categorias",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "descricao",
                table: "est_categorias",
                newName: "Descricao");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "est_categorias",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ordens_servico_pagamentos",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "data_pagamento",
                table: "ordens_servico_pagamentos",
                newName: "Data_Pagamento");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ordens_servico_pagamentos",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "ordem_servico_id",
                table: "ordens_servico_pagamentos",
                newName: "OrdemServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_os_pagamentos_ordem_servico_id",
                table: "ordens_servico_pagamentos",
                newName: "IX_ordens_servico_pagamentos_OrdemServicoId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ordens_servico_historico",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "data_alteracao",
                table: "ordens_servico_historico",
                newName: "Data_Alteracao");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ordens_servico_historico",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "valor_novo",
                table: "ordens_servico_historico",
                newName: "ValorNovo");

            migrationBuilder.RenameColumn(
                name: "valor_antigo",
                table: "ordens_servico_historico",
                newName: "ValorAntigo");

            migrationBuilder.RenameColumn(
                name: "ordem_servico_id",
                table: "ordens_servico_historico",
                newName: "OrdemServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_os_ordens_historico_ordem_servico_id",
                table: "ordens_servico_historico",
                newName: "IX_ordens_servico_historico_OrdemServicoId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ordens_servico",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "ordens_servico",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "data_conclusao",
                table: "ordens_servico",
                newName: "Data_Conclusao");

            migrationBuilder.RenameColumn(
                name: "data_abertura",
                table: "ordens_servico",
                newName: "Data_Abertura");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ordens_servico",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "mecanico_id",
                table: "ordens_servico",
                newName: "MecanicoId");

            migrationBuilder.RenameColumn(
                name: "descricao_problema",
                table: "ordens_servico",
                newName: "DescricaoProblema");

            migrationBuilder.RenameColumn(
                name: "cliente_id",
                table: "ordens_servico",
                newName: "ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_os_ordens_mecanico_id",
                table: "ordens_servico",
                newName: "IX_ordens_servico_MecanicoId");

            migrationBuilder.RenameIndex(
                name: "IX_os_ordens_cliente_id",
                table: "ordens_servico",
                newName: "IX_ordens_servico_ClienteId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ordens_servico_observacoes",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ordens_servico_observacoes",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "ordem_servico_id",
                table: "ordens_servico_observacoes",
                newName: "OrdemServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_os_observacoes_ordem_servico_id",
                table: "ordens_servico_observacoes",
                newName: "IX_ordens_servico_observacoes_OrdemServicoId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ordens_servico_itens",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "quantidade",
                table: "ordens_servico_itens",
                newName: "Quantidade");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ordens_servico_itens",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "valor_unitario",
                table: "ordens_servico_itens",
                newName: "ValorUnitario");

            migrationBuilder.RenameColumn(
                name: "peca_id",
                table: "ordens_servico_itens",
                newName: "PecaId");

            migrationBuilder.RenameColumn(
                name: "ordem_servico_id",
                table: "ordens_servico_itens",
                newName: "OrdemServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_os_itens_peca_id",
                table: "ordens_servico_itens",
                newName: "IX_ordens_servico_itens_PecaId");

            migrationBuilder.RenameIndex(
                name: "IX_os_itens_ordem_servico_id",
                table: "ordens_servico_itens",
                newName: "IX_ordens_servico_itens_OrdemServicoId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ordens_servico_checklists",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ordens_servico_checklists",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "ordem_servico_id",
                table: "ordens_servico_checklists",
                newName: "OrdemServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_os_checklists_ordem_servico_id",
                table: "ordens_servico_checklists",
                newName: "IX_ordens_servico_checklists_OrdemServicoId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ordens_servico_avaliacoes",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ordens_servico_avaliacoes",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "ordem_servico_id",
                table: "ordens_servico_avaliacoes",
                newName: "OrdemServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_os_avaliacoes_ordem_servico_id",
                table: "ordens_servico_avaliacoes",
                newName: "IX_ordens_servico_avaliacoes_OrdemServicoId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ordens_servico_anexos",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "data_upload",
                table: "ordens_servico_anexos",
                newName: "Data_Upload");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ordens_servico_anexos",
                newName: "Created_At");

            migrationBuilder.RenameColumn(
                name: "ordem_servico_id",
                table: "ordens_servico_anexos",
                newName: "OrdemServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_os_anexos_ordem_servico_id",
                table: "ordens_servico_anexos",
                newName: "IX_ordens_servico_anexos_OrdemServicoId");

            migrationBuilder.RenameColumn(
                name: "Resumo_Atividades",
                table: "mecanico_experiencias",
                newName: "ResumoAtividades");

            migrationBuilder.RenameColumn(
                name: "Mecanico_Id",
                table: "mecanico_experiencias",
                newName: "MecanicoId");

            migrationBuilder.RenameIndex(
                name: "IX_cad_mecanicos_experiencias_Mecanico_Id",
                table: "mecanico_experiencias",
                newName: "IX_mecanico_experiencias_MecanicoId");

            migrationBuilder.RenameColumn(
                name: "Mecanico_Id",
                table: "mecanico_especialidades_rel",
                newName: "MecanicoId");

            migrationBuilder.RenameColumn(
                name: "Especialidade_Id",
                table: "mecanico_especialidades_rel",
                newName: "EspecialidadeId");

            migrationBuilder.RenameIndex(
                name: "IX_cad_mecanicos_especialidades_rel_Mecanico_Id",
                table: "mecanico_especialidades_rel",
                newName: "IX_mecanico_especialidades_rel_MecanicoId");

            migrationBuilder.RenameIndex(
                name: "IX_cad_mecanicos_especialidades_rel_Especialidade_Id",
                table: "mecanico_especialidades_rel",
                newName: "IX_mecanico_especialidades_rel_EspecialidadeId");

            migrationBuilder.RenameColumn(
                name: "Mecanico_Id",
                table: "mecanico_enderecos",
                newName: "MecanicoId");

            migrationBuilder.RenameIndex(
                name: "IX_cad_mecanicos_enderecos_Mecanico_Id",
                table: "mecanico_enderecos",
                newName: "IX_mecanico_enderecos_MecanicoId");

            migrationBuilder.RenameColumn(
                name: "Orgao_Expedidor",
                table: "mecanico_documentos",
                newName: "OrgaoExpedidor");

            migrationBuilder.RenameColumn(
                name: "Mecanico_Id",
                table: "mecanico_documentos",
                newName: "MecanicoId");

            migrationBuilder.RenameColumn(
                name: "Arquivo_Url",
                table: "mecanico_documentos",
                newName: "ArquivoUrl");

            migrationBuilder.RenameIndex(
                name: "IX_cad_mecanicos_documentos_Mecanico_Id",
                table: "mecanico_documentos",
                newName: "IX_mecanico_documentos_MecanicoId");

            migrationBuilder.RenameColumn(
                name: "Mecanico_Id",
                table: "mecanico_disponibilidades",
                newName: "MecanicoId");

            migrationBuilder.RenameColumn(
                name: "Dia_Semana",
                table: "mecanico_disponibilidades",
                newName: "DiaSemana");

            migrationBuilder.RenameColumn(
                name: "Capacidade_Atendimentos",
                table: "mecanico_disponibilidades",
                newName: "CapacidadeAtendimentos");

            migrationBuilder.RenameIndex(
                name: "IX_cad_mecanicos_disponibilidades_Mecanico_Id",
                table: "mecanico_disponibilidades",
                newName: "IX_mecanico_disponibilidades_MecanicoId");

            migrationBuilder.RenameColumn(
                name: "Mecanico_Id",
                table: "mecanico_contatos",
                newName: "MecanicoId");

            migrationBuilder.RenameIndex(
                name: "IX_cad_mecanicos_contatos_Mecanico_Id",
                table: "mecanico_contatos",
                newName: "IX_mecanico_contatos_MecanicoId");

            migrationBuilder.RenameColumn(
                name: "Mecanico_Id",
                table: "mecanico_certificacoes",
                newName: "MecanicoId");

            migrationBuilder.RenameColumn(
                name: "Especialidade_Id",
                table: "mecanico_certificacoes",
                newName: "EspecialidadeId");

            migrationBuilder.RenameColumn(
                name: "Codigo_Certificacao",
                table: "mecanico_certificacoes",
                newName: "CodigoCertificacao");

            migrationBuilder.RenameIndex(
                name: "IX_cad_mecanicos_certificacoes_Mecanico_Id",
                table: "mecanico_certificacoes",
                newName: "IX_mecanico_certificacoes_MecanicoId");

            migrationBuilder.RenameIndex(
                name: "IX_cad_mecanicos_certificacoes_Especialidade_Id",
                table: "mecanico_certificacoes",
                newName: "IX_mecanico_certificacoes_EspecialidadeId");

            migrationBuilder.RenameColumn(
                name: "Valor_Hora",
                table: "mecanicos",
                newName: "ValorHora");

            migrationBuilder.RenameColumn(
                name: "Tipo_Documento",
                table: "mecanicos",
                newName: "TipoDocumento");

            migrationBuilder.RenameColumn(
                name: "Nome_Social",
                table: "mecanicos",
                newName: "NomeSocial");

            migrationBuilder.RenameColumn(
                name: "Especialidade_Principal_Id",
                table: "mecanicos",
                newName: "EspecialidadePrincipalId");

            migrationBuilder.RenameColumn(
                name: "Documento_Principal",
                table: "mecanicos",
                newName: "DocumentoPrincipal");

            migrationBuilder.RenameColumn(
                name: "Data_Nascimento",
                table: "mecanicos",
                newName: "DataNascimento");

            migrationBuilder.RenameColumn(
                name: "Carga_Horaria_Semanal",
                table: "mecanicos",
                newName: "CargaHorariaSemanal");

            migrationBuilder.RenameIndex(
                name: "IX_cad_mecanicos_Especialidade_Principal_Id",
                table: "mecanicos",
                newName: "IX_mecanicos_EspecialidadePrincipalId");

            migrationBuilder.RenameColumn(
                name: "Segmento_Id",
                table: "fornecedor_segmentos_rel",
                newName: "SegmentoId");

            migrationBuilder.RenameColumn(
                name: "Fornecedor_Id",
                table: "fornecedor_segmentos_rel",
                newName: "FornecedorId");

            migrationBuilder.RenameIndex(
                name: "IX_cad_fornecedores_segmentos_rel_Segmento_Id",
                table: "fornecedor_segmentos_rel",
                newName: "IX_fornecedor_segmentos_rel_SegmentoId");

            migrationBuilder.RenameIndex(
                name: "IX_cad_fornecedores_segmentos_rel_Fornecedor_Id",
                table: "fornecedor_segmentos_rel",
                newName: "IX_fornecedor_segmentos_rel_FornecedorId");

            migrationBuilder.RenameColumn(
                name: "Preferencia_Contato",
                table: "fornecedor_representantes",
                newName: "PreferenciaContato");

            migrationBuilder.RenameColumn(
                name: "Fornecedor_Id",
                table: "fornecedor_representantes",
                newName: "FornecedorId");

            migrationBuilder.RenameIndex(
                name: "IX_cad_fornecedores_representantes_Fornecedor_Id",
                table: "fornecedor_representantes",
                newName: "IX_fornecedor_representantes_FornecedorId");

            migrationBuilder.RenameColumn(
                name: "Fornecedor_Id",
                table: "fornecedor_enderecos",
                newName: "FornecedorId");

            migrationBuilder.RenameIndex(
                name: "IX_cad_fornecedores_enderecos_Fornecedor_Id",
                table: "fornecedor_enderecos",
                newName: "IX_fornecedor_enderecos_FornecedorId");

            migrationBuilder.RenameColumn(
                name: "Orgao_Expedidor",
                table: "fornecedor_documentos",
                newName: "OrgaoExpedidor");

            migrationBuilder.RenameColumn(
                name: "Fornecedor_Id",
                table: "fornecedor_documentos",
                newName: "FornecedorId");

            migrationBuilder.RenameColumn(
                name: "Arquivo_Url",
                table: "fornecedor_documentos",
                newName: "ArquivoUrl");

            migrationBuilder.RenameIndex(
                name: "IX_cad_fornecedores_documentos_Fornecedor_Id",
                table: "fornecedor_documentos",
                newName: "IX_fornecedor_documentos_FornecedorId");

            migrationBuilder.RenameColumn(
                name: "Fornecedor_Id",
                table: "fornecedor_contatos",
                newName: "FornecedorId");

            migrationBuilder.RenameIndex(
                name: "IX_cad_fornecedores_contatos_Fornecedor_Id",
                table: "fornecedor_contatos",
                newName: "IX_fornecedor_contatos_FornecedorId");

            migrationBuilder.RenameColumn(
                name: "Fornecedor_Id",
                table: "fornecedor_certificacoes",
                newName: "FornecedorId");

            migrationBuilder.RenameColumn(
                name: "Codigo_Certificacao",
                table: "fornecedor_certificacoes",
                newName: "CodigoCertificacao");

            migrationBuilder.RenameIndex(
                name: "IX_cad_fornecedores_certificacoes_Fornecedor_Id",
                table: "fornecedor_certificacoes",
                newName: "IX_fornecedor_certificacoes_FornecedorId");

            migrationBuilder.RenameColumn(
                name: "Tipo_Conta",
                table: "fornecedor_bancos",
                newName: "TipoConta");

            migrationBuilder.RenameColumn(
                name: "Pix_Chave",
                table: "fornecedor_bancos",
                newName: "PixChave");

            migrationBuilder.RenameColumn(
                name: "Fornecedor_Id",
                table: "fornecedor_bancos",
                newName: "FornecedorId");

            migrationBuilder.RenameIndex(
                name: "IX_cad_fornecedores_bancos_Fornecedor_Id",
                table: "fornecedor_bancos",
                newName: "IX_fornecedor_bancos_FornecedorId");

            migrationBuilder.RenameColumn(
                name: "Fornecedor_Id",
                table: "fornecedor_avaliacoes",
                newName: "FornecedorId");

            migrationBuilder.RenameColumn(
                name: "Avaliado_Por",
                table: "fornecedor_avaliacoes",
                newName: "AvaliadoPor");

            migrationBuilder.RenameIndex(
                name: "IX_cad_fornecedores_avaliacoes_Fornecedor_Id",
                table: "fornecedor_avaliacoes",
                newName: "IX_fornecedor_avaliacoes_FornecedorId");

            migrationBuilder.RenameColumn(
                name: "Termos_Negociados",
                table: "fornecedores",
                newName: "TermosNegociados");

            migrationBuilder.RenameColumn(
                name: "Telefone_Principal",
                table: "fornecedores",
                newName: "TelefonePrincipal");

            migrationBuilder.RenameColumn(
                name: "Segmento_Principal_Id",
                table: "fornecedores",
                newName: "SegmentoPrincipalId");

            migrationBuilder.RenameColumn(
                name: "Retirada_Local",
                table: "fornecedores",
                newName: "RetiradaLocal");

            migrationBuilder.RenameColumn(
                name: "Razao_Social",
                table: "fornecedores",
                newName: "RazaoSocial");

            migrationBuilder.RenameColumn(
                name: "Rating_Qualidade",
                table: "fornecedores",
                newName: "RatingQualidade");

            migrationBuilder.RenameColumn(
                name: "Rating_Logistica",
                table: "fornecedores",
                newName: "RatingLogistica");

            migrationBuilder.RenameColumn(
                name: "Prazo_Garantia_Padrao",
                table: "fornecedores",
                newName: "PrazoGarantiaPadrao");

            migrationBuilder.RenameColumn(
                name: "Prazo_Entrega_Medio",
                table: "fornecedores",
                newName: "PrazoEntregaMedio");

            migrationBuilder.RenameColumn(
                name: "Nota_Media",
                table: "fornecedores",
                newName: "NotaMedia");

            migrationBuilder.RenameColumn(
                name: "Nome_Fantasia",
                table: "fornecedores",
                newName: "NomeFantasia");

            migrationBuilder.RenameColumn(
                name: "Inscricao_Municipal",
                table: "fornecedores",
                newName: "InscricaoMunicipal");

            migrationBuilder.RenameColumn(
                name: "Inscricao_Estadual",
                table: "fornecedores",
                newName: "InscricaoEstadual");

            migrationBuilder.RenameColumn(
                name: "Condicao_Pagamento_Padrao",
                table: "fornecedores",
                newName: "CondicaoPagamentoPadrao");

            migrationBuilder.RenameColumn(
                name: "Atendimento_Personalizado",
                table: "fornecedores",
                newName: "AtendimentoPersonalizado");

            migrationBuilder.RenameIndex(
                name: "IX_cad_fornecedores_Segmento_Principal_Id",
                table: "fornecedores",
                newName: "IX_fornecedores_SegmentoPrincipalId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "fin_pagamentos",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "fin_lancamentos",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "fin_contas_receber",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "fin_contas_pagar",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "est_pecas_fornecedores",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecoUnitario",
                table: "est_pecas",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "EstoqueMinimo",
                table: "est_pecas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "EstoqueMaximo",
                table: "est_pecas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Valor",
                table: "ordens_servico_pagamentos",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorUnitario",
                table: "ordens_servico_itens",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<bool>(
                name: "Realizado",
                table: "ordens_servico_checklists",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "mecanico_especialidades_rel",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "Ativo",
                table: "mecanico_especialidades",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "mecanico_enderecos",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "CapacidadeAtendimentos",
                table: "mecanico_disponibilidades",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 5);

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "mecanico_contatos",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorHora",
                table: "mecanicos",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "CargaHorariaSemanal",
                table: "mecanicos",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 44);

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "fornecedor_segmentos_rel",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "Ativo",
                table: "fornecedor_segmentos",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "fornecedor_representantes",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "fornecedor_representantes",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(240)",
                oldMaxLength: 240,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "fornecedor_enderecos",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ArquivoUrl",
                table: "fornecedor_documentos",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(240)",
                oldMaxLength: 240,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "fornecedor_contatos",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Escopo",
                table: "fornecedor_certificacoes",
                type: "varchar(240)",
                maxLength: 240,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoCertificacao",
                table: "fornecedor_certificacoes",
                type: "varchar(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(60)",
                oldMaxLength: 60,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "Principal",
                table: "fornecedor_bancos",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "Nota",
                table: "fornecedor_avaliacoes",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Comentarios",
                table: "fornecedor_avaliacoes",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(400)",
                oldMaxLength: 400,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Categoria",
                table: "fornecedor_avaliacoes",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(60)",
                oldMaxLength: 60,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AvaliadoPor",
                table: "fornecedor_avaliacoes",
                type: "varchar(160)",
                maxLength: 160,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(120)",
                oldMaxLength: 120,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "fornecedores",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(600)",
                oldMaxLength: 600,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "TermosNegociados",
                table: "fornecedores",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(240)",
                oldMaxLength: 240,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "RetiradaLocal",
                table: "fornecedores",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "RatingQualidade",
                table: "fornecedores",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "RatingLogistica",
                table: "fornecedores",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "NotaMedia",
                table: "fornecedores",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AtendimentoPersonalizado",
                table: "fornecedores",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ordens_servico_pagamentos",
                table: "ordens_servico_pagamentos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ordens_servico_historico",
                table: "ordens_servico_historico",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ordens_servico",
                table: "ordens_servico",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ordens_servico_observacoes",
                table: "ordens_servico_observacoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ordens_servico_itens",
                table: "ordens_servico_itens",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ordens_servico_checklists",
                table: "ordens_servico_checklists",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ordens_servico_avaliacoes",
                table: "ordens_servico_avaliacoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ordens_servico_anexos",
                table: "ordens_servico_anexos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mecanico_experiencias",
                table: "mecanico_experiencias",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mecanico_especialidades_rel",
                table: "mecanico_especialidades_rel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mecanico_especialidades",
                table: "mecanico_especialidades",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mecanico_enderecos",
                table: "mecanico_enderecos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mecanico_documentos",
                table: "mecanico_documentos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mecanico_disponibilidades",
                table: "mecanico_disponibilidades",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mecanico_contatos",
                table: "mecanico_contatos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mecanico_certificacoes",
                table: "mecanico_certificacoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mecanicos",
                table: "mecanicos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fornecedor_segmentos_rel",
                table: "fornecedor_segmentos_rel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fornecedor_segmentos",
                table: "fornecedor_segmentos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fornecedor_representantes",
                table: "fornecedor_representantes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fornecedor_enderecos",
                table: "fornecedor_enderecos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fornecedor_documentos",
                table: "fornecedor_documentos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fornecedor_contatos",
                table: "fornecedor_contatos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fornecedor_certificacoes",
                table: "fornecedor_certificacoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fornecedor_bancos",
                table: "fornecedor_bancos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fornecedor_avaliacoes",
                table: "fornecedor_avaliacoes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fornecedores",
                table: "fornecedores",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_est_movimentacoes_est_pecas_PecaId",
                table: "est_movimentacoes",
                column: "PecaId",
                principalTable: "est_pecas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_est_categorias_CategoriaId",
                table: "est_pecas",
                column: "CategoriaId",
                principalTable: "est_categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_est_fabricantes_FabricanteId",
                table: "est_pecas",
                column: "FabricanteId",
                principalTable: "est_fabricantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_est_localizacoes_LocalizacaoId",
                table: "est_pecas",
                column: "LocalizacaoId",
                principalTable: "est_localizacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_anexos_est_pecas_PecaId",
                table: "est_pecas_anexos",
                column: "PecaId",
                principalTable: "est_pecas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_fornecedores_est_pecas_PecaId",
                table: "est_pecas_fornecedores",
                column: "PecaId",
                principalTable: "est_pecas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_fornecedores_fornecedores_FornecedorId",
                table: "est_pecas_fornecedores",
                column: "FornecedorId",
                principalTable: "fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_est_pecas_historico_est_pecas_PecaId",
                table: "est_pecas_historico",
                column: "PecaId",
                principalTable: "est_pecas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_anexos_fin_contas_pagar_ContaPagarId",
                table: "fin_anexos",
                column: "ContaPagarId",
                principalTable: "fin_contas_pagar",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_anexos_fin_contas_receber_ContaReceberId",
                table: "fin_anexos",
                column: "ContaReceberId",
                principalTable: "fin_contas_receber",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_anexos_fin_pagamentos_PagamentoId",
                table: "fin_anexos",
                column: "PagamentoId",
                principalTable: "fin_pagamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_contas_pagar_fin_metodos_pagamento_MetodoId",
                table: "fin_contas_pagar",
                column: "MetodoId",
                principalTable: "fin_metodos_pagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_contas_pagar_fornecedores_FornecedorId",
                table: "fin_contas_pagar",
                column: "FornecedorId",
                principalTable: "fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_contas_receber_cad_clientes_ClienteId",
                table: "fin_contas_receber",
                column: "ClienteId",
                principalTable: "cad_clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_contas_receber_fin_metodos_pagamento_MetodoId",
                table: "fin_contas_receber",
                column: "MetodoId",
                principalTable: "fin_metodos_pagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_pagamentos_cad_clientes_ClienteId",
                table: "fin_pagamentos",
                column: "ClienteId",
                principalTable: "cad_clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_pagamentos_fin_metodos_pagamento_MetodoId",
                table: "fin_pagamentos",
                column: "MetodoId",
                principalTable: "fin_metodos_pagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_pagamentos_fornecedores_FornecedorId",
                table: "fin_pagamentos",
                column: "FornecedorId",
                principalTable: "fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fin_pagamentos_ordens_servico_OrdemServicoId",
                table: "fin_pagamentos",
                column: "OrdemServicoId",
                principalTable: "ordens_servico",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_fornecedor_avaliacoes_fornecedores_FornecedorId",
                table: "fornecedor_avaliacoes",
                column: "FornecedorId",
                principalTable: "fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fornecedor_bancos_fornecedores_FornecedorId",
                table: "fornecedor_bancos",
                column: "FornecedorId",
                principalTable: "fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fornecedor_certificacoes_fornecedores_FornecedorId",
                table: "fornecedor_certificacoes",
                column: "FornecedorId",
                principalTable: "fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fornecedor_contatos_fornecedores_FornecedorId",
                table: "fornecedor_contatos",
                column: "FornecedorId",
                principalTable: "fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fornecedor_documentos_fornecedores_FornecedorId",
                table: "fornecedor_documentos",
                column: "FornecedorId",
                principalTable: "fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fornecedor_enderecos_fornecedores_FornecedorId",
                table: "fornecedor_enderecos",
                column: "FornecedorId",
                principalTable: "fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fornecedor_representantes_fornecedores_FornecedorId",
                table: "fornecedor_representantes",
                column: "FornecedorId",
                principalTable: "fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fornecedor_segmentos_rel_fornecedor_segmentos_SegmentoId",
                table: "fornecedor_segmentos_rel",
                column: "SegmentoId",
                principalTable: "fornecedor_segmentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fornecedor_segmentos_rel_fornecedores_FornecedorId",
                table: "fornecedor_segmentos_rel",
                column: "FornecedorId",
                principalTable: "fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fornecedores_fornecedor_segmentos_SegmentoPrincipalId",
                table: "fornecedores",
                column: "SegmentoPrincipalId",
                principalTable: "fornecedor_segmentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_mecanico_certificacoes_mecanico_especialidades_Especialidade~",
                table: "mecanico_certificacoes",
                column: "EspecialidadeId",
                principalTable: "mecanico_especialidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_mecanico_certificacoes_mecanicos_MecanicoId",
                table: "mecanico_certificacoes",
                column: "MecanicoId",
                principalTable: "mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mecanico_contatos_mecanicos_MecanicoId",
                table: "mecanico_contatos",
                column: "MecanicoId",
                principalTable: "mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mecanico_disponibilidades_mecanicos_MecanicoId",
                table: "mecanico_disponibilidades",
                column: "MecanicoId",
                principalTable: "mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mecanico_documentos_mecanicos_MecanicoId",
                table: "mecanico_documentos",
                column: "MecanicoId",
                principalTable: "mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mecanico_enderecos_mecanicos_MecanicoId",
                table: "mecanico_enderecos",
                column: "MecanicoId",
                principalTable: "mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mecanico_especialidades_rel_mecanico_especialidades_Especial~",
                table: "mecanico_especialidades_rel",
                column: "EspecialidadeId",
                principalTable: "mecanico_especialidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mecanico_especialidades_rel_mecanicos_MecanicoId",
                table: "mecanico_especialidades_rel",
                column: "MecanicoId",
                principalTable: "mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mecanico_experiencias_mecanicos_MecanicoId",
                table: "mecanico_experiencias",
                column: "MecanicoId",
                principalTable: "mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mecanicos_mecanico_especialidades_EspecialidadePrincipalId",
                table: "mecanicos",
                column: "EspecialidadePrincipalId",
                principalTable: "mecanico_especialidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ordens_servico_cad_clientes_ClienteId",
                table: "ordens_servico",
                column: "ClienteId",
                principalTable: "cad_clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ordens_servico_mecanicos_MecanicoId",
                table: "ordens_servico",
                column: "MecanicoId",
                principalTable: "mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ordens_servico_anexos_ordens_servico_OrdemServicoId",
                table: "ordens_servico_anexos",
                column: "OrdemServicoId",
                principalTable: "ordens_servico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ordens_servico_avaliacoes_ordens_servico_OrdemServicoId",
                table: "ordens_servico_avaliacoes",
                column: "OrdemServicoId",
                principalTable: "ordens_servico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ordens_servico_checklists_ordens_servico_OrdemServicoId",
                table: "ordens_servico_checklists",
                column: "OrdemServicoId",
                principalTable: "ordens_servico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ordens_servico_historico_ordens_servico_OrdemServicoId",
                table: "ordens_servico_historico",
                column: "OrdemServicoId",
                principalTable: "ordens_servico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ordens_servico_itens_est_pecas_PecaId",
                table: "ordens_servico_itens",
                column: "PecaId",
                principalTable: "est_pecas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ordens_servico_itens_ordens_servico_OrdemServicoId",
                table: "ordens_servico_itens",
                column: "OrdemServicoId",
                principalTable: "ordens_servico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ordens_servico_observacoes_ordens_servico_OrdemServicoId",
                table: "ordens_servico_observacoes",
                column: "OrdemServicoId",
                principalTable: "ordens_servico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ordens_servico_pagamentos_ordens_servico_OrdemServicoId",
                table: "ordens_servico_pagamentos",
                column: "OrdemServicoId",
                principalTable: "ordens_servico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
