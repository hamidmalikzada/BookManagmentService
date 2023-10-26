using AutoMapper;
using static BookManagmentService.Domain.ViewModels.PublisherVM;
using PublisherDomain = BookManagmentService.Domain.Domains.Publisher;
using PublisherEntity = BookManagmentService.Infrastructure.Entities.Publisher;
using BookDomain = BookManagmentService.Domain.Domains.Book;
using BookEntity = BookManagmentService.Infrastructure.Entities.Book;
using AuthorDomain = BookManagmentService.Domain.Domains.Author;
using AuthorEntity = BookManagmentService.Infrastructure.Entities.Author;
using BookAuthorDomain = BookManagmentService.Domain.Domains.BookAuthor;
using BookAuthorEntity = BookManagmentService.Infrastructure.Entities.BookAuthor;
using UserEntity = BookManagmentService.Infrastructure.Entities.User;
using UserDomain = BookManagmentService.Domain.Domains.User;
using BookManagmentService.Domain.ViewModels;
using BookManagmentService.Domain.Domains;
using static BookManagmentService.Domain.ViewModels.BookVM;
using static BookManagmentService.Domain.ViewModels.AuthorVM;
using Microsoft.EntityFrameworkCore.Design;
using BookManagmentService.Infrastructure.Entities;
using Category = BookManagmentService.Domain.Domains.Category;
using Book = BookManagmentService.Infrastructure.Entities.Book;

namespace BookManagmentService.Infrastructure.Mapings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PublisherEntity, PublisherDomain>();
            CreateMap<PublisherDomain, PublisherEntity>();

            CreateMap<BookEntity, BookDomain>();
            CreateMap<BookDomain, BookEntity>();
            CreateMap<AuthorEntity, AuthorDomain>()
                .ForMember(dest => dest.BookAuthors, opt => opt.MapFrom(src => src.BookAuthors)).PreserveReferences()
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books)).PreserveReferences();
            CreateMap<AuthorDomain, AuthorEntity>();
            CreateMap<BookAuthorEntity, BookAuthorDomain>().PreserveReferences();
            CreateMap<BookAuthorDomain, BookAuthorEntity>().PreserveReferences();

            CreateMap<PublisherAddVM, PublisherDomain>();
            CreateMap<PublisherEditVM, PublisherDomain>();
            CreateMap<PublisherIndexVM, PublisherDomain>();
            CreateMap<PublisherIndexVM, PublisherEntity>();

            CreateMap<BookAddVM, BookDomain>();
            CreateMap<BookAddVM, BookEntity>();
            CreateMap<BookAddVM, AuthorDomain>();
            CreateMap<BookAddVM, AuthorEntity>();
            CreateMap<BookAddVM, PublisherDomain>();
            CreateMap<BookAddVM, PublisherEntity>();

            CreateMap<BookIndexVM, BookDomain>();
            CreateMap<BookIndexVM, BookEntity>();
            CreateMap<BookIndexVM, AuthorDomain>();
            CreateMap<BookIndexVM, AuthorEntity>();
            CreateMap<BookIndexVM, PublisherDomain>();
            CreateMap<BookIndexVM, PublisherEntity>();

            CreateMap<BookDomain, BookAddVM>();
            CreateMap<BookEntity, BookAddVM>();
            CreateMap<AuthorEntity, BookAddVM>();
            CreateMap<AuthorDomain, BookAddVM>();
            CreateMap<PublisherDomain, BookAddVM>();
            CreateMap<PublisherEntity, BookAddVM>();

            CreateMap<BookDomain, BookIndexVM>();

            CreateMap<BookEntity, BookIndexVM>()
                .ForMember(dest => dest.Categori, opt => opt.MapFrom(src => (Categori)src.Category))
                .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher.PublisherName))
                .ForMember(dest => dest.AuthorNames, opt => opt.MapFrom(src => string.Join(", ", src.Authors.Select(a => a.FirstName + " " + a.LastName))))
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.Authors.Select(a => new AuthorAddVM
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName
                })));

            CreateMap<AuthorEntity, AuthorIndexVM>()
                .ForMember(dest => dest.AuthoredBooks, opt => opt.MapFrom(src => src.Books.Select(b => new BookAddVM
                {
                    Id = b.Id,
                    Title = b.Title,
                    PublishingDate = b.PublishingDate,
                    Categori = (Categori)b.Category,
                    PublisherName = b.Publisher != null ? b.Publisher.PublisherName : null
                })));

            CreateMap<PublisherEntity, PublisherIndexVM>()
                .ForMember(dest => dest.PublishedBooks, opt => opt.MapFrom(src => src.Books.Select(b => new BookPublisherVM
                {
                    Id = b.Id,
                    PublishingDate = b.PublishingDate,
                    Categori = (Categori)b.Category,
                    Title = b.Title

                })));

            CreateMap<BookAuthAddVM, BookEntity>()
                    .ForMember(dest => dest.Category, opt => opt.MapFrom(src => (Category)src.CategoryId));

            CreateMap<BookEntity, BookAuthAddVM>();


            CreateMap<AuthorEntity, BookIndexVM>();
            CreateMap<AuthorDomain, BookIndexVM>();
            CreateMap<PublisherDomain, BookIndexVM>();
            CreateMap<PublisherEntity, BookIndexVM>();


            CreateMap<AuthorAddVM, AuthorDomain>();
            CreateMap<AuthorAddVM, AuthorEntity>();
            CreateMap<BookAddVM, BookAuthorDomain>();
            CreateMap<BookAddVM, BookAuthorEntity>();

            //CreateMap<Book, BookIndexVM>()
            //.ForMember(dest => dest.Categori, opt => opt.MapFrom(src => (Category)src.Category));

            CreateMap<UserEntity, UserDomain>();
            CreateMap<UserDomain, UserEntity>();

        }
    }
}
