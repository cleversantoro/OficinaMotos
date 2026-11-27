using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Fornecedor;

namespace OficinaMotos.Application.Validators.Fornecedor
{
    public class UpdateFornecedorRepresentanteValidator : AbstractValidator<UpdateFornecedorRepresentanteDTO>
    {
        public UpdateFornecedorRepresentanteValidator()
        {
            RuleFor(x => x.FornecedorId).GreaterThan(0);
            RuleFor(x => x.PessoaId).GreaterThan(0);
            RuleFor(x => x.Cargo).MaximumLength(120);
            RuleFor(x => x.Observacoes).MaximumLength(240);
        }
    }
}
