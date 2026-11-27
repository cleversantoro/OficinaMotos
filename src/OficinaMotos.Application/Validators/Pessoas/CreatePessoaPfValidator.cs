using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Pessoas;

namespace OficinaMotos.Application.Validators.Pessoas
{
    public class CreatePessoaPfValidator : AbstractValidator<CreatePessoaPfDTO>
    {
        public CreatePessoaPfValidator()
        {
            RuleFor(x => x.PessoaId).GreaterThan(0);
            RuleFor(x => x.Rg).MaximumLength(30).When(x => !string.IsNullOrWhiteSpace(x.Rg));
            RuleFor(x => x.Genero).MaximumLength(30).When(x => !string.IsNullOrWhiteSpace(x.Genero));
            RuleFor(x => x.EstadoCivil).MaximumLength(30).When(x => !string.IsNullOrWhiteSpace(x.EstadoCivil));
            RuleFor(x => x.Profissao).MaximumLength(120).When(x => !string.IsNullOrWhiteSpace(x.Profissao));
        }
    }
}
