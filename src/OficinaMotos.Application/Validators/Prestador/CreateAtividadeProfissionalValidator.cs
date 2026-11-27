using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Prestador;

namespace OficinaMotos.Application.Validators.Prestador
{
    public class CreateAtividadeProfissionalValidator : AbstractValidator<CreateAtividadeProfissionalDTO>
    {
        public CreateAtividadeProfissionalValidator()
        {
            RuleFor(x => x.Codigo).NotEmpty().MaximumLength(40);
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Descricao).MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.Descricao));
        }
    }
}
