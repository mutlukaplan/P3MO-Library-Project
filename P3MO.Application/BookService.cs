using P3MO.Domain;
using P3MO.Shared.Models.Data;
using P3MO.Shared.Models.Dto;

namespace P3MO.Application
{
    public class BookService : IBookService
    {
        private readonly IBookManager _bookManager;
        public BookService(IBookManager bookManager)
        {
            _bookManager=bookManager;
        }
        public bool BookExists(int id)
        {
            return _bookManager.BookExists(id);
        }

        public Task DeleteBook(int id)
        {
            return _bookManager.DeleteBook(id);
        }

        public Task<Book> GetBook(int id)
        {
            return _bookManager.GetBook(id);
        }

        public Task<IEnumerable<Book>> GetBooks()
        {
            return _bookManager.GetBooks();
        }

        public Task<IEnumerable<object>> GetBooksByGenre()
        {
            return _bookManager.GetBooksByGenre();
        }

        public Task<BookDTO> PostBook(BookCreateDTO bookDTO)
        {
            return _bookManager.PostBook(bookDTO);
        }

        public Task UpdateBook(int id, BookUpdateDTO bookDTO)
        {
            return _bookManager.UpdateBook(id, bookDTO);
        }
    }
}
