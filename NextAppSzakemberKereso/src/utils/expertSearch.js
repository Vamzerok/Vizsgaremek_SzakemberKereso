//parses url query params into filterParams object.
export function parseFilterParams(searchParams) {
  return {
    //kinda realizing that occupationsId should be a string...
    //on the other hand, built in sanitization :D
    occupationId: searchParams.get('occupationId') ? Number(searchParams.get('occupationId')) : null,
    minFixedPrice: searchParams.get('minFixedPrice') ? Number(searchParams.get('minFixedPrice')) : null,
    maxFixedPrice: searchParams.get('maxFixedPrice') ? Number(searchParams.get('maxFixedPrice')) : null,
    allowUnitBased: searchParams.get('allowUnitBased') !== 'false', // default true
    unitName: searchParams.get('unitName') ?? null,
    countyName: searchParams.get('countyName') ?? null,
    settlementName: searchParams.get('settlementName') ?? null,
  };
}

//converts filterParams into URLSearchParams that can be used for routing and api calls
export function buildFilterQuery(filterParams) {
  const params = new URLSearchParams();
  if (filterParams.occupationId) params.set('occupationId', filterParams.occupationId);
  if (filterParams.minFixedPrice != null) params.set('minFixedPrice', filterParams.minFixedPrice);
  if (filterParams.maxFixedPrice != null) params.set('maxFixedPrice', filterParams.maxFixedPrice);
  if (!filterParams.allowUnitBased) params.set('allowUnitBased', 'false');
  if (filterParams.unitName) params.set('unitName', filterParams.unitName);
  if (filterParams.countyName) params.set('countyName', filterParams.countyName);
  if (filterParams.settlementName) params.set('settlementName', filterParams.settlementName);
  return params.toString();
}
