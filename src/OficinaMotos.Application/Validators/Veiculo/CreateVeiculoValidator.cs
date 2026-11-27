using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Veiculo;

namespace OficinaMotos.Application.Validators.Veiculo
{
    public class CreateVeiculoValidator : AbstractValidator<CreateVeiculoDTO>
    {
        public CreateVeiculoValidator()
        {
            RuleFor(x => x.ClienteId).GreaterThan(0);
            RuleFor(x => x.Codigo).MaximumLength(40);
            RuleFor(x => x.Placa).MaximumLength(10);
            RuleFor(x => x.Cor).MaximumLength(40);
            RuleFor(x => x.Chassi).MaximumLength(40);
            RuleFor(x => x.Renavam).MaximumLength(20);
            RuleFor(x => x.Combustivel).MaximumLength(30);
            RuleFor(x => x.Observacoes).MaximumLength(500);
        }
    }
}
