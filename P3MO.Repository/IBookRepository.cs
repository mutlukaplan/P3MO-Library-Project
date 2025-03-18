using P3MO.Shared.Models.Data;
using P3MO.Shared.Models.Dto;

namespace P3MO.Repository
{
    public interface IBookRepository
    {
        Task UpdateBook(int id, BookUpdateDTO bookDTO);
        Task<BookDTO> PostBook(BookCreateDTO bookDTO);
        Task<Book> GetBook(int id);
        Task<IEnumerable<Book>> GetBooks(); 
        Task DeleteBook(int id);
        Task<IEnumerable<object>> GetBooksByGenre();
        bool BookExists(int id);
    }
}