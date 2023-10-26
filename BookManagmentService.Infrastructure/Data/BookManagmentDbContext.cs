using BookManagmentService.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagmentService.Infrastructure.Data
{
    public class BookManagmentDbContext : DbContext
    {
        public BookManagmentDbContext(DbContextOptions<BookManagmentDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(b => b.Books)
                .WithMany(a => a.Authors)
                .UsingEntity<BookAuthor>(
                j => j.HasOne(c => c.Book).WithMany(d => d.BookAuthors),
                j => j.HasOne(c => c.Author).WithMany(d => d.BookAuthors));
        }
    }
    public class BookContextFactory : IDesignTimeDbContextFactory<BookManagmentDbContext>
    {
        public BookManagmentDbContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<BookManagmentDbContext>();
            optionBuilder.UseSqlServer(@"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=Book;Integrated Security=SSPI;");

            return new BookManagmentDbContext(optionBuilder.Options);
        }
    }
}

