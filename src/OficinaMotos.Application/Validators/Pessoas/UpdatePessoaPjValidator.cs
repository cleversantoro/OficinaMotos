using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Pessoas;

namespace OficinaMotos.Application.Validators.Pessoas
{
    public class UpdatePessoaPjValidator : AbstractValidator<UpdatePessoaPjDTO>
    {
        public UpdatePessoaPjValidator()
        {
            RuleFor(x => x.PessoaId).GreaterThan(0);
            RuleFor(x => x.RazaoSocial).NotEmpty().MaximumLength(200);
            RuleFor(x => x.NomeFantasia).MaximumLength(180).When(x => !string.IsNullOrWhiteSpace(x.NomeFantasia));
            RuleFor(x => x.InscricaoEstadual).MaximumLength(40).When(x => !string.IsNullOrWhiteSpace(x.InscricaoEstadual));
            RuleFor(x => x.InscricaoMunicipal).MaximumLength(40).When(x => !string.IsNullOrWhiteSpace(x.InscricaoMunicipal));
            RuleFor(x => x.Responsavel).MaximumLength(150).When(x => !string.IsNullOrWhiteSpace(x.Responsavel));
            RuleFor(x => x.Website).MaximumLength(255).When(x => !string.IsNullOrWhiteSpace(x.Website));
        }
    }
}
