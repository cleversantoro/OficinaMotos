using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Prestador;

namespace OficinaMotos.Application.Validators.Prestador
{
    public class UpdateAtividadeProfissionalValidator : AbstractValidator<UpdateAtividadeProfissionalDTO>
    {
        public UpdateAtividadeProfissionalValidator()
        {
            RuleFor(x => x.Codigo).NotEmpty().MaximumLength(40);
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Descricao).MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.Descricao));
        }
    }
}
