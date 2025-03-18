'use client';

import { useState, useEffect } from 'react';
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import { Book, getBookById, deleteBook, generateBookDetailPdf } from '@/lib/api/booksApi';
import { Button } from '@/components/ui/button';
import { 
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle
} from '@/components/ui/card';
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger
} from "@/components/ui/alert-dialog";
import { Loader2, ArrowLeft, Edit, Trash2, FileDown } from 'lucide-react';
import { toast } from 'sonner';

interface BookDetailProps {
  id: number;
}

export function BookDetail({ id }: BookDetailProps) {
  const [book, setBook] = useState<Book | null>(null);
  const [loading, setLoading] = useState(true);
  const [confirmDelete, setConfirmDelete] = useState(false);
  const router = useRouter();

  useEffect(() => {
    const loadBook = async () => {
      try {
        const data = await getBookById(id);
        setBook(data);
      } catch (error) {
        console.error('Error loading book:', error);
        toast.error('Failed to load book details');
      } finally {
        setLoading(false);
      }
    };

    loadBook();
  }, [id]);

  const handleDelete = async () => {
    try {
      await deleteBook(id);
      toast.success('Book deleted successfully');

      router.push('/books');
    } catch (error) {
      console.error('Error deleting book:', error);
      toast.error('Failed to delete book');
    }
  };

  const handlePrint = () => {
    try {
      generateBookDetailPdf(id);
    } catch (error) {
      console.error('Error generating PDF:', error);
      toast.error('Failed to generate PDF');
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <Loader2 className="h-8 w-8 animate-spin text-muted-foreground" />
      </div>
    );
  }

  if (!book) {
    return (
      <div className="text-center py-12">
        <h2 className="text-xl font-semibold">Book not found</h2>
        <p className="mt-2 text-muted-foreground">
          The book you are looking for does not exist or has been removed.
        </p>
        <Button asChild className="mt-4">
          <Link href="/books">
            <ArrowLeft className="mr-2 h-4 w-4" />
            Back to Books
          </Link>
        </Button>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <div>
          <Button asChild variant="outline" size="sm">
            <Link href="/books">
              <ArrowLeft className="mr-2 h-4 w-4" />
              Back to Books
            </Link>
          </Button>
        </div>
        <div className="flex flex-wrap gap-2">
          <Button onClick={handlePrint} variant="outline" size="sm">
            <FileDown className="mr-2 h-4 w-4" />
            Print
          </Button>
          <Button asChild variant="outline" size="sm">
            <Link href={`/books/edit/${id}`}>
              <Edit className="mr-2 h-4 w-4" />
              Edit
            </Link>
          </Button>
          <AlertDialog open={confirmDelete} onOpenChange={setConfirmDelete}>
            <AlertDialogTrigger asChild>
              <Button variant="destructive" size="sm">
                <Trash2 className="mr-2 h-4 w-4" />
                Delete
              </Button>
            </AlertDialogTrigger>
            <AlertDialogContent>
              <AlertDialogHeader>
                <AlertDialogTitle>Are you sure?</AlertDialogTitle>
                <AlertDialogDescription>
                  This will permanently delete "{book.title}" and cannot be undone.
                </AlertDialogDescription>
              </AlertDialogHeader>
              <AlertDialogFooter>
                <AlertDialogCancel>Cancel</AlertDialogCancel>
                <AlertDialogAction
                  onClick={handleDelete}
                  className="bg-red-600 hover:bg-red-700"
                >
                  Delete
                </AlertDialogAction>
              </AlertDialogFooter>
            </AlertDialogContent>
          </AlertDialog>
        </div>
      </div>

      <div className="md:grid md:grid-cols-4 gap-6">
        <Card className="md:col-span-3">
          <CardHeader>
            <CardTitle className="text-2xl">{book.title}</CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <h3 className="font-semibold text-sm text-muted-foreground">Publication Year</h3>
                <p>{book.publicationYear}</p>
              </div>
              <div>
                <h3 className="font-semibold text-sm text-muted-foreground">ISBN</h3>
                <p>{book.isbn || 'N/A'}</p>
              </div>
              <div>
                <h3 className="font-semibold text-sm text-muted-foreground">Author</h3>
                <p>{`${book.author.firstName} ${book.author.lastName}`}</p>
              </div>
              <div>
                <h3 className="font-semibold text-sm text-muted-foreground">Genre</h3>
                <p>{book.genre.name}</p>
              </div>
              <div>
                <h3 className="font-semibold text-sm text-muted-foreground">Page Count</h3>
                <p>{book.pageCount}</p>
              </div>
            </div>

            <div>
              <h3 className="font-semibold text-sm text-muted-foreground">Description</h3>
              <p className="mt-2 whitespace-pre-line">{book.description || 'No description available.'}</p>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Book Cover</CardTitle>
          </CardHeader>
          <CardContent className="flex justify-center">
            {book.coverImageUrl ? (
              <img 
                src={book.coverImageUrl} 
                alt={`Cover of ${book.title}`} 
                className="max-w-full h-auto rounded-md shadow-md"
              />
            ) : (
              <div className="w-full aspect-[2/3] bg-muted rounded-md flex items-center justify-center text-muted-foreground">
                No cover available
              </div>
            )}
          </CardContent>
          <CardFooter className="text-xs text-muted-foreground">
            Added on {new Date(book.createdAt).toLocaleDateString()}
          </CardFooter>
        </Card>
      </div>
    </div>
  );
}