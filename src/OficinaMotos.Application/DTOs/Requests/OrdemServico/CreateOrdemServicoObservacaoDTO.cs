using OficinaMotos.Domain.Enums;

namespace OficinaMotos.Application.DTOs.Requests.OrdemServico
{
    public class CreateOrdemServicoObservacaoDTO
    {
        public long OrdemServicoId { get; set; }
        public long? UsuarioId { get; set; }
        public string Texto { get; set; } = string.Empty;
        public OrdemServicoObservacaoTipo Tipo { get; set; } = OrdemServicoObservacaoTipo.Interna;
    }
}
