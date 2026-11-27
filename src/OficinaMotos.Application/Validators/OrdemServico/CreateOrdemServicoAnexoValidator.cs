using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;

namespace OficinaMotos.Application.Validators.OrdemServico
{
    public class CreateOrdemServicoAnexoValidator : AbstractValidator<CreateOrdemServicoAnexoDTO>
    {
        public CreateOrdemServicoAnexoValidator()
        {
            RuleFor(x => x.OrdemServicoId).GreaterThan(0);
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Observacao).MaximumLength(240);
        }
    }
}
