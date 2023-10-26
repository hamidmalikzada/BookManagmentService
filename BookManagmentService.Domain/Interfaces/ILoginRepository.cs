using BookManagmentService.Domain.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagmentService.Domain.Interfaces
{
    public interface ILoginRepository
    {
        //Task<User> Login(User user);
        string GenerateJSONWebToken(User user);
        User AuthenticateUser(Login login);
    }
}
