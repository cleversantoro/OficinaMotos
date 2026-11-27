using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Veiculo;

namespace OficinaMotos.Application.Validators.Veiculo
{
    public class CreateVeiculoMarcaValidator : AbstractValidator<CreateVeiculoMarcaDTO>
    {
        public CreateVeiculoMarcaValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        }
    }
}
