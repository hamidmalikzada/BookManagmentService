using BookManagmentService.Domain.Domains;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookManagmentService.Domain.ViewModels.AuthorVM;
using static BookManagmentService.Domain.ViewModels.PublisherVM;

namespace BookManagmentService.Domain.ViewModels
{
    public class BookVM
    {
        public class BookAddVM
        {
            public int Id { get; set; }
            [Required]
            public string Title { get; set; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime PublishingDate { get; set; }
            public Categori Categori { get; set; }
            [Required]
            public string PublisherName { get; set; }

        }

        public class BookIndexVM
        {
            public int Id { get; set; }
            [Required]
            public string Title { get; set; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [Display(Name = "Publishing Date")]
            public DateTime PublishingDate { get; set; }
            public Categori Categori { get; set; }
            public string PublisherName { get; set; }
            public string AuthorNames { get; set; }
            public List<AuthorAddVM> Authors { get; set; }
        }


        public class BookPublisherVM
        {
            public int Id { get; set; }
            [Required]
            public string Title { get; set; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime PublishingDate { get; set; }
            public Categori Categori { get; set; }
        }

        public class BookAuthAddVM
        {
            public int Id { get; set; }
            [Required]
            public string Title { get; set; }
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime PublishingDate { get; set; }
            [Required]
            public int CategoryId { get; set; }
            [Required]
            public int PublisherId { get; set; }
            [Required]
            public List<int> AuthorIds { get; set; }
        }
        public enum Categori
        {
            Fantasy,
            Fiction,
            Historical,
            Horror,
            Romance,
            Thriller,
            Education,
        }

    }

}
