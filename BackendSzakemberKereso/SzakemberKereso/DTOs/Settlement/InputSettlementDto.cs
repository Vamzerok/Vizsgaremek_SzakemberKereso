using FluentValidation;
using System.Data;

namespace SzakemberKereso.DTOs.Settlement
{
    public class InputSettlementDto
    {
        public int PostalCode { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class  InputSettlementDtoValidator : AbstractValidator<InputSettlementDto>
    {
        public InputSettlementDtoValidator()
        {
            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Az irányítószám meghatározása kötelező .")
                .GreaterThan(0).WithMessage("Azi irányítószám nem lehet negatív.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("A település neve kötelező.")
                .MaximumLength(100).WithMessage("A település neve legfeljebb 100 karakter lehet.");
        }
    }
}
