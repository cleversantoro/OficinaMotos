using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Pessoas;

namespace OficinaMotos.Application.Validators.Pessoas
{
    public class UpdatePessoaContatoValidator : AbstractValidator<UpdatePessoaContatoDTO>
    {
        public UpdatePessoaContatoValidator()
        {
            RuleFor(x => x.PessoaId).GreaterThan(0);
            RuleFor(x => x.Tipo).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Valor).NotEmpty().MaximumLength(180);
            RuleFor(x => x.Observacao).MaximumLength(255).When(x => !string.IsNullOrWhiteSpace(x.Observacao));
        }
    }
}
