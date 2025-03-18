using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P3MO.Application;
using P3MO.Shared.Models.Data;
using P3MO.Shared.Models.Dto;

namespace P3MO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _bookService.GetBooks();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookService.GetBook(id);

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
            var booksByGenre = await _bookService.GetBooksByGenre();

            return Ok(booksByGenre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDTO bookDTO)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID provided");
            }

            var book = await _bookService.GetBook(id);

            if (book == null)
            {
                return NotFound();
            }

            try
            {
                await _bookService.UpdateBook(id, bookDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_bookService.BookExists(id))
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
                var bookdto = await _bookService.PostBook(bookDTO);
                return CreatedAtAction("GetBook", new { id = bookdto.Id }, bookdto);
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
            var book = await _bookService.GetBook(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookService.DeleteBook(id);

            return NoContent();
        }
    }
}
