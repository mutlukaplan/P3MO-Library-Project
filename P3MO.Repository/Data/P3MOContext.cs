using Microsoft.EntityFrameworkCore;
using P3MO.Shared.Models.Data;

namespace P3MO.Repository.Data
{
    public class P3MOContext: DbContext
    {
        public P3MOContext(DbContextOptions<P3MOContext> options) : base(options) { }

        //public DbSet<Album> Albums { get; set; }

        //public DbSet<Artist> Artists { get; set; } = default!;

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }    


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data for testing
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2024, 1, 1);
            // Seed Authors
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FirstName = "George", LastName = "Orwell", Biography = "English novelist, essayist, and critic.", BirthDate = new DateTime(1903, 6, 25) },
                new Author { Id = 2, FirstName = "J.K.", LastName = "Rowling", Biography = "British author, philanthropist, film producer, and screenwriter.", BirthDate = new DateTime(1965, 7, 31) },
                new Author { Id = 3, FirstName = "Ernest", LastName = "Hemingway", Biography = "American novelist, short-story writer, and journalist.", BirthDate = new DateTime(1899, 7, 21) }
            );

            // Seed Genres
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Fiction", Description = "Literary work based on imagination" },
                new Genre { Id = 2, Name = "Science Fiction", Description = "Fiction based on scientific discoveries or advanced technology" },
                new Genre { Id = 3, Name = "Fantasy", Description = "Fiction involving magic or supernatural elements" },
                new Genre { Id = 4, Name = "Non-fiction", Description = "Based on facts and real events" }
            );

            // Seed Books
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "1984",
                    PublicationYear = 1949,
                    ISBN = "978-0451524935",
                    Description = "A dystopian novel set in a totalitarian regime",
                    PageCount = 328,
                    AuthorId = 1,
                    GenreId = 2,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate,
                    CoverImageUrl = "https://covers.openlibrary.org/b/id/7222246-L.jpg",
                },
                new Book
                {
                    Id = 2,
                    Title = "Harry Potter and the Philosopher's Stone",
                    PublicationYear = 1997,
                    ISBN = "978-0747532743",
                    Description = "First book in the Harry Potter series",
                    PageCount = 223,
                    AuthorId = 2,
                    GenreId = 3,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate,
                    CoverImageUrl = "https://covers.openlibrary.org/b/id/10110415-L.jpg",
                },
                new Book
                {
                    Id = 3,
                    Title = "The Old Man and the Sea",
                    PublicationYear = 1952,
                    ISBN = "978-0684801223",
                    Description = "Story of an aging Cuban fisherman",
                    PageCount = 127,
                    AuthorId = 3,
                    GenreId = 1,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate,
                    CoverImageUrl = "https://covers.openlibrary.org/b/id/8095344-L.jpg",
                }
            );
        }
    }
}
