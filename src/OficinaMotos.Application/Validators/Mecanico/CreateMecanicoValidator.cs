using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Mecanico;

namespace OficinaMotos.Application.Validators.Mecanico
{
    public class CreateMecanicoValidator : AbstractValidator<CreateMecanicoDTO>
    {
        public CreateMecanicoValidator()
        {
            RuleFor(x => x.PrestadorId).GreaterThan(0);
            RuleFor(x => x.Matricula).MaximumLength(40);
            RuleFor(x => x.Nivel).MaximumLength(40);
            RuleFor(x => x.Observacoes).MaximumLength(500);
        }
    }
}
