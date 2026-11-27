using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Prestador;

namespace OficinaMotos.Application.Validators.Prestador
{
    public class UpdatePrestadorValidator : AbstractValidator<UpdatePrestadorDTO>
    {
        public UpdatePrestadorValidator()
        {
            RuleFor(x => x.PessoaId).GreaterThan(0);
            RuleFor(x => x.Codigo).NotEmpty().MaximumLength(40);
            RuleFor(x => x.ValorHora).GreaterThanOrEqualTo(0).When(x => x.ValorHora.HasValue);
            RuleFor(x => x.Observacoes).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Observacoes));
        }
    }
}
