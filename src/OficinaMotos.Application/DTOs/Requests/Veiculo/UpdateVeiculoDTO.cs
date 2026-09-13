namespace OficinaMotos.Application.DTOs.Requests.Veiculo
{
    public class UpdateVeiculoDTO
    {
        public long ClienteId { get; set; }
        public string Placa { get; set; } = string.Empty;
        public long? ModeloId { get; set; }
        public int? AnoFab { get; set; }
        public int? AnoMod { get; set; }
        public string? Cor { get; set; }
        public string? Chassi { get; set; }
        public string? Renavam { get; set; }
        public string? Km { get; set; }
        public int? ProximoKmRevisao { get; set; }
        public string? Combustivel { get; set; }
        public string? Observacao { get; set; }
        public bool Principal { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
