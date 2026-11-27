using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Pessoas;

namespace OficinaMotos.Application.Validators.Pessoas
{
    public class UpdatePessoaAnexoValidator : AbstractValidator<UpdatePessoaAnexoDTO>
    {
        public UpdatePessoaAnexoValidator()
        {
            RuleFor(x => x.PessoaId).GreaterThan(0);
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Tipo).NotEmpty().MaximumLength(60);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Observacao).MaximumLength(255).When(x => !string.IsNullOrWhiteSpace(x.Observacao));
            RuleFor(x => x.MimeType).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.MimeType));
            RuleFor(x => x.TamanhoBytes).GreaterThanOrEqualTo(0).When(x => x.TamanhoBytes.HasValue);
        }
    }
}
