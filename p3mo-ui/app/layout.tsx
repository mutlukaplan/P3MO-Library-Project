import './globals.css';
import type { Metadata } from 'next';
import { Inter } from 'next/font/google';
import Link from 'next/link';
import { Toaster } from 'sonner';
import { cn } from '@/lib/utils';

const inter = Inter({ subsets: ['latin'] });

export const metadata: Metadata = {
  title: 'Library Management System',
  description: 'Modern web application for managing book libraries',
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en" suppressHydrationWarning>
      <body className={cn('min-h-screen bg-background', inter.className)}>
        <div className="flex flex-col min-h-screen">
          <header className="border-b">
            <div className="container flex h-16 items-center">
              <Link href="/" className="font-bold text-xl">
                Library Management
              </Link>
              <nav className="flex items-center ml-auto space-x-4">
                <Link
                  href="/books"
                  className="text-sm font-medium hover:underline"
                >
                  Books
                </Link>
              </nav>
            </div>
          </header>
          <main className="flex-1">
            <div className="container py-6">{children}</div>
          </main>
          <footer className="border-t py-4 text-center text-sm text-muted-foreground">
            <div className="container">
              Library Management System | {new Date().getFullYear()}
            </div>
          </footer>
        </div>
        <Toaster richColors position="top-right" />
      </body>
    </html>
  );
}