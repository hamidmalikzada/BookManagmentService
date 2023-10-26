using AutoMapper;
using BookManagmentService.Domain.Domains;
using BookManagmentService.Domain.Interfaces;
using BookManagmentService.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BookManagmentService.Domain.ViewModels.AuthorVM;
using Author = BookManagmentService.Domain.Domains.Author;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookManagmentService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        public AuthorController(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
        // GET: api/<AuthorController>
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<AuthorIndexVM>> Get()
        {
            var authors = await _authorRepository.GetAllAuthors();

            if (authors.Any())
            {
                return Ok(authors);
            }
            else
            {
                return NotFound("No authors found!");
            }
        }

        // GET api/<AuthorController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorIndexVM>> GetById(int id)
        {
            var result = await _authorRepository.GetAuthorById(id);
            if (result == null)
            {
                return NotFound($"Could not find author with id: {id}");
            }
            return Ok(result);
        }

        // POST api/<AuthorController>
        [HttpPost]
        public async Task<ActionResult<Author>> Post([FromBody] AuthorAddVM author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            try
            {
                var autor = _mapper.Map<Author>(author);
                var result = await _authorRepository.AddAuthor(autor);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // PUT api/<AuthorController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorAddVM>> Put(int id, AuthorAddVM author)
        {
            try
            {
                return Ok(await _authorRepository.UpdateAuthor(id, author));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // DELETE api/<AuthorController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _authorRepository.DeleteAuthor(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Could not find author with id {id}");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
