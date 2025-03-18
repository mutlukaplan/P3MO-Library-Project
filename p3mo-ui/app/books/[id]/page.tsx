'use client';

import { BookDetail } from '@/components/books/BookDetail';

interface BookDetailPageProps {
  params: {
    id: string;
  };
}

export default function BookDetailPage({ params }: BookDetailPageProps) {
  return <BookDetail id={parseInt(params.id)} />;
}