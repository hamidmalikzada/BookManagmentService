using BookManagmentService.Domain.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagmentService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetUserPasswordByNameAsync(string userName);

        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(int id, User user);
        Task<bool> DeleteAsync(int id);
    }
}
