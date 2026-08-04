START TRANSACTION;

ALTER TABLE `seg_usuarios_perfis` DROP COLUMN `DeletedAt`;

ALTER TABLE `seg_usuarios_perfis` DROP COLUMN `IsDeleted`;

ALTER TABLE `seg_usuarios` DROP COLUMN `DeletedAt`;

ALTER TABLE `seg_usuarios` DROP COLUMN `IsDeleted`;

ALTER TABLE `seg_refresh_tokens` DROP COLUMN `DeletedAt`;

ALTER TABLE `seg_refresh_tokens` DROP COLUMN `IsDeleted`;

ALTER TABLE `seg_permissoes` DROP COLUMN `DeletedAt`;

ALTER TABLE `seg_permissoes` DROP COLUMN `IsDeleted`;

ALTER TABLE `seg_perfis_permissoes` DROP COLUMN `DeletedAt`;

ALTER TABLE `seg_perfis_permissoes` DROP COLUMN `IsDeleted`;

ALTER TABLE `seg_perfis` DROP COLUMN `DeletedAt`;

ALTER TABLE `seg_perfis` DROP COLUMN `IsDeleted`;

ALTER TABLE `seg_modulos` DROP COLUMN `DeletedAt`;

ALTER TABLE `seg_modulos` DROP COLUMN `IsDeleted`;

ALTER TABLE `seg_audit_log` DROP COLUMN `DeletedAt`;

ALTER TABLE `seg_audit_log` DROP COLUMN `IsDeleted`;

ALTER TABLE `os_pagamentos` DROP COLUMN `DeletedAt`;

ALTER TABLE `os_pagamentos` DROP COLUMN `IsDeleted`;

ALTER TABLE `os_ordens_historico` DROP COLUMN `DeletedAt`;

ALTER TABLE `os_ordens_historico` DROP COLUMN `IsDeleted`;

ALTER TABLE `os_ordens` DROP COLUMN `DeletedAt`;

ALTER TABLE `os_ordens` DROP COLUMN `IsDeleted`;

ALTER TABLE `os_observacoes` DROP COLUMN `DeletedAt`;

ALTER TABLE `os_observacoes` DROP COLUMN `IsDeleted`;

ALTER TABLE `os_itens` DROP COLUMN `DeletedAt`;

ALTER TABLE `os_itens` DROP COLUMN `IsDeleted`;

ALTER TABLE `os_checklists` DROP COLUMN `DeletedAt`;

ALTER TABLE `os_checklists` DROP COLUMN `IsDeleted`;

ALTER TABLE `os_avaliacoes` DROP COLUMN `DeletedAt`;

ALTER TABLE `os_avaliacoes` DROP COLUMN `IsDeleted`;

ALTER TABLE `os_anexos` DROP COLUMN `DeletedAt`;

ALTER TABLE `os_anexos` DROP COLUMN `IsDeleted`;

ALTER TABLE `fin_pagamentos` DROP COLUMN `DeletedAt`;

ALTER TABLE `fin_pagamentos` DROP COLUMN `IsDeleted`;

ALTER TABLE `fin_metodos_pagamento` DROP COLUMN `DeletedAt`;

ALTER TABLE `fin_metodos_pagamento` DROP COLUMN `IsDeleted`;

ALTER TABLE `fin_lancamentos` DROP COLUMN `DeletedAt`;

ALTER TABLE `fin_lancamentos` DROP COLUMN `IsDeleted`;

ALTER TABLE `fin_historico` DROP COLUMN `DeletedAt`;

ALTER TABLE `fin_historico` DROP COLUMN `IsDeleted`;

ALTER TABLE `fin_contas_receber` DROP COLUMN `DeletedAt`;

ALTER TABLE `fin_contas_receber` DROP COLUMN `IsDeleted`;

ALTER TABLE `fin_contas_pagar` DROP COLUMN `DeletedAt`;

ALTER TABLE `fin_contas_pagar` DROP COLUMN `IsDeleted`;

ALTER TABLE `fin_anexos` DROP COLUMN `DeletedAt`;

ALTER TABLE `fin_anexos` DROP COLUMN `IsDeleted`;

ALTER TABLE `est_pecas_historico` DROP COLUMN `DeletedAt`;

ALTER TABLE `est_pecas_historico` DROP COLUMN `IsDeleted`;

ALTER TABLE `est_pecas_fornecedores` DROP COLUMN `DeletedAt`;

ALTER TABLE `est_pecas_fornecedores` DROP COLUMN `IsDeleted`;

ALTER TABLE `est_pecas_anexos` DROP COLUMN `DeletedAt`;

ALTER TABLE `est_pecas_anexos` DROP COLUMN `IsDeleted`;

ALTER TABLE `est_pecas` DROP COLUMN `DeletedAt`;

ALTER TABLE `est_pecas` DROP COLUMN `IsDeleted`;

ALTER TABLE `est_movimentacoes` DROP COLUMN `DeletedAt`;

ALTER TABLE `est_movimentacoes` DROP COLUMN `IsDeleted`;

ALTER TABLE `est_localizacoes` DROP COLUMN `DeletedAt`;

ALTER TABLE `est_localizacoes` DROP COLUMN `IsDeleted`;

ALTER TABLE `est_fabricantes` DROP COLUMN `DeletedAt`;

ALTER TABLE `est_fabricantes` DROP COLUMN `IsDeleted`;

ALTER TABLE `est_categorias` DROP COLUMN `DeletedAt`;

ALTER TABLE `est_categorias` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_veiculos_modelos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_veiculos_modelos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_veiculos_marcas` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_veiculos_marcas` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_veiculos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_veiculos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_mecanicos_experiencias` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_mecanicos_experiencias` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_mecanicos_especialidades_rel` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_mecanicos_especialidades_rel` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_mecanicos_especialidades` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_mecanicos_especialidades` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_mecanicos_enderecos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_mecanicos_enderecos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_mecanicos_documentos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_mecanicos_documentos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_mecanicos_disponibilidades` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_mecanicos_disponibilidades` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_mecanicos_contatos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_mecanicos_contatos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_mecanicos_certificacoes` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_mecanicos_certificacoes` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_mecanicos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_mecanicos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_fornecedores_segmentos_rel` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_fornecedores_segmentos_rel` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_fornecedores_segmentos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_fornecedores_segmentos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_fornecedores_representantes` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_fornecedores_representantes` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_fornecedores_enderecos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_fornecedores_enderecos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_fornecedores_documentos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_fornecedores_documentos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_fornecedores_contatos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_fornecedores_contatos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_fornecedores_certificacoes` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_fornecedores_certificacoes` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_fornecedores_bancos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_fornecedores_bancos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_fornecedores_avaliacoes` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_fornecedores_avaliacoes` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_fornecedores` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_fornecedores` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_clientes_pj` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_clientes_pj` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_clientes_pf` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_clientes_pf` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_clientes_origens` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_clientes_origens` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_clientes_lgpd_consentimentos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_clientes_lgpd_consentimentos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_clientes_indicacoes` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_clientes_indicacoes` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_clientes_financeiro` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_clientes_financeiro` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_clientes_enderecos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_clientes_enderecos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_clientes_documentos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_clientes_documentos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_clientes_contatos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_clientes_contatos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_clientes_anexos` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_clientes_anexos` DROP COLUMN `IsDeleted`;

ALTER TABLE `cad_clientes` DROP COLUMN `DeletedAt`;

ALTER TABLE `cad_clientes` DROP COLUMN `IsDeleted`;

DELETE FROM `__EFMigrationsHistory`
WHERE `MigrationId` = '20260804044157_AddSoftDeleteToBaseEntity';

COMMIT;

