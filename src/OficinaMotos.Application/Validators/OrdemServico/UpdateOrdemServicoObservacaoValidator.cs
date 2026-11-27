using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;

namespace OficinaMotos.Application.Validators.OrdemServico
{
    public class UpdateOrdemServicoObservacaoValidator : AbstractValidator<UpdateOrdemServicoObservacaoDTO>
    {
        public UpdateOrdemServicoObservacaoValidator()
        {
            RuleFor(x => x.OrdemServicoId).GreaterThan(0);
            //RuleFor(x => x.Usuario).MaximumLength(160);
            RuleFor(x => x.Texto).NotEmpty().MaximumLength(500);
        }
    }
}
