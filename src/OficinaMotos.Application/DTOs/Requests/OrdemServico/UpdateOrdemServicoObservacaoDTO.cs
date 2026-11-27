using OficinaMotos.Application.DTOs.Responses.Seguranca;
using OficinaMotos.Domain.Enums;

namespace OficinaMotos.Application.DTOs.Requests.OrdemServico
{
    public class UpdateOrdemServicoObservacaoDTO
    {
        public long OrdemServicoId { get; set; }

        public long? UsuarioId { get; set; }
        public SegUsuarioResponseDTO? Usuario { get; set; }

        public string Texto { get; set; } = string.Empty;
        public OrdemServicoObservacaoTipo Tipo { get; set; } = OrdemServicoObservacaoTipo.Interna;
    }
}
