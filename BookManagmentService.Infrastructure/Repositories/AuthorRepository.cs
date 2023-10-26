using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookManagmentService.Domain.Domains;
using BookManagmentService.Domain.Interfaces;
using BookManagmentService.Infrastructure.Data;
using BookManagmentService.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookManagmentService.Domain.ViewModels.AuthorVM;
using Author = BookManagmentService.Domain.Domains.Author;
using AuthorDomain = BookManagmentService.Domain.Domains.Author;
using AuthorEntity = BookManagmentService.Infrastructure.Entities.Author;
using BookAuthor = BookManagmentService.Domain.Domains.BookAuthor;

namespace BookManagmentService.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookManagmentDbContext _bookManagmentDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<IAuthorRepository> _logger;
        public AuthorRepository(BookManagmentDbContext bookManagmentDbContext, IMapper mapper, ILogger<IAuthorRepository> logger)
        {
            _bookManagmentDbContext = bookManagmentDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AuthorDomain> AddAuthor(Author author)
        {
            try
            {
                if (author == null)
                {
                    throw new ArgumentNullException(nameof(author), "Author cannot be null.");
                }
                var autor = _mapper.Map<AuthorEntity>(author);
                _bookManagmentDbContext.Authors.Add(autor);
                await _bookManagmentDbContext.SaveChangesAsync();
                return author;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<bool> AuthorExists(int id)
        {
            return await _bookManagmentDbContext.Authors.AnyAsync(e => e.Id == id);
        }

        public void DeleteAuthor(int id)
        {
            try
            {
                var author = _bookManagmentDbContext.Authors.Find(id);
                if (author != null)
                {
                    _bookManagmentDbContext.Authors.Remove(author);
                    _bookManagmentDbContext.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException($"Author not found with Id: {id}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<AuthorIndexVM>> GetAllAuthors()
        {
            try
            {
                var authors = await _bookManagmentDbContext.Authors
                        .Include(b => b.BookAuthors)
                        .ThenInclude(a => a.Book)
                        .ThenInclude(p => p.Publisher)
                        .ToListAsync();

                return _mapper.Map<IEnumerable<AuthorIndexVM>>(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Enumerable.Empty<AuthorIndexVM>();
            }
        }

        public async Task<AuthorIndexVM> GetAuthorById(int id)
        {
            try
            {
                var author = await _bookManagmentDbContext.Authors
                    .Where(x => x.Id == id)
                        .Include(b => b.BookAuthors)
                        .ThenInclude(a => a.Book)
                        .ThenInclude(p => p.Publisher)
                    .ProjectTo<AuthorIndexVM>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (author == null)
                {
                    return null;
                }
                return author;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<AuthorAddVM> UpdateAuthor(int id, AuthorAddVM author)
        {
            try
            {
                var authorToUpdate = _bookManagmentDbContext.Authors.Find(id);
                if (authorToUpdate == null)
                {
                    throw new KeyNotFoundException($"Could not find author with id {id}");
                }

                authorToUpdate.FirstName = author.FirstName;
                authorToUpdate.LastName = author.LastName;

                _bookManagmentDbContext.Authors.Update(authorToUpdate);
                await _bookManagmentDbContext.SaveChangesAsync();
                return author;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        //public async Task<Author> UpdateAuthor(int id, Author author, string[] selectedBooks)
        //{
        //    if (id != author.Id)
        //    {
        //        throw new Exception($"Could not find author with id: {id}");
        //    }
        //    try
        //    {
        //        var authorToUpdate = _mapper.Map<AuthorEntity>(author);
        //        authorToUpdate = await _bookManagmentDbContext.Authors
        //            .Include(b => b.BookAuthors)
        //            .ThenInclude(a => a.Book)
        //            .FirstOrDefaultAsync(x => x.Id == id);

        //        var autor = _mapper.Map<AuthorDomain>(authorToUpdate);

        //        UpdateAuthorBooks(selectedBooks, autor);
        //        await _bookManagmentDbContext.SaveChangesAsync();

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.ToString());
        //        throw new Exception("Something went wrong!");
        //    }
        //}

        //private void UpdateAuthorBooks(string[] selectedBooks, Author authorToUpdate)
        //{
        //    if (selectedBooks == null)
        //    {
        //        authorToUpdate.BookAuthors = new List<BookAuthor>();
        //        return;
        //    }

        //    var selectedAuthorsHS = new HashSet<string>(selectedBooks);
        //    var authorBooks = new HashSet<int>
        //        (authorToUpdate.BookAuthors.Select(c => c.Book.Id));
        //    foreach (var book in _bookManagmentDbContext.Books)
        //    {
        //        if (selectedAuthorsHS.Contains(book.Id.ToString()))
        //        {
        //            if (!authorBooks.Contains(book.Id))
        //            {
        //                authorToUpdate.BookAuthors.Add(new BookAuthor {BookId = book.Id, AuthorId = authorToUpdate.Id });
        //            }
        //        }
        //        else
        //        {
        //            if (authorBooks.Contains(book.Id))
        //            {
        //                BookAuthor bookToRemove = authorToUpdate.BookAuthors.FirstOrDefault(i => i.BookId == book.Id);
        //                _bookManagmentDbContext.Remove(bookToRemove);
        //            }
        //        }
        //    }
        //}
    }
}
