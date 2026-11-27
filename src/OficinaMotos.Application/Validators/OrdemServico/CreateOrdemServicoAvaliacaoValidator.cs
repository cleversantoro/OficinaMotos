using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;

namespace OficinaMotos.Application.Validators.OrdemServico
{
    public class CreateOrdemServicoAvaliacaoValidator : AbstractValidator<CreateOrdemServicoAvaliacaoDTO>
    {
        public CreateOrdemServicoAvaliacaoValidator()
        {
            RuleFor(x => x.OrdemServicoId).GreaterThan(0);
            RuleFor(x => x.ClienteId).GreaterThan(0);
            RuleFor(x => x.Nota).InclusiveBetween((byte)1, (byte)5);
            RuleFor(x => x.Comentario).MaximumLength(1000);
        }
    }
}
