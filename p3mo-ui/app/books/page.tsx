// Make sure this file exists at: app/books/page.tsx
// This is a Server Component
'use client';
import { BookList } from '@/components/books/BookList';

export default function BooksPage() {
  return <BookList />;
}