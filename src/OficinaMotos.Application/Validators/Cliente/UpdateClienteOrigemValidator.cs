using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Cliente;

namespace OficinaMotos.Application.Validators.Cliente
{
    public class UpdateClienteOrigemValidator : AbstractValidator<UpdateClienteOrigemDTO>
    {
        public UpdateClienteOrigemValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Descricao).MaximumLength(240);
        }
    }
}
