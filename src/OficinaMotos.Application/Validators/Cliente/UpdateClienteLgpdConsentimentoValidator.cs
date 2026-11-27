using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Cliente;

namespace OficinaMotos.Application.Validators.Cliente
{
    public class UpdateClienteLgpdConsentimentoValidator : AbstractValidator<UpdateClienteLgpdConsentimentoDTO>
    {
        public UpdateClienteLgpdConsentimentoValidator()
        {
            RuleFor(x => x.ClienteId).GreaterThan(0);
            RuleFor(x => x.Tipo).GreaterThan(0);
            RuleFor(x => x.Canal).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Observacoes).MaximumLength(240);
        }
    }
}
