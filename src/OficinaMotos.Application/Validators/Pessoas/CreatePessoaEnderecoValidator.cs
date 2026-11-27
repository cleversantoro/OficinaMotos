using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Pessoas;

namespace OficinaMotos.Application.Validators.Pessoas
{
    public class CreatePessoaEnderecoValidator : AbstractValidator<CreatePessoaEnderecoDTO>
    {
        public CreatePessoaEnderecoValidator()
        {
            RuleFor(x => x.PessoaId).GreaterThan(0);
            RuleFor(x => x.Tipo).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Cep).MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Cep));
            RuleFor(x => x.Logradouro).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Numero).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Bairro).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Cidade).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Estado).NotEmpty().MaximumLength(60);
            RuleFor(x => x.Pais).NotEmpty().MaximumLength(60);
            RuleFor(x => x.Complemento).MaximumLength(120).When(x => !string.IsNullOrWhiteSpace(x.Complemento));
            RuleFor(x => x.Observacao).MaximumLength(255).When(x => !string.IsNullOrWhiteSpace(x.Observacao));
        }
    }
}
