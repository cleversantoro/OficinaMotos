namespace OficinaMotos.Application.DTOs.Requests.Veiculo
{
    public class UpdateVeiculoModeloDTO
    {
        public long MarcaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? Cilindrada { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
