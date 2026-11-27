using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Financeiro;

namespace OficinaMotos.Application.Validators.Financeiro
{
    public class CreateFinanceiroLancamentoValidator : AbstractValidator<CreateFinanceiroLancamentoDTO>
    {
        public CreateFinanceiroLancamentoValidator()
        {
            RuleFor(x => x.Tipo).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Categoria).MaximumLength(100);
            RuleFor(x => x.Descricao).NotEmpty().MaximumLength(240);
            RuleFor(x => x.OrigemTipo).MaximumLength(40);
            RuleFor(x => x.Observacoes).MaximumLength(500);
        }
    }
}
