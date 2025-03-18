'use client';

import { useState, useEffect } from 'react';
import Link from 'next/link';
import { 
  Book,
  getBooks, 
  getBooksByGenre, 
  deleteBook, 
  generateBookListPdf
} from '@/lib/api/booksApi';
import { BookChart } from './BookChart';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { 
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle
} from '@/components/ui/card';
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
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
import { Loader2, MoreHorizontal, FileDown, Plus, Search, RefreshCw, Edit, Trash2 } from 'lucide-react';
import { toast } from 'sonner';

export function BookList() {
  const [books, setBooks] = useState<Book[]>([]);
  const [filteredBooks, setFilteredBooks] = useState<Book[]>([]);
  const [genreData, setGenreData] = useState<any[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [loading, setLoading] = useState(true);
  const [bookToDelete, setBookToDelete] = useState<Book | null>(null);

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    if (books.length > 0) {
      setFilteredBooks(
        books.filter(book => 
          book.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
          book.author.firstName.toLowerCase().includes(searchTerm.toLowerCase()) ||
          book.author.lastName.toLowerCase().includes(searchTerm.toLowerCase()) ||
          book.genre.name.toLowerCase().includes(searchTerm.toLowerCase())
        )
      );
    }
  }, [searchTerm, books]);

  const loadData = async () => {
    setLoading(true);
    try {
      const [booksData, genreData] = await Promise.all([
        getBooks(),
        getBooksByGenre()
      ]);
      setBooks(booksData);
      setFilteredBooks(booksData);
      setGenreData(genreData);
    } catch (error) {
      console.error('Error loading data:', error);
      toast.error('Failed to load books data');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteBook(id);
      setBooks(books.filter(book => book.id !== id));
      toast.success('Book deleted successfully');
    } catch (error) {
      console.error('Error deleting book:', error);
      toast.error('Failed to delete book');
    }
  };

  const handlePrint = () => {
    try {
      generateBookListPdf();
    } catch (error) {
      console.error('Error generating PDF:', error);
      toast.error('Failed to generate PDF');
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
        <div>
          <h1 className="text-2xl font-bold">Library Books</h1>
          <p className="text-muted-foreground">Manage your book collection</p>
        </div>
        <div className="flex flex-col sm:flex-row gap-2">
          <Button asChild>
            <Link href="/books/create">
              <Plus className="mr-2 h-4 w-4" />
              Add Book
            </Link>
          </Button>
          <Button onClick={handlePrint} variant="outline">
            <FileDown className="mr-2 h-4 w-4" />
            Print List
          </Button>
          <Button onClick={loadData} variant="outline" size="icon">
            <RefreshCw className="h-4 w-4" />
          </Button>
        </div>
      </div>
      
      <div className="md:grid md:grid-cols-3 gap-6">
        <Card className="md:col-span-2">
          <CardHeader>
            <CardTitle>All Books</CardTitle>
            <CardDescription>
              <div className="flex items-center gap-2">
                <Search className="h-4 w-4 text-muted-foreground" />
                <Input
                  placeholder="Search books..."
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="max-w-sm"
                />
              </div>
            </CardDescription>
          </CardHeader>
          <CardContent>
            {loading ? (
              <div className="flex justify-center items-center h-64">
                <Loader2 className="h-8 w-8 animate-spin text-muted-foreground" />
              </div>
            ) : (
              <div className="rounded-md border">
                <Table>
                  <TableHeader>
                    <TableRow>
                      <TableHead>Title</TableHead>
                      <TableHead>Author</TableHead>
                      <TableHead>Genre</TableHead>
                      <TableHead>Year</TableHead>
                      <TableHead className="w-[100px]">Actions</TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {filteredBooks.length === 0 ? (
                      <TableRow>
                        <TableCell colSpan={5} className="text-center h-24">
                          No books found
                        </TableCell>
                      </TableRow>
                    ) : (
                      filteredBooks.map((book) => (
                        <TableRow key={book.id}>
                          <TableCell>
                            <Link 
                              href={`/books/${book.id}`}
                              className="font-medium text-blue-600 hover:underline"
                            >
                              {book.title}
                            </Link>
                          </TableCell>
                          <TableCell>{`${book.author.firstName} ${book.author.lastName}`}</TableCell>
                          <TableCell>{book.genre.name}</TableCell>
                          <TableCell>{book.publicationYear}</TableCell>
                          <TableCell>
                            <DropdownMenu>
                              <DropdownMenuTrigger asChild>
                                <Button variant="ghost" size="icon">
                                  <MoreHorizontal className="h-4 w-4" />
                                </Button>
                              </DropdownMenuTrigger>
                              <DropdownMenuContent align="end">
                                <DropdownMenuItem asChild>
                                  <Link href={`/books/${book.id}`}>
                                    View Details
                                  </Link>
                                </DropdownMenuItem>
                                <DropdownMenuItem asChild>
                                  <Link href={`/books/edit/${book.id}`}>
                                    <Edit className="mr-2 h-4 w-4" />
                                    Edit
                                  </Link>
                                </DropdownMenuItem>
                                <DropdownMenuItem 
                                  onClick={() => setBookToDelete(book)}
                                  className="text-red-600"
                                >
                                  <Trash2 className="mr-2 h-4 w-4" />
                                  Delete
                                </DropdownMenuItem>
                              </DropdownMenuContent>
                            </DropdownMenu>
                          </TableCell>
                        </TableRow>
                      ))
                    )}
                  </TableBody>
                </Table>
              </div>
            )}
          </CardContent>
          <CardFooter>
            <p className="text-sm text-muted-foreground">
              Showing {filteredBooks.length} of {books.length} books
            </p>
          </CardFooter>
        </Card>
        
        <Card>
          <CardHeader>
            <CardTitle>Book Statistics</CardTitle>
            <CardDescription>Distribution by genre</CardDescription>
          </CardHeader>
          <CardContent>
            {loading ? (
              <div className="flex justify-center items-center h-64">
                <Loader2 className="h-8 w-8 animate-spin text-muted-foreground" />
              </div>
            ) : (
              <BookChart data={genreData} />
            )}
          </CardContent>
        </Card>
      </div>

      <AlertDialog open={!!bookToDelete} onOpenChange={(open) => !open && setBookToDelete(null)}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Are you sure?</AlertDialogTitle>
            <AlertDialogDescription>
              This will permanently delete "{bookToDelete?.title}" and cannot be undone.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction
              onClick={() => {
                if (bookToDelete) {
                  handleDelete(bookToDelete.id);
                  setBookToDelete(null);
                }
              }}
              className="bg-red-600 hover:bg-red-700"
            >
              Delete
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  );
}