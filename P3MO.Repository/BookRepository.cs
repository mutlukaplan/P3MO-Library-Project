using Microsoft.EntityFrameworkCore;
using P3MO.Repository.Data;
using P3MO.Shared.Models.Data;
using P3MO.Shared.Models.Dto;

namespace P3MO.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly P3MOContext _context;

        public BookRepository(P3MOContext context)
        {
            _context = context;       
        }

        public bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        public async Task DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return ;
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Book?> GetBook(int id)
        {
            var book = await _context.Books
                        .Include(b => b.Author)
                        .Include(b => b.Genre)
                        .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return null;
            return book;
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Genre)
                    .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetBooksByGenre()
        {
            var booksByGenre = await _context.Genres
                            .Select(g => new
                            {
                                GenreName = g.Name,
                                BookCount = g.Books.Count
                            })
                            .ToListAsync();

            return booksByGenre;
        }

        public async Task<BookDTO> PostBook(BookCreateDTO bookDTO)
        {
            try
            {
                var book = new Book
                {
                    Title = bookDTO.Title,
                    PublicationYear = bookDTO.PublicationYear,
                    ISBN = bookDTO.ISBN,
                    CoverImageUrl = bookDTO.CoverImageUrl,
                    Description = bookDTO.Description,
                    PageCount = bookDTO.PageCount,
                    AuthorId = bookDTO.AuthorId,
                    GenreId = bookDTO.GenreId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                // Reload the entity with related data
                await _context.Entry(book).Reference(b => b.Author).LoadAsync();
                await _context.Entry(book).Reference(b => b.Genre).LoadAsync();

                return  MapToDTO(book);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occured during db create: {ex.Message}");
            }
        }

        public async Task UpdateBook(int id, BookUpdateDTO bookDTO)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return;
            }

            // Update entity from DTO (mapping)
            book.Title = bookDTO.Title;
            book.PublicationYear = bookDTO.PublicationYear;
            book.ISBN = bookDTO.ISBN;
            book.CoverImageUrl = bookDTO.CoverImageUrl;
            book.Description = bookDTO.Description;
            book.PageCount = bookDTO.PageCount;
            book.AuthorId = bookDTO.AuthorId;
            book.GenreId = bookDTO.GenreId;
            book.UpdatedAt = DateTime.UtcNow;

            // EF will handle navigation properties

            try
            {
                await _context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                throw new Exception($"Error occured: {ex.Message}");
            }
        }


        private static BookDTO MapToDTO(Book book)
        {
            return new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                PublicationYear = book.PublicationYear,
                ISBN = book.ISBN,
                CoverImageUrl = book.CoverImageUrl,
                Description = book.Description,
                PageCount = book.PageCount,
                AuthorId = book.AuthorId,
                GenreId = book.GenreId,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt,
                Author = book.Author != null ? new AuthorDTO
                {
                    Id = book.Author.Id,
                    FirstName = book.Author.FirstName,
                    LastName = book.Author.LastName,
                    Biography = book.Author.Biography,
                    BirthDate = book.Author.BirthDate
                } : null,
                Genre = book.Genre != null ? new GenreDTO
                {
                    Id = book.Genre.Id,
                    Name = book.Genre.Name,
                    Description = book.Genre.Description
                } : null
            };
        }
    }
}
