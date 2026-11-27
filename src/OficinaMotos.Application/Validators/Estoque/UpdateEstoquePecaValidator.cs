using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Estoque;

namespace OficinaMotos.Application.Validators.Estoque
{
    public class UpdateEstoquePecaValidator : AbstractValidator<UpdateEstoquePecaDTO>
    {
        public UpdateEstoquePecaValidator()
        {
            RuleFor(x => x.Codigo).NotEmpty().MaximumLength(60);
            RuleFor(x => x.CodigoBarras).MaximumLength(80);
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(180);
            RuleFor(x => x.Descricao).MaximumLength(500);
            RuleFor(x => x.Unidade).NotEmpty().MaximumLength(20);
        }
    }
}
