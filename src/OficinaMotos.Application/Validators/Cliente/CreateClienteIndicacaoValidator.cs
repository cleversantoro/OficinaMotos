using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Cliente;

namespace OficinaMotos.Application.Validators.Cliente
{
    public class CreateClienteIndicacaoValidator : AbstractValidator<CreateClienteIndicacaoDTO>
    {
        public CreateClienteIndicacaoValidator()
        {
            RuleFor(x => x.ClienteId).GreaterThan(0);
            RuleFor(x => x.IndicadorNome).NotEmpty().MaximumLength(200);
            RuleFor(x => x.IndicadorTelefone).MaximumLength(80);
            RuleFor(x => x.Observacao).MaximumLength(240);
        }
    }
}
