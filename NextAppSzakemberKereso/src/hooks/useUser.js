'use client';
import useSWR from 'swr';

const fetcher = async (url) => {
  const res = await fetch(url, { credentials: 'include' });
  if (!res.ok) {
    const error = new Error('Not authenticated.');
    error.status = res.status;
    throw error;
  }
  return res.json();
};

export default function useUser() {
  const { data: user, isLoading, isValidating, error, mutate } = useSWR(
    '/api/Users/me',
    fetcher
  );
  return { user, isLoading, isValidating, error, mutate };
}
