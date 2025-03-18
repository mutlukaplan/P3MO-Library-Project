'use client';

import { BookForm } from '@/components/books/BookForm';

interface EditBookPageProps {
  params: {
    id: string;
  };
}

export default function EditBookPage({ params }: EditBookPageProps) {
  const id = parseInt(params.id);
  
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold mb-2">Edit Book</h1>
        <p className="text-muted-foreground">
          Update the information for this book
        </p>
      </div>
      
      <BookForm id={id} />
    </div>
  );
}