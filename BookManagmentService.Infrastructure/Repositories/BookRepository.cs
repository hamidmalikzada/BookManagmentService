using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookManagmentService.Domain.Domains;
using BookManagmentService.Domain.Interfaces;
using BookManagmentService.Domain.ViewModels;
using BookManagmentService.Infrastructure.Data;
using BookManagmentService.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Book = BookManagmentService.Domain.Domains.Book;
using BookAuthor = BookManagmentService.Domain.Domains.BookAuthor;
using BookDomain = BookManagmentService.Domain.Domains.Book;
using BookEntity = BookManagmentService.Infrastructure.Entities.Book;
using BookAuthorDomain = BookManagmentService.Domain.Domains.BookAuthor;
using BookAuthorEntity = BookManagmentService.Infrastructure.Entities.BookAuthor;
using static BookManagmentService.Domain.ViewModels.BookVM;
using static BookManagmentService.Domain.ViewModels.AuthorVM;
using System.Net;

namespace BookManagmentService.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookManagmentDbContext _bookManagmentDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<IBookRepository> _logger;
        public BookRepository(BookManagmentDbContext bookManagmentDbContext, IMapper mapper, ILogger<IBookRepository> logger)
        {
            _bookManagmentDbContext = bookManagmentDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BookAuthAddVM> AddBook(BookAuthAddVM book)
        {
            try
            {
                var bookEntity = _mapper.Map<BookEntity>(book);
                var authors = _bookManagmentDbContext.Authors
                    .Where(a => book.AuthorIds.Contains(a.Id))
                    .ToList();

                if (bookEntity.BookAuthors == null)
                {
                    bookEntity.BookAuthors = new List<BookAuthorEntity>();
                }
                // Associate authors with the book
                foreach (var author in authors)
                {
                    var bookAuthor = new BookAuthorEntity
                    {
                        Author = author,
                        Book = bookEntity
                    };
                    bookEntity.BookAuthors.Add(bookAuthor);
                }

                _bookManagmentDbContext.Books.Add(bookEntity);
                await _bookManagmentDbContext.SaveChangesAsync();
                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Something went wrong!");
            }
        }



        public async Task<bool> BookExists(int id)
        {
            return await _bookManagmentDbContext.Books.AnyAsync(e => e.Id == id);
        }

        public void DeleteBook(int id)
        {
            try
            {
                var book = _bookManagmentDbContext.Books.Find(id);
                if (book != null)
                {
                    _bookManagmentDbContext.Books.Remove(book);
                    _bookManagmentDbContext.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException($"Book with id not found: {id}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<BookIndexVM>> GetAllBooks()
        {
            try
            {
                var books = await _bookManagmentDbContext.Books
                        .Include(b => b.BookAuthors)
                        .ThenInclude(a => a.Author)
                        .Include(p => p.Publisher)
                        .ToListAsync();

                var BookVM = _mapper.Map<IEnumerable<BookIndexVM>>(books);

                return BookVM;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Enumerable.Empty<BookIndexVM>();
            }
        }

        public async Task<BookIndexVM> GetBookById(int id)
        {
            try
            {
                var book = await _bookManagmentDbContext.Books
                    .Where(x => x.Id == id)
                    .Include(b => b.BookAuthors)
                    .ThenInclude(a => a.Author)
                    .Include(p => p.Publisher)
                    .ProjectTo<BookIndexVM>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (book == null)
                {
                    return null;
                }
                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<BookAuthAddVM> UpdateBook(int id, BookAuthAddVM updatedBook)
        {
            // Retrieve the existing book from the database
            try
            {
                var existingBook = await _bookManagmentDbContext.Books
                       .Include(b => b.Authors)
                       .FirstOrDefaultAsync(b => b.Id == id);

                if (existingBook == null)
                {
                    throw new KeyNotFoundException($"Book with id {id} not found");
                }

                existingBook.Title = updatedBook.Title;
                existingBook.PublishingDate = updatedBook.PublishingDate;
                existingBook.Category = (Entities.Category)updatedBook.CategoryId;
                existingBook.PublisherId = updatedBook.PublisherId;

                // Update the relationships with authors (assuming you pass a list of AuthorIds in updatedBook)
                existingBook.Authors.Clear(); // Remove existing authors
                var selectedAuthors = await _bookManagmentDbContext.Authors
                    .Where(a => updatedBook.AuthorIds.Contains(a.Id))
                    .ToListAsync();
                existingBook.Authors = selectedAuthors;

                await _bookManagmentDbContext.SaveChangesAsync();

                return _mapper.Map<BookAuthAddVM>(existingBook);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }

        }

        //public async Task<BookDomain> UpdateBook(int id, Book book, string[] selectedAuthors)
        //{
        //    if (id != book.Id)
        //    {
        //        throw new Exception($"Could not find book with id: {id}");
        //    }
        //    try
        //    {
        //        var bookToUpdate = _mapper.Map<BookEntity>(book);
        //        bookToUpdate = await _bookManagmentDbContext.Books
        //            .Include(b => b.BookAuthors)
        //            .ThenInclude(a => a.Author)
        //            .AsNoTracking()
        //            .FirstOrDefaultAsync(x => x.Id == id);

        //        var bok = _mapper.Map<BookDomain>(bookToUpdate);

        //        UpdateAuthorBooks(selectedAuthors, bok);
        //        await _bookManagmentDbContext.SaveChangesAsync();

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.ToString());
        //        throw new Exception("Something went wrong!");
        //    }
        //}


        //private void UpdateAuthorBooks(string[] selectedAuthors, Book bookToUpdate)
        //{
        //    if (bookToUpdate == null)
        //    {
        //        bookToUpdate.BookAuthors = new List<BookAuthor>();
        //        return;
        //    }

        //    var selectedAuthorsHS = new HashSet<string>(selectedAuthors);
        //    var authorBooks = new HashSet<int>
        //        (bookToUpdate.BookAuthors.Select(c => c.Author.Id));
        //    foreach (var author in _bookManagmentDbContext.Authors)
        //    {
        //        if (selectedAuthorsHS.Contains(author.Id.ToString()))
        //        {
        //            if (!authorBooks.Contains(author.Id))
        //            {
        //                bookToUpdate.BookAuthors.Add(new BookAuthor { BookId = bookToUpdate.Id, AuthorId = author.Id });
        //            }
        //        }
        //        else
        //        {
        //            if (authorBooks.Contains(author.Id))
        //            {
        //                BookAuthor bookToRemove = bookToUpdate.BookAuthors.FirstOrDefault(i => i.AuthorId == author.Id);
        //                _bookManagmentDbContext.Remove(bookToRemove);
        //            }
        //        }
        //    }
        //}

    }
}
