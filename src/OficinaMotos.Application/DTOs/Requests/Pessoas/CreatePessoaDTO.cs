using System;
using System.Collections.Generic;

namespace OficinaMotos.Application.DTOs.Requests.Pessoas
{
    public class CreatePessoaDTO
    {
        public string TipoPessoa { get; set; } = "PF";
        public string Documento { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? NomeExibicao { get; set; }
        public bool Ativo { get; set; } = true;
        public string? Observacoes { get; set; }
        public PessoaPfDTO? PessoaFisica { get; set; }
        public PessoaPjDTO? PessoaJuridica { get; set; }
        public List<PessoaContatoDTO> Contatos { get; set; } = new();
        public List<PessoaEnderecoDTO> Enderecos { get; set; } = new();
        public List<PessoaDocumentoDTO> Documentos { get; set; } = new();
        public List<PessoaAnexoDTO> Anexos { get; set; } = new();
    }

    public class PessoaPfDTO
    {
        public string? Rg { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Genero { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Profissao { get; set; }
    }

    public class PessoaPjDTO
    {
        public string RazaoSocial { get; set; } = string.Empty;
        public string? NomeFantasia { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? Responsavel { get; set; }
        public string? Website { get; set; }
    }

    public class PessoaContatoDTO
    {
        public string Tipo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public bool Principal { get; set; }
        public string? Observacao { get; set; }
    }

    public class PessoaEnderecoDTO
    {
        public string Tipo { get; set; } = "PRINCIPAL";
        public string? Cep { get; set; }
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Pais { get; set; } = "Brasil";
        public string? Complemento { get; set; }
        public bool Principal { get; set; }
        public string? Observacao { get; set; }
    }

    public class PessoaDocumentoDTO
    {
        public string Tipo { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string? OrgaoExpedidor { get; set; }
        public string? ArquivoUrl { get; set; }
        public string? Observacoes { get; set; }
    }

    public class PessoaAnexoDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public long? TamanhoBytes { get; set; }
        public string? MimeType { get; set; }
    }
}
