export const PricingType = {
  Fixed: 0,
  FixedAndUnitBased: 1,
}

export function formatPricing(pricing) {
  if (!pricing) return null;
  switch (pricing.pricingType) {
    case PricingType.Fixed:
      return `${pricing.fixedPrice.toLocaleString("hu-HU")} Ft`;
    case PricingType.FixedAndUnitBased:
      if (pricing.fixedPrice > 0) {
        return `${pricing.fixedPrice.toLocaleString("hu-HU")} Ft + ${pricing.unitPrice?.toLocaleString("hu-HU")} Ft / ${pricing.unitName}`;
      }
      return `${pricing.unitPrice?.toLocaleString("hu-HU")} Ft / ${pricing.unitName}`;
    default:
      return null;
  }
}
