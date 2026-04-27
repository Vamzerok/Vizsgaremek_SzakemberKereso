using FluentValidation;
using SzakemberKereso.DTOs.Pricing;

namespace SzakemberKereso.DTOs.Service
{
    public class InputServiceDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public PricingDto Pricing { get; set; } = null!;
    }

    public class InputServiceDtoValidator : AbstractValidator<InputServiceDto>
    {
        public InputServiceDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("A szolgáltatásnak kötelező nevet adni.")
                .MaximumLength(100).WithMessage("A szolgáltatás neve legfeljebb 100 karakter lehet.");

            RuleFor(x => x.Pricing)
                .NotNull().WithMessage("Az árazás megadása kötelező.")
                .SetValidator(new PricingDtoValidator());  //nested validation go brrr
        }
    }
}
