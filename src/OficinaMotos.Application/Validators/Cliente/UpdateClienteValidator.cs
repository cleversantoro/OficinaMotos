using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Cliente;

namespace OficinaMotos.Application.Validators.Cliente
{
    public class UpdateClienteValidator : AbstractValidator<UpdateClienteDTO>
    {
        public UpdateClienteValidator()
        {
            RuleFor(x => x.PessoaId).GreaterThan(0);
            RuleFor(x => x.Codigo).GreaterThan(0);
            RuleFor(x => x.Observacoes).MaximumLength(500);
        }
    }
}
