import useSWR from "swr";

const fetcher = (url) =>
  fetch(url, { credentials: "include" }).then((r) => {
    if (!r.ok) throw new Error();
    return r.json();
  });

export default function useExpert() {
  const { data: expert, isLoading, error, mutate } = useSWR(
    "/api/Experts/me",
    fetcher
  );
  return { expert, isLoading, error, mutate };
}
