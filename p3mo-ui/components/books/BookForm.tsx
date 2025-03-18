'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { 
  Book, 
  Author, 
  Genre, 
  getBookById, 
  getAuthors, 
  getGenres, 
  createBook, 
  updateBook 
} from '@/lib/api/booksApi';
import { bookSchema, BookFormValues } from '@/lib/validations/book';
import { Button } from '@/components/ui/button';
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Loader2 } from 'lucide-react';
import { toast } from 'sonner';

interface BookFormProps {
  id?: number;
}

export function BookForm({ id }: BookFormProps) {
  const [loading, setLoading] = useState(false);
  const [initializing, setInitializing] = useState(id ? true : false);
  const [authors, setAuthors] = useState<Author[]>([]);
  const [genres, setGenres] = useState<Genre[]>([]);
  const router = useRouter();
  
  const form = useForm<BookFormValues>({
    resolver: zodResolver(bookSchema),
    defaultValues: {
      title: '',
      publicationYear: new Date().getFullYear(),
      isbn: '',
      coverImageUrl: '',
      description: '',
      pageCount: 1,
      authorId: 0,
      genreId: 0,
    },
    mode: 'onChange',
  });

  useEffect(() => {
    const loadFormData = async () => {
      try {
        // Load authors and genres
        const [authorsData, genresData] = await Promise.all([
          getAuthors(),
          getGenres()
        ]);
        
        setAuthors(authorsData);
        setGenres(genresData);
        
        // If editing, load book data
        if (id) {
          const bookData = await getBookById(id);
          
          form.reset({
            title: bookData.title,
            publicationYear: bookData.publicationYear,
            isbn: bookData.isbn || '',
            coverImageUrl: bookData.coverImageUrl || '',
            description: bookData.description || '',
            pageCount: bookData.pageCount,
            authorId: bookData.authorId,
            genreId: bookData.genreId,
          });
        }
      } catch (error) {
        console.error('Error loading form data:', error);
        toast.error('Failed to load form data');
      } finally {
        setInitializing(false);
      }
    };
    
    loadFormData();
  }, [id, form]);

  const onSubmit = async (data: BookFormValues) => {
    setLoading(true);
    try {
      if (id) {
        await updateBook(id, data);
        toast.success('Book updated successfully');
      } else {
        await createBook(data);
        toast.success('Book created successfully');
      }
      router.push('/books');
    } catch (error) {
      console.error('Error saving book:', error);
      toast.error(id ? 'Failed to update book' : 'Failed to create book');
      setLoading(false);
    }
  };
  
  if (initializing) {
    return (
      <div className="flex justify-center items-center h-64">
        <Loader2 className="h-8 w-8 animate-spin text-muted-foreground" />
      </div>
    );
  }

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <FormField
            control={form.control}
            name="title"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Title</FormLabel>
                <FormControl>
                  <Input {...field} placeholder="Book title" />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          
          <FormField
            control={form.control}
            name="publicationYear"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Publication Year</FormLabel>
                <FormControl>
                  <Input 
                    type="number" 
                    {...field} 
                    min={1000}
                    max={new Date().getFullYear()}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          
          <FormField
            control={form.control}
            name="isbn"
            render={({ field }) => (
              <FormItem>
                <FormLabel>ISBN</FormLabel>
                <FormControl>
                  <Input {...field} placeholder="ISBN number" />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          
          <FormField
            control={form.control}
            name="pageCount"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Page Count</FormLabel>
                <FormControl>
                  <Input 
                    type="number" 
                    {...field} 
                    min={1}
                    max={10000}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          
          <FormField
            control={form.control}
            name="authorId"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Author</FormLabel>
                <Select 
                  onValueChange={(value) => field.onChange(parseInt(value))}
                  defaultValue={field.value ? field.value.toString() : undefined}
                >
                  <FormControl>
                    <SelectTrigger>
                      <SelectValue placeholder="Select an author" />
                    </SelectTrigger>
                  </FormControl>
                  <SelectContent>
                    {authors.map((author) => (
                      <SelectItem 
                        key={author.id} 
                        value={author.id.toString()}
                      >
                        {`${author.firstName} ${author.lastName}`}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
                <FormMessage />
              </FormItem>
            )}
          />
          
          <FormField
            control={form.control}
            name="genreId"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Genre</FormLabel>
                <Select 
                  onValueChange={(value) => field.onChange(parseInt(value))}
                  defaultValue={field.value ? field.value.toString() : undefined}
                >
                  <FormControl>
                    <SelectTrigger>
                      <SelectValue placeholder="Select a genre" />
                    </SelectTrigger>
                  </FormControl>
                  <SelectContent>
                    {genres.map((genre) => (
                      <SelectItem 
                        key={genre.id} 
                        value={genre.id.toString()}
                      >
                        {genre.name}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>
        
        <FormField
          control={form.control}
          name="coverImageUrl"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Cover Image URL</FormLabel>
              <FormControl>
                <Input {...field} placeholder="URL to the book cover image" />
              </FormControl>
              <FormDescription>
                Provide a URL to an image of the book cover
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />
        
        <FormField
          control={form.control}
          name="description"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Description</FormLabel>
              <FormControl>
                <Textarea 
                  {...field} 
                  placeholder="Book description" 
                  rows={5}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        
        <div className="flex justify-end gap-4">
          <Button 
            type="button" 
            variant="outline" 
            onClick={() => router.push('/books')}
          >
            Cancel
          </Button>
          <Button type="submit" disabled={loading}>
            {loading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
            {id ? 'Update' : 'Create'} Book
          </Button>
        </div>
      </form>
    </Form>
  );
}