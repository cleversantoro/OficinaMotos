using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Financeiro;

namespace OficinaMotos.Application.Validators.Financeiro
{
    public class CreateContaPagarPagamentoValidator : AbstractValidator<CreateContaPagarPagamentoDTO>
    {
        public CreateContaPagarPagamentoValidator()
        {
            RuleFor(x => x.ContaPagarId).GreaterThan(0);
            RuleFor(x => x.ValorPago).GreaterThan(0);
            RuleFor(x => x.MetodoId).GreaterThan(0);
            RuleFor(x => x.DataPagamento).NotEmpty();
            RuleFor(x => x.Observacao).MaximumLength(240);
        }
    }
}
