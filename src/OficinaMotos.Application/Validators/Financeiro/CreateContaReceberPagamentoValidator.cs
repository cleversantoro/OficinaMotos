using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Financeiro;

namespace OficinaMotos.Application.Validators.Financeiro
{
    public class CreateContaReceberPagamentoValidator : AbstractValidator<CreateContaReceberPagamentoDTO>
    {
        public CreateContaReceberPagamentoValidator()
        {
            RuleFor(x => x.ContaReceberId).GreaterThan(0);
            RuleFor(x => x.Valor).GreaterThan(0);
            RuleFor(x => x.Tipo).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Status).NotEmpty().MaximumLength(30);
            RuleFor(x => x.ReferenciaExterna).MaximumLength(120);
            RuleFor(x => x.Observacao).MaximumLength(300);
        }
    }
}
