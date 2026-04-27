/** @type {import('next').NextConfig} */
const nextConfig = {
  images: {
    remotePatterns: [
      new URL('https://placehold.co/**')
    ],
  },
  async rewrites() {
    return [
      {
        source: '/api/:path*',
        destination: `http://localhost:5272/api/:path*`,
      },
    ];
  },
};

export default nextConfig;
