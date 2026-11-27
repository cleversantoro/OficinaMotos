using FluentValidation;
using OficinaMotos.Application.DTOs.Requests.Prestador;

namespace OficinaMotos.Application.Validators.Prestador
{
    public class UpdatePrestadorDisponibilidadeValidator : AbstractValidator<UpdatePrestadorDisponibilidadeDTO>
    {
        public UpdatePrestadorDisponibilidadeValidator()
        {
            RuleFor(x => x.PrestadorId).GreaterThan(0);
            RuleFor(x => (int)x.DiaSemana).InclusiveBetween(0, 7);
            RuleFor(x => x.HoraFim).GreaterThan(x => x.HoraInicio);
        }
    }
}
