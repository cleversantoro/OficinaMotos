-- Dados mínimos para validar login e criação de OrdemServico.
-- Credenciais locais: admin@oficina.com / admin123
-- Execute depois de aplicar as migrations no banco oficina_db.

START TRANSACTION;

SET @agora = UTC_TIMESTAMP(6);

INSERT INTO seg_perfis
    (Nome, Descricao, Nivel, Status, Created_At, Updated_At, DeletedAt, IsDeleted)
SELECT
    'Administrador',
    'Perfil administrativo para testes locais',
    1,
    1,
    @agora,
    NULL,
    NULL,
    0
WHERE NOT EXISTS (
    SELECT 1 FROM seg_perfis WHERE Nome = 'Administrador' AND IsDeleted = 0
);

SELECT Id INTO @perfil_id
FROM seg_perfis
WHERE Nome = 'Administrador' AND IsDeleted = 0
ORDER BY Id
LIMIT 1;

INSERT INTO seg_usuarios
    (Nome, Email, Login, Senha, Telefone, Foto_Url, Status, Ultimo_Login,
     Token_Reset, Token_Reset_Expira_Em, Tentativas_Login, Bloqueado_Ate,
     Criado_Por, Created_At, Updated_At, DeletedAt, IsDeleted)
SELECT
    'Administrador',
    'admin@oficina.com',
    'admin',
    '$2a$12$F8VXcHHHkTxV8m6jnebos.OGcV2LhbjSYahnywrlGAW4E4fiEn.56',
    '11999999999',
    NULL,
    1,
    NULL,
    NULL,
    NULL,
    0,
    NULL,
    NULL,
    @agora,
    NULL,
    NULL,
    0
WHERE NOT EXISTS (
    SELECT 1
    FROM seg_usuarios
    WHERE (Email = 'admin@oficina.com' OR Login = 'admin')
      AND IsDeleted = 0
);

SELECT Id INTO @usuario_id
FROM seg_usuarios
WHERE Email = 'admin@oficina.com' AND IsDeleted = 0
ORDER BY Id
LIMIT 1;

INSERT INTO seg_usuarios_perfis
    (Usuario_Id, Perfil_Id, Ativo, Created_At, Updated_At, DeletedAt, IsDeleted)
SELECT @usuario_id, @perfil_id, 1, @agora, NULL, NULL, 0
WHERE NOT EXISTS (
    SELECT 1
    FROM seg_usuarios_perfis
    WHERE Usuario_Id = @usuario_id AND Perfil_Id = @perfil_id AND IsDeleted = 0
);

INSERT INTO cad_clientes
    (Codigo, Nome, NomeExibicao, Documento, Tipo, Status, Vip, Observacoes,
     OrigemCadastroId, Telefone, Email, Origem_Id, Created_At, Updated_At,
     DeletedAt, IsDeleted)
SELECT
    1,
    'Cliente Teste',
    'Cliente Teste',
    '12345678901',
    1,
    1,
    0,
    'Cliente para testes locais da ordem de serviço',
    1,
    '11988887777',
    'cliente@teste.com',
    NULL,
    @agora,
    NULL,
    NULL,
    0
WHERE NOT EXISTS (
    SELECT 1 FROM cad_clientes WHERE Documento = '12345678901' AND IsDeleted = 0
);

SELECT Id INTO @cliente_id
FROM cad_clientes
WHERE Documento = '12345678901' AND IsDeleted = 0
ORDER BY Id
LIMIT 1;

INSERT INTO cad_mecanicos
    (Codigo, Nome, Sobrenome, Nome_Social, Documento_Principal, Tipo_Documento,
     Data_Nascimento, Data_Admissao, Data_Demissao, Status,
     Especialidade_Principal_Id, Nivel, Valor_Hora, Carga_Horaria_Semanal,
     Observacoes, Created_At, Updated_At, DeletedAt, IsDeleted)
SELECT
    'MEC-001',
    'Mecânico',
    'Teste',
    'Mecânico Teste',
    '98765432100',
    1,
    '1990-01-10 00:00:00',
    @agora,
    NULL,
    'Ativo',
    NULL,
    'Pleno',
    120.00,
    40,
    'Mecânico para testes locais da ordem de serviço',
    @agora,
    NULL,
    NULL,
    0
WHERE NOT EXISTS (
    SELECT 1 FROM cad_mecanicos
    WHERE Documento_Principal = '98765432100' AND IsDeleted = 0
);

SELECT Id INTO @mecanico_id
FROM cad_mecanicos
WHERE Documento_Principal = '98765432100' AND IsDeleted = 0
ORDER BY Id
LIMIT 1;

INSERT INTO cad_veiculos
    (Cliente_Id, Placa, Modelo_Id, Ano_Fab, Ano_Mod, Cor, Chassi, Renavam,
     Km, Combustivel, Observacao, Principal, Ativo, Created_At, Updated_At,
     DeletedAt, IsDeleted)
SELECT
    @cliente_id,
    'ABC1D23',
    NULL,
    2022,
    2023,
    'Preto',
    'CHASSI123456789',
    '12345678901',
    '15000',
    'Gasolina',
    'Veículo para testes locais da ordem de serviço',
    1,
    1,
    @agora,
    NULL,
    NULL,
    0
WHERE NOT EXISTS (
    SELECT 1 FROM cad_veiculos WHERE Placa = 'ABC1D23' AND IsDeleted = 0
);

SELECT Id INTO @veiculo_id
FROM cad_veiculos
WHERE Placa = 'ABC1D23' AND IsDeleted = 0
ORDER BY Id
LIMIT 1;

COMMIT;

SELECT
    @usuario_id AS UsuarioId,
    @perfil_id AS PerfilId,
    @cliente_id AS ClienteId,
    @mecanico_id AS MecanicoId,
    @veiculo_id AS VeiculoId;
