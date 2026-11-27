using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Pessoas;

namespace OficinaMotos.Application.Validators.Pessoas
{
    public class UpdatePessoaValidator : AbstractValidator<UpdatePessoaDTO>
    {
        public UpdatePessoaValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().MinimumLength(3).MaximumLength(180);
            RuleFor(x => x.Documento).NotEmpty().MaximumLength(20);
            RuleFor(x => x.TipoPessoa).Must(x => x is "PF" or "PJ");
            RuleFor(x => x.PessoaJuridica)
                .Must(x => x != null && !string.IsNullOrWhiteSpace(x.RazaoSocial))
                .When(x => x.TipoPessoa == "PJ");
        }
    }
}
