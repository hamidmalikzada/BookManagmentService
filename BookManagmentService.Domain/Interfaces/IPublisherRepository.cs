using BookManagmentService.Domain.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookManagmentService.Domain.ViewModels.BookVM;
using static BookManagmentService.Domain.ViewModels.PublisherVM;

namespace BookManagmentService.Domain.Interfaces
{
    public interface IPublisherRepository
    {
        Task<IEnumerable<PublisherIndexVM>> GetAllPublishers();
        Task<PublisherIndexVM> GetPublisherById(int id);
        Task<Publisher> AddPublisher(Publisher publisher);
        Task<Publisher> UpdatePublisher(int id, Publisher publisher);
        void DeletePublisher(int id);
        Task<bool> PublisherExists(int id);
    }
}
