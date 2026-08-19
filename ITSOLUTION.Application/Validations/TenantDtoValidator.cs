using FluentValidation;
using ITSOLUTION.Application.DTOs;

namespace ITSOLUTION.Application.Validations
{
    public class TenantDtoValidator : AbstractValidator<TenantDto>
    {
        public TenantDtoValidator()
        {
            RuleFor(x => x.NombreComercial)
                .NotEmpty().WithMessage("El Nombre Comercial es obligatorio.")
                .MinimumLength(3).WithMessage("El nombre debe tener al menos 3 caracteres.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

            // CORREGIDO: Ruc_NIT en mayúsculas
            RuleFor(x => x.RUC_NIT)
                .NotEmpty().WithMessage("El RUC/NIT es obligatorio.")
                .Length(10, 13).WithMessage("El RUC/NIT debe tener entre 10 y 13 caracteres numéricos.");
        }
    }
}