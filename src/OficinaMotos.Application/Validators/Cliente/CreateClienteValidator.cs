using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Cliente;
using System.Linq;

namespace OficinaMotos.Application.Validators.Cliente
{
    public class CreateClienteValidator : AbstractValidator<CreateClienteDTO>
    {
        public CreateClienteValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .Length(3, 150).WithMessage("O nome deve ter entre 3 e 150 caracteres");

            RuleFor(x => x.Tipo)
                .Must(t => t == 1 || t == 2)
                .WithMessage("O tipo de cliente deve ser 1 (Pessoa Física) ou 2 (Pessoa Jurídica)");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email inválido");

            RuleFor(x => x)
                .Must(ValidarDocumentoPorTipo)
                .WithMessage(x => x.Tipo == 2 ? "CNPJ inválido" : "CPF inválido");
        }

        private bool ValidarDocumentoPorTipo(CreateClienteDTO dto)
        {
            var raw = !string.IsNullOrWhiteSpace(dto.Documento) ? dto.Documento : dto.Cpf;
            if (string.IsNullOrWhiteSpace(raw)) return false;

            var digits = new string(raw.Where(char.IsDigit).ToArray());
            return dto.Tipo == 2 ? digits.Length == 14 : digits.Length == 11;
        }
    }
}