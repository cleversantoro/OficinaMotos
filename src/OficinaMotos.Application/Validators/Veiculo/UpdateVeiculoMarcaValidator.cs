using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Veiculo;

namespace OficinaMotos.Application.Validators.Veiculo
{
    public class UpdateVeiculoMarcaValidator : AbstractValidator<UpdateVeiculoMarcaDTO>
    {
        public UpdateVeiculoMarcaValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(160);
            //RuleFor(x => x.Pais).MaximumLength(120);
        }
    }
}
