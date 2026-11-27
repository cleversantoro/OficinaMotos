using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Veiculo;

namespace OficinaMotos.Application.Validators.Veiculo
{
    public class UpdateVeiculoValidator : AbstractValidator<UpdateVeiculoDTO>
    {
        public UpdateVeiculoValidator()
        {
            RuleFor(x => x.ClienteId).GreaterThan(0);
            RuleFor(x => x.Placa).NotEmpty().MaximumLength(12);
            RuleFor(x => x.Cor).MaximumLength(50);
            RuleFor(x => x.Chassi).MaximumLength(80);
            RuleFor(x => x.Renavam).MaximumLength(20);
            RuleFor(x => x.Quilometragem).GreaterThan(0);
            RuleFor(x => x.Combustivel).MaximumLength(50);
            RuleFor(x => x.Observacoes).MaximumLength(240);
        }
    }
}
