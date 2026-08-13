using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;

namespace OficinaMotos.Application.Validators.OrdemServico
{
    public class UpdateOrdemServicoValidator : AbstractValidator<UpdateOrdemServicoDTO>
    {
        public UpdateOrdemServicoValidator()
        {
            RuleFor(x => x.ClienteId).GreaterThan(0);
            RuleFor(x => x.MecanicoId).GreaterThan(0);
            RuleFor(x => x.VeiculoId).GreaterThan(0);
            RuleFor(x => x.DescricaoProblema).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Status da ordem de serviço inválido.");
        }
    }
}
