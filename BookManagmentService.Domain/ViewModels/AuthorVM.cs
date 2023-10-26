using BookManagmentService.Domain.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookManagmentService.Domain.ViewModels.BookVM;

namespace BookManagmentService.Domain.ViewModels
{
    public class AuthorVM
    {
        public class AuthorAddVM
        {
            public int Id { get; set; }
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
        }

        public class AuthorIndexVM
        {
            public int Id { get; set; }
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            public List<BookAddVM> AuthoredBooks { get; set; }
        }

        public class AuthorsInBook 
        { 
            public List<AuthorAddVM> Authors { get; set; }
        }
    }
}
