using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Financeiro;

namespace OficinaMotos.Application.Validators.Financeiro
{
    public class CreateContaReceberPagamentoValidator : AbstractValidator<CreateContaReceberPagamentoDTO>
    {
        public CreateContaReceberPagamentoValidator()
        {
            RuleFor(x => x.ContaReceberId).GreaterThan(0);
            RuleFor(x => x.ValorPago).GreaterThan(0);
            RuleFor(x => x.MetodoId).GreaterThan(0);
            RuleFor(x => x.DataPagamento).NotEmpty();
            RuleFor(x => x.Observacao).MaximumLength(240);
        }
    }
}
