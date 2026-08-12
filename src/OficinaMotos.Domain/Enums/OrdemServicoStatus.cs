using System;

namespace OficinaMotos.Domain.Enums
{
    public enum OrdemServicoStatus
    {
        Aberta = 1,
        EmAndamento = 2,
        AguardandoPeca = 3,
        Concluida = 4,
        Cancelada = 5
    }

    public static class OrdemServicoStatusExtensions
    {
        public static OrdemServicoStatus ParseLegacy(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return OrdemServicoStatus.Aberta;

            var normalized = value.Trim();

            if (Enum.TryParse<OrdemServicoStatus>(normalized, true, out var parsed))
                return parsed;

            var compact = normalized
                .Replace("_", "")
                .Replace("-", "")
                .Replace(" ", "")
                .Replace(".", "")
                .ToUpperInvariant();

            return compact switch
            {
                "ABERTA" => OrdemServicoStatus.Aberta,
                "EMANDAMENTO" => OrdemServicoStatus.EmAndamento,
                "AGUARDANDOPECA" or "AGUARDANDOAPROVACAO" => OrdemServicoStatus.AguardandoPeca,
                "CONCLUIDA" => OrdemServicoStatus.Concluida,
                "CANCELADA" => OrdemServicoStatus.Cancelada,
                _ => Enum.TryParse<OrdemServicoStatus>(compact, true, out var fallback) ? fallback : OrdemServicoStatus.Aberta
            };
        }
    }
}
