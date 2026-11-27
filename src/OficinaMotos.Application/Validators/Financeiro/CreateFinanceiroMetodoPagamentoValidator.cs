using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Financeiro;

namespace OficinaMotos.Application.Validators.Financeiro
{
    public class CreateFinanceiroMetodoPagamentoValidator : AbstractValidator<CreateFinanceiroMetodoPagamentoDTO>
    {
        public CreateFinanceiroMetodoPagamentoValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Tipo).NotEmpty().MaximumLength(40);
        }
    }
}
