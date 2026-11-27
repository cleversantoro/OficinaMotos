using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;

namespace OficinaMotos.Application.Validators.OrdemServico
{
    public class UpdateOrdemServicoHistoricoValidator : AbstractValidator<UpdateOrdemServicoHistoricoDTO>
    {
        public UpdateOrdemServicoHistoricoValidator()
        {
            RuleFor(x => x.OrdemServicoId).GreaterThan(0);
            //RuleFor(x => x.Usuario).MaximumLength(160);
            RuleFor(x => x.Campo).MaximumLength(120);
            RuleFor(x => x.ValorAntigo).MaximumLength(300);
            RuleFor(x => x.ValorNovo).MaximumLength(300);
        }
    }
}
