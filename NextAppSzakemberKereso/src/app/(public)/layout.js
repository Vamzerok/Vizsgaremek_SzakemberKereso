import DefaultHeader from '@/components/general/DefaultHeader';

export default function PublicLayout({ children }) {
  return (
    <>
      <DefaultHeader />
      {children}
    </>
  );
}
