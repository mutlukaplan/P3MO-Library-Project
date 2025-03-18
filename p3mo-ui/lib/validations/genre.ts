import { z } from 'zod';

export const genreSchema = z.object({
  id: z.number().optional(),
  name: z.string().min(1, { message: 'Name is required' }).max(100, { message: 'Name must be less than 100 characters' }),
  description: z.string().max(500, { message: 'Description must be less than 500 characters' }).optional().or(z.literal('')),
});

export type GenreFormValues = z.infer<typeof genreSchema>;