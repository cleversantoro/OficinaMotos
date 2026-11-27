using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Estoque;

namespace OficinaMotos.Application.Validators.Estoque
{
    public class CreateEstoquePecaHistoricoValidator : AbstractValidator<CreateEstoquePecaHistoricoDTO>
    {
        public CreateEstoquePecaHistoricoValidator()
        {
            RuleFor(x => x.PecaId).GreaterThan(0);
            RuleFor(x => x.Campo).NotEmpty().MaximumLength(80);
            RuleFor(x => x.ValorAnterior).MaximumLength(500);
            RuleFor(x => x.ValorNovo).MaximumLength(500);
        }
    }
}
