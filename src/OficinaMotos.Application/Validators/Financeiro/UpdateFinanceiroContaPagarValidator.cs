using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Financeiro;

namespace OficinaMotos.Application.Validators.Financeiro
{
    public class UpdateFinanceiroContaPagarValidator : AbstractValidator<UpdateFinanceiroContaPagarDTO>
    {
        public UpdateFinanceiroContaPagarValidator()
        {
            RuleFor(x => x.Descricao).NotEmpty().MaximumLength(240);
            RuleFor(x => x.Documento).MaximumLength(80);
            RuleFor(x => x.ValorOriginal).GreaterThan(0);
            RuleFor(x => x.Status).NotEmpty().MaximumLength(30);
            RuleFor(x => x.Observacoes).MaximumLength(500);
        }
    }
}
