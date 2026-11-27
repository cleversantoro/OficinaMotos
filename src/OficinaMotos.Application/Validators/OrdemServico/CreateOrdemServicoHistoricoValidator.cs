using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;

namespace OficinaMotos.Application.Validators.OrdemServico
{
    public class CreateOrdemServicoHistoricoValidator : AbstractValidator<CreateOrdemServicoHistoricoDTO>
    {
        public CreateOrdemServicoHistoricoValidator()
        {
            RuleFor(x => x.OrdemServicoId).GreaterThan(0);
            RuleFor(x => x.Campo).MaximumLength(120);
            RuleFor(x => x.ValorAntigo).MaximumLength(500);
            RuleFor(x => x.ValorNovo).MaximumLength(500);
        }
    }
}
