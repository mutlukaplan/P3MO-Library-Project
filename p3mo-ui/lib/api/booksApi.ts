import { BookFormValues } from '../validations/book';

export interface Book {
    id: number;
    title: string;
    publicationYear: number;
    isbn: string;
    coverImageUrl: string;
    description: string;
    pageCount: number;
    authorId: number;
    genreId: number;
    createdAt: string;
    updatedAt: string;
    author: Author;
    genre: Genre;
}

export interface Author {
    id: number;
    firstName: string;
    lastName: string;
    biography: string;
    birthDate: string;
    fullName?: string;
}

export interface Genre {
    id: number;
    name: string;
    description: string;
}

export interface GenreCount {
    genreName: string;
    bookCount: number;
}

export async function getBooks(): Promise<Book[]> {
    const response = await fetch('/api/books');
    if (!response.ok) {
        throw new Error('Failed to fetch books');
    }

    return response.json();
}

export async function getBookById(id: number): Promise<Book> {
    const response = await fetch(`/api/books/${id}`);

    if (!response.ok) {
        throw new Error('Failed to fetch book details');
    }

    return response.json();
}

export async function createBook(book: BookFormValues): Promise<Book> {
    const response = await fetch('/api/books', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(book),
    });

    if (!response.ok) {
        throw new Error('Failed to create book');
    }

    return response.json();
}

export async function updateBook(id: number, book: BookFormValues): Promise<void> {
    const response = await fetch(`/api/books/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ id, ...book }),
    });

    if (!response.ok) {
        throw new Error('Failed to update book');
    }
}

export async function deleteBook(id: number): Promise<void> {
    const response = await fetch(`/api/books/${id}`, {
        method: 'DELETE',
    });

    if (!response.ok) {
        throw new Error('Failed to delete book');
    }
}

export async function getBooksByGenre(): Promise<GenreCount[]> {
    const response = await fetch('/api/books/by-genre');

    if (!response.ok) {
        throw new Error('Failed to fetch books by genre');
    }

    return response.json();
}

export async function getAuthors(): Promise<Author[]> {
    const response = await fetch('/api/authors');

    if (!response.ok) {
        throw new Error('Failed to fetch authors');
    }

    return response.json();
}

export async function getGenres(): Promise<Genre[]> {
    const response = await fetch('/api/genres');

    if (!response.ok) {
        throw new Error('Failed to fetch genres');
    }

    return response.json();
}

export function generateBookListPdf(): void {
    window.open('/api/pdf/books', '_blank');
}

export function generateBookDetailPdf(id: number): void {
    window.open(`/api/pdf/books/${id}`, '_blank');
}