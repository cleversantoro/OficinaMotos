namespace OficinaMotos.Application.DTOs.Responses.OrdemServicoDTO
{
    public class OrdemServicoItemResponseDTO
    {
        public long Id { get; set; }
        public long OrdemServicoId { get; set; }
        public long? PecaId { get; set; }
        public long? AtividadeProfissionalId { get; set; }
        public long? PrestadorId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Total => Quantidade * ValorUnitario;
    }
}
