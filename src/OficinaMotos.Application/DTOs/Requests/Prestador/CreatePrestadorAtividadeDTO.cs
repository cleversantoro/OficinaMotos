namespace OficinaMotos.Application.DTOs.Requests.Prestador
{
    public class CreatePrestadorAtividadeDTO
    {
        public long PrestadorId { get; set; }
        public long AtividadeProfissionalId { get; set; }
        public string? Nivel { get; set; }
        public bool Principal { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
