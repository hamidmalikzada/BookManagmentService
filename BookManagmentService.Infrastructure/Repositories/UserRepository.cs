using AutoMapper;
using BookManagmentService.Domain.Domains;
using BookManagmentService.Domain.Interfaces;
using BookManagmentService.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserEntity = BookManagmentService.Infrastructure.Entities.User;
using UserDomain = BookManagmentService.Domain.Domains.User;
using Microsoft.EntityFrameworkCore;
using static BookManagmentService.Domain.ViewModels.PublisherVM;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace BookManagmentService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookManagmentDbContext _bookManagmentDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<IUserRepository> _logger;

        public UserRepository(BookManagmentDbContext bookManagmentDbContext, IMapper mapper, ILogger<IUserRepository> logger)
        {
            _bookManagmentDbContext = bookManagmentDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<User> CreateAsync(User user)
        {
            try
            {
                var userToAdd = _mapper.Map<UserEntity>(user);
                userToAdd.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                await _bookManagmentDbContext.Users.AddAsync(userToAdd);
                await _bookManagmentDbContext.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateException ex)
            {

                throw new DbUpdateException("error while adding user to database");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }

        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existingUser = await _bookManagmentDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (existingUser == null)
                {
                    throw new KeyNotFoundException($"Could not find user with id {id}");
                }

                _bookManagmentDbContext.Users.Remove(existingUser);
                await _bookManagmentDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
            return false;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                var users = await _bookManagmentDbContext.Users.ToListAsync();
                if (users == null)
                {
                    throw new KeyNotFoundException("could not find any users");
                }
                return _mapper.Map<IEnumerable<UserDomain>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                var user = await _bookManagmentDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    throw new KeyNotFoundException($"Could not find user with id {id}");
                }
                return _mapper.Map<UserDomain>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }

        }

        public async Task<UserDomain> GetUserPasswordByNameAsync(string userName)
        {
            var user = await _bookManagmentDbContext.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();
            
            return _mapper.Map<UserDomain>(user);
        }

        public async Task<User> UpdateAsync(int id, User user)
        {
            try
            {
                var existingUser = await _bookManagmentDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (existingUser == null)
                {
                    throw new KeyNotFoundException($"Could not find user with id {id}");
                }

                _mapper.Map<UserEntity>(existingUser);
                existingUser.UserName = user.UserName;
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                await _bookManagmentDbContext.SaveChangesAsync();

                return _mapper.Map<UserDomain>(existingUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
