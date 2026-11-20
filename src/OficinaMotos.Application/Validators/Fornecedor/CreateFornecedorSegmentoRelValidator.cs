using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Fornecedor;

namespace OficinaMotos.Application.Validators.Fornecedor
{
    public class CreateFornecedorSegmentoRelValidator : AbstractValidator<CreateFornecedorSegmentoRelDTO>
    {
        public CreateFornecedorSegmentoRelValidator()
        {
            RuleFor(x => x.FornecedorId).GreaterThan(0);
            RuleFor(x => x.SegmentoId).GreaterThan(0);
        }
    }
}
