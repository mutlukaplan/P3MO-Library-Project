using P3MO.Repository;
using P3MO.Shared.Models.Data;
using P3MO.Shared.Models.Dto;

namespace P3MO.Domain
{
    public class BookManager : IBookManager
    {
        private readonly IBookRepository _bookRepository;
        public BookManager(IBookRepository bookRepository )
        {
               _bookRepository = bookRepository; 
        }
        public bool BookExists(int id)
        {
            return _bookRepository.BookExists(id);
        }

        public Task DeleteBook(int id)
        {
            return _bookRepository.DeleteBook(id);
        }

        public Task<Book> GetBook(int id)
        {
            return _bookRepository.GetBook(id);
        }

        public Task<IEnumerable<Book>> GetBooks()
        {
            return _bookRepository.GetBooks();
        }

        public Task<IEnumerable<object>> GetBooksByGenre()
        {
            return _bookRepository.GetBooksByGenre();
        }

        public Task<BookDTO> PostBook(BookCreateDTO bookDTO)
        {
            return _bookRepository.PostBook(bookDTO);
        }

        public Task UpdateBook(int id, BookUpdateDTO bookDTO)
        {
            return _bookRepository.UpdateBook(id, bookDTO);
        }
    }
}
