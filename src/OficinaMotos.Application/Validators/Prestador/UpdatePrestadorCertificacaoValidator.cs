using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Prestador;

namespace OficinaMotos.Application.Validators.Prestador
{
    public class UpdatePrestadorCertificacaoValidator : AbstractValidator<UpdatePrestadorCertificacaoDTO>
    {
        public UpdatePrestadorCertificacaoValidator()
        {
            RuleFor(x => x.PrestadorId).GreaterThan(0);
            RuleFor(x => x.Titulo).NotEmpty().MaximumLength(160);
            RuleFor(x => x.Instituicao).MaximumLength(160).When(x => !string.IsNullOrWhiteSpace(x.Instituicao));
            RuleFor(x => x.CodigoCertificacao).MaximumLength(60).When(x => !string.IsNullOrWhiteSpace(x.CodigoCertificacao));
            RuleFor(x => x.ArquivoUrl).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.ArquivoUrl));
        }
    }
}
