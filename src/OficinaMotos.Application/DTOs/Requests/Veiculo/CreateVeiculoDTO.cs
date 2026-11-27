namespace OficinaMotos.Application.DTOs.Requests.Veiculo
{
    public class CreateVeiculoDTO
    {
        public long ClienteId { get; set; }
        public long? MarcaId { get; set; }
        public long? ModeloId { get; set; }
        public string? Codigo { get; set; }
        public string? Placa { get; set; }
        public string? Renavam { get; set; }
        public string? Chassi { get; set; }
        public short? AnoFabricacao { get; set; }
        public short? AnoModelo { get; set; }
        public string? Cor { get; set; }
        public string? Combustivel { get; set; }
        public int? Quilometragem { get; set; }
        public string? Observacoes { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
