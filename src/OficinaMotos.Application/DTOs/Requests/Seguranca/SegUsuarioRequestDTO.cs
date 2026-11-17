namespace OficinaMotos.Application.DTOs.Requests.Seguranca
{
    public class CreateSegUsuarioDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        /// <summary>Senha em texto plano — será convertida em hash bcrypt.</summary>
        public string Senha { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public long PerfilId { get; set; }
    }

    public class UpdateSegUsuarioDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public int Status { get; set; }
        public long? PerfilId { get; set; }
    }

    public class AlterarSenhaDTO
    {
        public string SenhaAtual { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
    }

    public class RedefinirSenhaDTO
    {
        public string Token { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
    }
}
