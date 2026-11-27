using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Estoque;

namespace OficinaMotos.Application.Validators.Estoque
{
    public class UpdateEstoqueFabricanteValidator : AbstractValidator<UpdateEstoqueFabricanteDTO>
    {
        public UpdateEstoqueFabricanteValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("Nome é obrigatório").MaximumLength(160);
            RuleFor(x => x.Documento).MaximumLength(30);
            RuleFor(x => x.Website).MaximumLength(200);
        }
    }
}
