/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  images: {
    domains: ['covers.openlibrary.org', 'images.unsplash.com'],
  },
};

// Make sure to use module.exports here
module.exports = nextConfig;