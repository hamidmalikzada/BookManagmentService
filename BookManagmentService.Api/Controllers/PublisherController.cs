using AutoMapper;
using BookManagmentService.Domain.Domains;
using BookManagmentService.Domain.Interfaces;
using BookManagmentService.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using static BookManagmentService.Domain.ViewModels.BookVM;
using static BookManagmentService.Domain.ViewModels.PublisherVM;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookManagmentService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherRepository _publisherRepository;
        private readonly IMapper _mapper;
        public PublisherController(IPublisherRepository publisherRepository, IMapper mapper)
        {
            _publisherRepository = publisherRepository;
            _mapper = mapper;   
        }
        // GET: api/<PublisherController>
        [HttpGet]
        public async Task<ActionResult<PublisherIndexVM>> Get()
        {
            return Ok(await _publisherRepository.GetAllPublishers());
        }

        // GET api/<PublisherController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherIndexVM>> GetById(int id)
        {
            try
            {
                var publisher = await _publisherRepository.GetPublisherById(id);
                return Ok(publisher);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Could not find publisher with id {id}");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // POST api/<PublisherController>
        [HttpPost]
        public async Task<ActionResult<Publisher>> Post([FromBody] PublisherAddVM publisher)
        {
            var publishr = _mapper.Map<Publisher>(publisher);
            return await _publisherRepository.AddPublisher(publishr);
        }

        // PUT api/<PublisherController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Publisher>> Put(int id, [FromBody] PublisherEditVM publisher)
        {
            var publishr = _mapper.Map<Publisher>(publisher);
            return await _publisherRepository.UpdatePublisher(id, publishr);
        }

        // DELETE api/<PublisherController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _publisherRepository.DeletePublisher(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Could not find publisher with id {id}");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
