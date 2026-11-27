namespace OficinaMotos.Application.DTOs.Requests.Financeiro
{
    public class UpdateFinanceiroHistoricoDTO
    {
        public string Entidade { get; set; } = string.Empty;
        public long EntidadeId { get; set; }
        public string Acao { get; set; } = string.Empty;
        public string? DadosAnteriores { get; set; }
        public string? DadosNovos { get; set; }
        public long? UsuarioId { get; set; }
    }
}
