using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;

namespace OficinaMotos.Application.Validators.OrdemServico
{
    public class UpdateOrdemServicoPagamentoValidator : AbstractValidator<UpdateOrdemServicoPagamentoDTO>
    {
        public UpdateOrdemServicoPagamentoValidator()
        {
            RuleFor(x => x.OrdemServicoId).GreaterThan(0);
            RuleFor(x => x.Status).IsInEnum();
            RuleFor(x => x.Metodo).MaximumLength(120);
            RuleFor(x => x.Observacao).MaximumLength(240);
        }
    }
}
