using BookManagmentService.Domain.Domains;
using BookManagmentService.Domain.Interfaces;
using BookManagmentService.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookManagmentService.Infrastructure.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly  BookManagmentDbContext _bookManagmentDbContext;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public LoginRepository(BookManagmentDbContext bookManagmentDbContext, IConfiguration configuration, IUserRepository userRepository)
        {
            _bookManagmentDbContext = bookManagmentDbContext;
            _configuration = configuration;
            _userRepository = userRepository;
        }
        //public Task<User> Login(User user)
        //{
        //    //throw new NotImplementedException();
        //    IActionResult response = Unauthorized();
        //    var usr = AuthenticateUser(user);
        //    if (usr != null)
        //    {
        //        var tokenString = GenerateJSONWebToken(usr);
        //        response = Ok(new { token = tokenString, apiId = user.Id });
        //    }
        //    return response;
        //}


        public string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.PrimarySid, userInfo.Id.ToString()),
                    new Claim(ClaimTypes.Upn, userInfo.UserName),
                    new Claim(ClaimTypes.Name, userInfo.UserName),

                }),
                Expires = DateTime.UtcNow.AddYears(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Issuer"],
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public User AuthenticateUser(Login login)
        {
            var user = _userRepository.GetUserPasswordByNameAsync(login.UserName).Result;
            if (user == null)
            {
                throw new KeyNotFoundException("Could not find user");
            }

            var match = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
            if (match)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
