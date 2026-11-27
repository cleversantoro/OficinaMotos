using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Infrastructure.Repositories.PessoaRepo
{
    internal static class PessoaQueryHelper
    {
        public static void AplicarRegraTipoPessoa(Pessoa? pessoa)
        {
            if (pessoa == null) return;

            var tipo = pessoa.TipoPessoa?.Trim().ToUpperInvariant();
            if (tipo == "PF")
            {
                pessoa.PessoaJuridica = null;
            }
            else if (tipo == "PJ")
            {
                pessoa.PessoaFisica = null;
            }
        }
    }
}
