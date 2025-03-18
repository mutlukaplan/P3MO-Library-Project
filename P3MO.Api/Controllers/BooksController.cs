using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P3MO.Repository.Data;
using P3MO.Shared.Models.Data;
using P3MO.Shared.Models.Dto;

namespace P3MO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {

        private readonly P3MOContext _context;

        public BooksController(P3MOContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // GET: api/Books/ByGenre
        [HttpGet("ByGenre")]
        public async Task<ActionResult<IEnumerable<object>>> GetBooksByGenre()
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDTO bookDTO)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID provided");
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
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
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating book: {ex.Message}");
            }

            return NoContent();
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<BookDTO>> PostBook(BookCreateDTO bookDTO)
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

                return CreatedAtAction("GetBook", new { id = book.Id }, MapToDTO(book));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating book: {ex.Message}");
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
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
