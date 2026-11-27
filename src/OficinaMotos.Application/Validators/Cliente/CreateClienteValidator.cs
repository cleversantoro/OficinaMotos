using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Cliente;

namespace OficinaMotos.Application.Validators.Cliente
{
    public class CreateClienteValidator : AbstractValidator<CreateClienteDTO>
    {
        public CreateClienteValidator()
        {
            RuleFor(x => x.PessoaId).GreaterThan(0);
            RuleFor(x => x.Codigo).GreaterThan(0);
            RuleFor(x => x.Observacoes).MaximumLength(500);
        }
    }
}
