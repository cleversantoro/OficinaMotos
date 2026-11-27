namespace OficinaMotos.Application.DTOs.Requests.Pessoas
{
    public class UpdatePessoaPjDTO
    {
        public long PessoaId { get; set; }
        public string RazaoSocial { get; set; } = string.Empty;
        public string? NomeFantasia { get; set; }
        public string? InscricaoEstadual { get; set; }
        public string? InscricaoMunicipal { get; set; }
        public string? Responsavel { get; set; }
        public string? Website { get; set; }
    }
}
