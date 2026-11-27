using OficinaMotos.Domain.Enums;

namespace OficinaMotos.Application.DTOs.Requests.OrdemServico
{
    public class CreateOrdemServicoAnexoDTO
    {
        public long OrdemServicoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public OrdemServicoAnexoTipo Tipo { get; set; } = OrdemServicoAnexoTipo.Imagem;
        public string Url { get; set; } = string.Empty;
        public string? Observacao { get; set; }
    }
}
