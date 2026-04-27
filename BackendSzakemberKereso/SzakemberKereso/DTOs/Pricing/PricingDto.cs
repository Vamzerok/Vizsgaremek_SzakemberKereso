using FluentValidation;
using SzakemberKereso.Models;

namespace SzakemberKereso.DTOs.Pricing
{
    public class PricingDto
    {
        public PricingType PricingType { get; set; }
        public decimal FixedPrice { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? UnitName { get; set; }
    }

    public class PricingDtoValidator : AbstractValidator<PricingDto>
    {
        public PricingDtoValidator()
        {
            RuleFor(p => p.PricingType).IsInEnum();
            RuleFor(p => p.FixedPrice).GreaterThanOrEqualTo(0);
            When(p => p.PricingType == PricingType.Fixed, () =>
            {
                RuleFor(p => p.FixedPrice)
                    .NotNull()
                    .GreaterThan(0);
            });
            When(p => p.PricingType == PricingType.FixedAndUnitBased, () =>
            {
                RuleFor(p => p.UnitPrice)
                    .NotNull()
                    .GreaterThanOrEqualTo(0);
                RuleFor(p => p.UnitName)
                    .NotEmpty()
                    .MaximumLength(64);
            });
        }
    }
}
