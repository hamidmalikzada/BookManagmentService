using BookManagmentService.Domain.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookManagmentService.Domain.ViewModels.BookVM;

namespace BookManagmentService.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookIndexVM>> GetAllBooks();
        Task<BookIndexVM> GetBookById(int id);
        Task<BookAuthAddVM> AddBook(BookAuthAddVM book);
        Task<BookAuthAddVM> UpdateBook(int id, BookAuthAddVM updatedBook);
        void DeleteBook(int id);
        Task<bool> BookExists(int id);
    }
}
