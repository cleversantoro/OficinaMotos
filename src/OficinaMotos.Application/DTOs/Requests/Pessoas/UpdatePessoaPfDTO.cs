using System;

namespace OficinaMotos.Application.DTOs.Requests.Pessoas
{
    public class UpdatePessoaPfDTO
    {
        public long PessoaId { get; set; }
        public string? Rg { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Genero { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Profissao { get; set; }
    }
}
