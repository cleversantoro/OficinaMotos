using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Estoque;

namespace OficinaMotos.Application.Validators.Estoque
{
    public class CreateEstoqueLocalizacaoValidator : AbstractValidator<CreateEstoqueLocalizacaoDTO>
    {
        public CreateEstoqueLocalizacaoValidator()
        {
            RuleFor(x => x.Codigo).NotEmpty().MaximumLength(40);
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Descricao).MaximumLength(240);
        }
    }
}
