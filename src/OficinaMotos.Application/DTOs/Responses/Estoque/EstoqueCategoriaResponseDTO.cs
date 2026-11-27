using System;

namespace OficinaMotos.Application.DTOs.Responses.Estoque
{
    public class EstoqueCategoriaResponseDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public long? CategoriaPaiId { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
