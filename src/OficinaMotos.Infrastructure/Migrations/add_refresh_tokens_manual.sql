-- Script manual para criar a tabela de refresh tokens no banco atual
-- Execute este script diretamente no MySQL caso a migração automática não consiga aplicar.

CREATE TABLE IF NOT EXISTS `seg_refresh_tokens` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Usuario_Id` bigint NOT NULL,
  `Token_Hash` varchar(255) NOT NULL,
  `Expira_Em` datetime(6) NOT NULL,
  `Revogado_Em` datetime(6) NULL,
  `Motivo_Revogacao` varchar(160) NULL,
  `Ip_Criacao` varchar(45) NULL,
  `User_Agent_Criacao` varchar(500) NULL,
  `Ultimo_Uso_Em` datetime(6) NULL,
  `Created_At` datetime(6) NOT NULL,
  `Updated_At` datetime(6) NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UQ_seg_refresh_tokens_hash` (`Token_Hash`),
  CONSTRAINT `FK_seg_refresh_tokens_seg_usuarios_Usuario_Id` FOREIGN KEY (`Usuario_Id`) REFERENCES `seg_usuarios` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
