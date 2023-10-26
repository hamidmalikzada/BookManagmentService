using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookManagmentService.Infrastructure.Entities
{
    public class Publisher
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Publisher Name")]
        public string PublisherName { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
