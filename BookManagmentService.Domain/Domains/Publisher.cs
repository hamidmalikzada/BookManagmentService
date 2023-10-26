using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookManagmentService.Domain.Domains
{
    public class Publisher
    {
        public int Id { get; set; }
        [Required]
        public string PublisherName { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
