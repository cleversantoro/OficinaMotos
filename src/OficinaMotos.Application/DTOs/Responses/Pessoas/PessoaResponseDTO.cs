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

    public class PessoaPfResponseDTO : PessoaPfFieldsDTO { }
    public class PessoaPjResponseDTO : PessoaPjFieldsDTO { }

    public class PessoaPfFieldsDTO
    {
        public string? Rg { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Genero { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Profissao { get; set; }
    }

    public class PessoaPjFieldsDTO
    {
        public string RazaoSocial { get; set; } = string.Empty;
        public string? NomeFantasia { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? Responsavel { get; set; }
        public string? Website { get; set; }
    }

    public class PessoaContatoResponseDTO
    {
        public long Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public bool Principal { get; set; }
        public string? Observacao { get; set; }
    }

    public class PessoaEnderecoResponseDTO
    {
        public long Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? Cep { get; set; }
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public bool Principal { get; set; }
        public string? Observacao { get; set; }
    }

    public class PessoaDocumentoResponseDTO
    {
        public long Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? OrgaoExpedidor { get; set; }
        public string? ArquivoUrl { get; set; }
        public string? Observacoes { get; set; }
    }

    public class PessoaAnexoResponseDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public long? TamanhoBytes { get; set; }
        public string? MimeType { get; set; }
    }
}
