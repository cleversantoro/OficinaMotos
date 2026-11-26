using System;

namespace OficinaMotos.Domain.Enums
{
    public enum OrdemServicoEnum
    {
        Aberta = 1,
        EmAndamento = 2,
        AguardandoPeca = 3,
        Concluida = 4,
        Cancelada = 5,
        Pendente = 6,
    }

    public static class OrdemServicoStatusExtensions
    {
        public static OrdemServicoEnum ParseLegacy(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return OrdemServicoEnum.Aberta;

            var normalized = value.Trim();

            if (Enum.TryParse<OrdemServicoEnum>(normalized, true, out var parsed))
                return parsed;

            var compact = normalized
                .Replace("_", "")
                .Replace("-", "")
                .Replace(" ", "")
                .Replace(".", "")
                .ToUpperInvariant();

            return compact switch
            {
                "ABERTA" => OrdemServicoEnum.Aberta,
                "EMANDAMENTO" => OrdemServicoEnum.EmAndamento,
                "AGUARDANDOPECA" or "AGUARDANDOAPROVACAO" => OrdemServicoEnum.AguardandoPeca,
                "CONCLUIDA" => OrdemServicoEnum.Concluida,
                "CANCELADA" => OrdemServicoEnum.Cancelada,
                _ => Enum.TryParse<OrdemServicoEnum>(compact, true, out var fallback) ? fallback : OrdemServicoEnum.Aberta
            };
        }
    }

    public enum OrdemServicoPrioridade
    {
        Normal = 1,
        Alta = 2,
        Baixa = 3
    }

    public enum OrdemServicoTipo
    {
        Servico = 1
    }

    public enum OrdemServicoObservacaoTipo
    {
        Interna = 1,
        Externa = 2,
    }

    public enum OrdemServicoAnexoTipo
    {
        Imagem = 1,
        Documento = 2
    }

    public enum OrdemServicoPagamentoStatus
    {
        Pendente = 1,
        Pago = 2,
        Cancelado = 3
    }

}
