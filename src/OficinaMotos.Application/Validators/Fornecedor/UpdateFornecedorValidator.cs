using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Fornecedor;

namespace OficinaMotos.Application.Validators.Fornecedor
{
    public class UpdateFornecedorValidator : AbstractValidator<UpdateFornecedorDTO>
    {
        public UpdateFornecedorValidator()
        {
            RuleFor(x => x.PessoaId).GreaterThan(0);
            RuleFor(x => x.Codigo).NotEmpty().MaximumLength(40);
            RuleFor(x => x.Tipo).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Status).NotEmpty().MaximumLength(30);
            RuleFor(x => x.CondicaoPagamentoPadrao).MaximumLength(120);
            RuleFor(x => x.PrazoGarantiaPadrao).MaximumLength(120);
            RuleFor(x => x.TermosNegociados).MaximumLength(240);
            RuleFor(x => x.Observacoes).MaximumLength(600);
        }
    }
}
