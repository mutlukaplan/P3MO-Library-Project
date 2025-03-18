import { z } from 'zod';

export const bookSchema = z.object({
  id: z.number().optional(),
  title: z.string().min(1, { message: 'Title is required' }).max(200, { message: 'Title must be less than 200 characters' }),
  publicationYear: z.coerce
    .number()
    .min(1000, { message: 'Publication year must be at least 1000' })
    .max(new Date().getFullYear(), { message: `Publication year cannot be in the future` }),
  isbn: z.string().max(20, { message: 'ISBN must be less than 20 characters' }).optional().or(z.literal('')),
  coverImageUrl: z.string().max(500, { message: 'Cover image URL must be less than 500 characters' }).url({ message: 'Must be a valid URL' }).optional().or(z.literal('')),
  description: z.string().max(2000, { message: 'Description must be less than 2000 characters' }).optional().or(z.literal('')),
  pageCount: z.coerce
    .number()
    .min(1, { message: 'Page count must be at least 1' })
    .max(10000, { message: 'Page count must be less than 10,000' }),
  authorId: z.coerce.number().min(1, { message: 'Author is required' }),
  genreId: z.coerce.number().min(1, { message: 'Genre is required' }),
});

export type BookFormValues = z.infer<typeof bookSchema>;

