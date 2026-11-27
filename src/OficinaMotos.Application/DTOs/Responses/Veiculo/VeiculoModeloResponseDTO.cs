using System;

namespace OficinaMotos.Application.DTOs.Responses.Veiculo
{
    public class VeiculoModeloResponseDTO
    {
        public long Id { get; set; }
        public long MarcaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? Cilindrada { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
