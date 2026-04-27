export const genericFetcher = (url) =>
  fetch(url, { credentials: "include" }).then((r) => {
    if (!r.ok) throw new Error();
    return r.json();
  });