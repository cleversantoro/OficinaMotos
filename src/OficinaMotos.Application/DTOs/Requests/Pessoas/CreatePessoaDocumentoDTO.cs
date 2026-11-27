using System;

namespace OficinaMotos.Application.DTOs.Requests.Pessoas
{
    public class CreatePessoaDocumentoDTO
    {
        public long PessoaId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? OrgaoExpedidor { get; set; }
        public string? ArquivoUrl { get; set; }
        public string? Observacoes { get; set; }
    }
}
