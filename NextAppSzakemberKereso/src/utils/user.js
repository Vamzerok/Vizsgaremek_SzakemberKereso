export function isUserAnExpert(user) {
  return !!user?.roles?.includes("Expert");
}
