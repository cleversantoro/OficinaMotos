using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;

namespace OficinaMotos.Application.Validators.OrdemServico
{
    public class CreateOrdemServicoObservacaoValidator : AbstractValidator<CreateOrdemServicoObservacaoDTO>
    {
        public CreateOrdemServicoObservacaoValidator()
        {
            RuleFor(x => x.OrdemServicoId).GreaterThan(0);
            RuleFor(x => x.Texto).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Tipo).NotEmpty().MaximumLength(30);
        }
    }
}
