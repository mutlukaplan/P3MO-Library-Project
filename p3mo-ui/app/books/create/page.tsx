'use client';

import { BookForm } from '@/components/books/BookForm';

export default function CreateBookPage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold mb-2">Add New Book</h1>
        <p className="text-muted-foreground">
          Add a new book to your library collection
        </p>
      </div>
      
      <BookForm />
    </div>
  );
}