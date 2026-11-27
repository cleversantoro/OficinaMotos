using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Estoque;

namespace OficinaMotos.Application.Validators.Estoque
{
    public class UpdateEstoqueLocalizacaoValidator : AbstractValidator<UpdateEstoqueLocalizacaoDTO>
    {
        public UpdateEstoqueLocalizacaoValidator()
        {
            RuleFor(x => x.Codigo).NotEmpty().MaximumLength(40);
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Descricao).MaximumLength(240);
        }
    }
}
