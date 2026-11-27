using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Financeiro;

namespace OficinaMotos.Application.Validators.Financeiro
{
    public class CreateFinanceiroHistoricoValidator : AbstractValidator<CreateFinanceiroHistoricoDTO>
    {
        public CreateFinanceiroHistoricoValidator()
        {
            RuleFor(x => x.Entidade).NotEmpty().MaximumLength(80);
            RuleFor(x => x.EntidadeId).GreaterThan(0);
            RuleFor(x => x.Acao).NotEmpty().MaximumLength(40);
        }
    }
}
