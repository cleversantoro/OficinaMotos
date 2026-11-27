using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Prestador;

namespace OficinaMotos.Application.Validators.Prestador
{
    public class CreatePrestadorExperienciaValidator : AbstractValidator<CreatePrestadorExperienciaDTO>
    {
        public CreatePrestadorExperienciaValidator()
        {
            RuleFor(x => x.PrestadorId).GreaterThan(0);
            RuleFor(x => x.Empresa).NotEmpty().MaximumLength(160);
            RuleFor(x => x.Cargo).MaximumLength(120).When(x => !string.IsNullOrWhiteSpace(x.Cargo));
            RuleFor(x => x.Descricao).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Descricao));
        }
    }
}
