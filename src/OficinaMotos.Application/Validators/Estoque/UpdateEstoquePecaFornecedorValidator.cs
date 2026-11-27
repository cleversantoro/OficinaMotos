using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Estoque;

namespace OficinaMotos.Application.Validators.Estoque
{
    public class UpdateEstoquePecaFornecedorValidator : AbstractValidator<UpdateEstoquePecaFornecedorDTO>
    {
        public UpdateEstoquePecaFornecedorValidator()
        {
            RuleFor(x => x.PecaId).GreaterThan(0);
            RuleFor(x => x.FornecedorId).GreaterThan(0);
            RuleFor(x => x.CodigoFornecedor).MaximumLength(80);
        }
    }
}
