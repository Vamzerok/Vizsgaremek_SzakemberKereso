export function formatLocation(location) {
  if (!location) return '-';
  return `${location.settlement?.postalCode} 
    ${location.settlement?.name}, 
    ${location.streetName} 
    ${location.publicAreaType ?? "u."} 
    ${location.buildingNumber}.`;
}
