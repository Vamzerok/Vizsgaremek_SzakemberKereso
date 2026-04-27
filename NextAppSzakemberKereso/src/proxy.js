import { NextResponse } from 'next/server';

export function proxy(request) {
  const isLoggedIn = request.cookies.has('.AspNetCore.Identity.Application');
  if (!isLoggedIn) {
    const from = encodeURIComponent(request.nextUrl.pathname);
    return NextResponse.redirect(new URL(`/auth/login?from=${from}`, request.url));
  }
  return NextResponse.next();
}

export const config = { matcher: '/dashboard/:path*' };
