using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookManagmentService.Domain.Domains;
using BookManagmentService.Domain.Interfaces;
using BookManagmentService.Infrastructure.Data;
using BookManagmentService.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using static BookManagmentService.Domain.ViewModels.BookVM;
using static BookManagmentService.Domain.ViewModels.PublisherVM;
using Publisher = BookManagmentService.Domain.Domains.Publisher;
using PublisherDomain = BookManagmentService.Domain.Domains.Publisher;
using PublisherEntity = BookManagmentService.Infrastructure.Entities.Publisher;


namespace BookManagmentService.Infrastructure.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly BookManagmentDbContext _bookManagmentDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<IPublisherRepository> _logger;
        public PublisherRepository(BookManagmentDbContext bookManagmentDbContext, IMapper mapper, ILogger<IPublisherRepository> logger)
        {
            _bookManagmentDbContext = bookManagmentDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PublisherDomain> AddPublisher(Publisher publisher)
        {
            try
            {
                var publishr = _mapper.Map<PublisherEntity>(publisher);
                _bookManagmentDbContext.Publishers.Add(publishr);
                await _bookManagmentDbContext.SaveChangesAsync();
                return publisher;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Something went wrong!");
            }
        }

        public void DeletePublisher(int id)
        {
            try
            {
                var publisher = _bookManagmentDbContext.Publishers.Find(id);
                if (publisher != null)
                {
                    _bookManagmentDbContext.Publishers.Remove(publisher);
                    _bookManagmentDbContext.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException($"Could not find publiser with id: {id}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<PublisherIndexVM>> GetAllPublishers()
        {
            try
            {
                var publishers = await _bookManagmentDbContext.Publishers
                    .Include(x => x.Books)
                    .ToListAsync();
                return _mapper.Map<IEnumerable<PublisherIndexVM>>(publishers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Enumerable.Empty<PublisherIndexVM>();
            }
        }

        public async Task<PublisherIndexVM> GetPublisherById(int id)
        {
            try
            {
                var publisher = await _bookManagmentDbContext.Publishers
                    .Where(x => x.Id == id)
                    .Include(x => x.Books)
                    .ProjectTo<PublisherIndexVM>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (publisher == null)
                {
                    throw new KeyNotFoundException($"Could not find publiser with id: {id}");
                }
                return publisher;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        public async Task<bool> PublisherExists(int id)
        {
            return await _bookManagmentDbContext.Publishers.AnyAsync(e => e.Id == id);
        }

        public async Task<Publisher> UpdatePublisher(int id, Publisher publisher)
        {
            if (id != publisher.Id)
            {
                throw new KeyNotFoundException($"Could not find publiser with id: {id}");
            }
            try
            {
                var publishr = _mapper.Map<PublisherEntity>(publisher);
                _bookManagmentDbContext.Publishers.Update(publishr);
                await _bookManagmentDbContext.SaveChangesAsync();
                var editedPublishr = await _bookManagmentDbContext.Publishers
                    .ProjectTo<Publisher>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == id);
                return editedPublishr;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception("Something went wrong! " + ex.ToString());
            }
        }
    }
}
