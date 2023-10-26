using BookManagmentService.Domain.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookManagmentService.Domain.ViewModels.AuthorVM;

namespace BookManagmentService.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<AuthorIndexVM>> GetAllAuthors();
        Task<AuthorIndexVM> GetAuthorById(int id);
        Task<Author> AddAuthor(Author author);
        Task<AuthorAddVM> UpdateAuthor(int id, AuthorAddVM author);
        void DeleteAuthor(int id);
        Task<bool> AuthorExists(int id);
    }
}
