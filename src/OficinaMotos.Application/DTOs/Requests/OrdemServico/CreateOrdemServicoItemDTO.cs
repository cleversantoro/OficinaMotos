using OficinaMotos.Domain.Enums;

namespace OficinaMotos.Application.DTOs.Requests.OrdemServico
{
    public class CreateOrdemServicoItemDTO
    {
        public long OrdemServicoId { get; set; }
        public long? PecaId { get; set; }
        public long? AtividadeProfissionalId { get; set; }
        public long? PrestadorId { get; set; }
        public OrdemServicoEnum Status { get; set; } = OrdemServicoEnum.Pendente;
        public OrdemServicoTipo Tipo { get; set; } = OrdemServicoTipo.Servico;
        public string Descricao { get; set; } = string.Empty;
        public decimal Quantidade { get; set; } = 1;
        public decimal ValorUnitario { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; }
        public string? Observacoes { get; set; }
    }
}
