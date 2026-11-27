using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Cliente;

namespace OficinaMotos.Application.Validators.Cliente
{
    public class UpdateClienteFinanceiroValidator : AbstractValidator<UpdateClienteFinanceiroDTO>
    {
        public UpdateClienteFinanceiroValidator()
        {
            RuleFor(x => x.ClienteId).GreaterThan(0);
            RuleFor(x => x.LimiteCredito).GreaterThanOrEqualTo(0).When(x => x.LimiteCredito.HasValue);
            RuleFor(x => x.PrazoPagamento).GreaterThan(0).When(x => x.PrazoPagamento.HasValue);
            RuleFor(x => x.Observacoes).MaximumLength(300);
        }
    }
}
