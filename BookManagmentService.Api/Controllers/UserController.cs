using AutoMapper;
using BookManagmentService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

using UserEntity = BookManagmentService.Infrastructure.Entities.User;
using UserDomain = BookManagmentService.Domain.Domains.User;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookManagmentService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<UserDomain>> Get()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDomain>> Get(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"could not find aurthor with id {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<UserDomain>> Post([FromBody] UserDomain user)
        {
            _mapper.Map<UserEntity>(user);
           var createdUser = await _userRepository.CreateAsync(user);
            return Ok(createdUser);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDomain>> Put(int id, [FromBody] UserDomain user)
        {
            _mapper.Map<UserEntity>(user);
            var updatedUser = await _userRepository.UpdateAsync(id, user);
            return Ok(updatedUser);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _userRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }            
        }
    }
}
