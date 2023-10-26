using BookManagmentService.Domain.Domains;
using BookManagmentService.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BookManagmentService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;
        public AuthenticationController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }
        [HttpPost]
        public IActionResult Login([FromBody] Login login)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                IActionResult response = Unauthorized();
                var user = _loginRepository.AuthenticateUser(login);
                if (user != null)
                {
                    var tokenString = _loginRepository.GenerateJSONWebToken(user);
                    response = Ok(new { token = tokenString, apiId = user.Id });
                }
                return response;
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

        }
    }
}
