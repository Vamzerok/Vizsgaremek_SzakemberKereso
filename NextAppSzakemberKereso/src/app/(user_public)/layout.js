import DefaultHeader from "@/components/general/DefaultHeader";

export default function UserPublicLayout({ children }) {
  return (
    <>
      <DefaultHeader />
      {children}
    </>
  );
}