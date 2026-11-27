using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Prestador;

namespace OficinaMotos.Application.Validators.Prestador
{
    public class UpdatePrestadorAtividadeValidator : AbstractValidator<UpdatePrestadorAtividadeDTO>
    {
        public UpdatePrestadorAtividadeValidator()
        {
            RuleFor(x => x.PrestadorId).GreaterThan(0);
            RuleFor(x => x.AtividadeProfissionalId).GreaterThan(0);
            RuleFor(x => x.Nivel).MaximumLength(40).When(x => !string.IsNullOrWhiteSpace(x.Nivel));
        }
    }
}
