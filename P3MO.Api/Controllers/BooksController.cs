using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P3MO.Repository.Data;
using P3MO.Shared.Models.Data;

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
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            book.UpdatedAt = DateTime.UtcNow;
            _context.Entry(book).State = EntityState.Modified;

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

            return NoContent();
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            book.CreatedAt = DateTime.UtcNow;
            book.UpdatedAt = DateTime.UtcNow;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
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
    }
}
