using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Pessoas;

namespace OficinaMotos.Application.Validators.Pessoas
{
    public class CreatePessoaValidator : AbstractValidator<CreatePessoaDTO>
    {
        public CreatePessoaValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().MinimumLength(3).MaximumLength(180);
            RuleFor(x => x.Documento).NotEmpty().MaximumLength(20);
            RuleFor(x => x.TipoPessoa).Must(x => x is "PF" or "PJ");
            RuleFor(x => x.PessoaJuridica)
                .Must(x => x != null && !string.IsNullOrWhiteSpace(x.RazaoSocial))
                .When(x => x.TipoPessoa == "PJ");
            RuleForEach(x => x.Enderecos).ChildRules(endereco =>
            {
                endereco.RuleFor(x => x.Logradouro).NotEmpty().MaximumLength(200);
                endereco.RuleFor(x => x.Numero).NotEmpty().MaximumLength(20);
                endereco.RuleFor(x => x.Bairro).NotEmpty().MaximumLength(120);
                endereco.RuleFor(x => x.Cidade).NotEmpty().MaximumLength(120);
                endereco.RuleFor(x => x.Estado).NotEmpty().MaximumLength(60);
            });
        }
    }

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
