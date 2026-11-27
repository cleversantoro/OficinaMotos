using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Pessoas;

namespace OficinaMotos.Application.Validators.Pessoas
{
    public class UpdatePessoaDocumentoValidator : AbstractValidator<UpdatePessoaDocumentoDTO>
    {
        public UpdatePessoaDocumentoValidator()
        {
            RuleFor(x => x.PessoaId).GreaterThan(0);
            RuleFor(x => x.Tipo).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Numero).NotEmpty().MaximumLength(80);
            RuleFor(x => x.OrgaoExpedidor).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.OrgaoExpedidor));
            RuleFor(x => x.ArquivoUrl).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.ArquivoUrl));
            RuleFor(x => x.Observacoes).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Observacoes));
        }
    }
}
