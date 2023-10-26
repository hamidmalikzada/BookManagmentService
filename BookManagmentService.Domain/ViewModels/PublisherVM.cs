using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookManagmentService.Domain.ViewModels.BookVM;

namespace BookManagmentService.Domain.ViewModels
{
    public class PublisherVM
    {
        public class PublisherAddVM
        {
            [Required(ErrorMessage = "Title is required!")]
            [Display(Name = "Publisher Name")]
            public string PublisherName { get; set; }
        }

        public class PublisherEditVM
        {
            public int Id { get; set; }
            [Required(ErrorMessage = "Title is required!")]
            [Display(Name = "Publisher Name")]
            public string PublisherName { get; set; }
        }

        public class PublisherIndexVM
        {
            public int Id { get; set; }
            public string PublisherName { get; set; }
            public List<BookPublisherVM> PublishedBooks { get; set; }
        }
    }
}
