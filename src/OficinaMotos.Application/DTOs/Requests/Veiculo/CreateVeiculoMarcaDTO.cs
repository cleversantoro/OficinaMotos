namespace OficinaMotos.Application.DTOs.Requests.Veiculo
{
    public class CreateVeiculoMarcaDTO
    {
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}
