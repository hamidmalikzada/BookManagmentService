using AutoMapper;
using BookManagmentService.Domain.Domains;
using BookManagmentService.Domain.Interfaces;
using BookManagmentService.Domain.ViewModels;
using BookManagmentService.Infrastructure.Entities;
using BookManagmentService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using static BookManagmentService.Domain.ViewModels.AuthorVM;
using static BookManagmentService.Domain.ViewModels.BookVM;
using Book = BookManagmentService.Domain.Domains.Book;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookManagmentService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BookController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        // GET: api/<BookController>
        [HttpGet]
        public async Task<ActionResult<BookIndexVM>> Get()
        {
            var books = await _bookRepository.GetAllBooks();

            if (books.Any())
            {
                return Ok(books);
            }
            else
            {
                return NotFound("No books found!");
            }
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookIndexVM>> GetById(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound($"Could not find book with id: {id}");
            }
            return Ok(book);
        }




        //POST api/<BookController>
        [HttpPost]
        public async Task<ActionResult<Book>> Post(BookAuthAddVM book)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            try
            {

                //var bok = _mapper.Map<Book>(book);
                var result = await _bookRepository.AddBook(book);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }


        }

        // PUT api/<BookController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BookAuthAddVM>> Put(int id, BookAuthAddVM updatedBook)
        {
            return Ok(await _bookRepository.UpdateBook(id, updatedBook));
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _bookRepository.DeleteBook(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Could not find book with id {id}");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
