using System;

namespace OficinaMotos.Application.DTOs.Responses.Veiculo
{
    public class VeiculoMarcaResponseDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
