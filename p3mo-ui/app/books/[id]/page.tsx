'use client';
import { BookDetail } from '@/components/books/BookDetail';

export default function BookDetailPage({ params }) {
  return <BookDetail id={parseInt(params.id)} />;
}