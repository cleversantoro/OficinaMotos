namespace OficinaMotos.Application.DTOs.Requests.OrdemServico
{
    public class UpdateOrdemServicoItemDTO
    {
        public long OrdemServicoId { get; set; }
        public long? PecaId { get; set; }
        public long? AtividadeProfissionalId { get; set; }
        public long? PrestadorId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
    }
}
