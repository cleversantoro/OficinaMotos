using System;
using System.Collections.Generic;

namespace OficinaMotos.Application.DTOs.Responses.Pessoas
{
    public class PessoaResponseDTO
    {
        public long Id { get; set; }
        public long Codigo { get; set; }
        public string TipoPessoa { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public string? Observacoes { get; set; }
        public PessoaPfResponseDTO? PessoaFisica { get; set; }
        public PessoaPjResponseDTO? PessoaJuridica { get; set; }
        public List<PessoaContatoResponseDTO> Contatos { get; set; } = new();
        public List<PessoaEnderecoResponseDTO> Enderecos { get; set; } = new();
        public List<PessoaDocumentoResponseDTO> Documentos { get; set; } = new();
        public List<PessoaAnexoResponseDTO> Anexos { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
