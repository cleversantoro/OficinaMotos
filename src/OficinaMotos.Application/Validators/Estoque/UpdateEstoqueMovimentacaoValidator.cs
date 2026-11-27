using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Estoque;

namespace OficinaMotos.Application.Validators.Estoque
{
    public class UpdateEstoqueMovimentacaoValidator : AbstractValidator<UpdateEstoqueMovimentacaoDTO>
    {
        public UpdateEstoqueMovimentacaoValidator()
        {
            RuleFor(x => x.PecaId).GreaterThan(0);
            RuleFor(x => x.Tipo).NotEmpty().MaximumLength(30);
            RuleFor(x => x.OrigemTipo).MaximumLength(40);
            RuleFor(x => x.Observacao).MaximumLength(300);
        }
    }
}
