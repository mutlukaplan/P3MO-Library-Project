using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using P3MO.Repository;
using P3MO.Repository.Data;
using P3MO.Shared.Models.Data;

namespace P3MO.Repository_Tests
{
    public class BooksRepositoryTests
    {
        [Fact]
        public async Task GetBooks_ReturnsAllBooks()
        {
            // Arrange
            var testData = GetTestBooks();

            var mockContext = new Mock<P3MOContext>(new DbContextOptions<P3MOContext>());
            mockContext.Setup(c => c.Books).ReturnsDbSet(testData);

            var repo = new BookRepository(mockContext.Object);

            // Act
            var result = await repo.GetBooks();

            // Assert
            var rslt = Assert.IsType<List<Book>>(result);
            var books = Assert.IsAssignableFrom<List<Book>>(rslt);
            Assert.Equal(3, books.Count());

        }

        [Fact]
        public async Task GetBook_WithValidId_ReturnsBook()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<P3MOContext>()
                .UseInMemoryDatabase(databaseName: "GetBookTest")
                .Options;

            // Create test data
            using (var context = new TestLibraryDbContext(options))
            {
                context.Books.AddRange(GetTestBooks());
                context.SaveChanges();
            }

            // Use a fresh instance of the context for the test
            using (var context = new TestLibraryDbContext(options))
            {
                var repository = new BookRepository(context);
                var validId = 1;

                // Act
                var result = await repository.GetBook(validId);

                // Assert
                var rsl = Assert.IsType<Book>(result);
                var book = Assert.IsType<Book>(rsl);
                Assert.Equal(validId, book.Id);
                Assert.Equal("1984", book.Title);
            }
        }

        // Helper method to create test data
        private List<Book> GetTestBooks()
        {
            var author = new Author
            {
                Id = 1,
                FirstName = "George",
                LastName = "Orwell",
                Biography = "English novelist"
            };

            var genre = new Genre
            {
                Id = 1,
                Name = "Fiction",
                Description = "Fiction books"
            };

            return new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "1984",
                    PublicationYear = 1949,
                    ISBN = "978-0451524935",
                    Description = "A dystopian novel",
                    PageCount = 328,
                    AuthorId = 1,
                    GenreId = 1,
                    Author = author,
                    Genre = genre,
                    CreatedAt = new DateTime(2023, 1, 1),
                    UpdatedAt = new DateTime(2023, 1, 1)
                },
                new Book
                {
                    Id = 2,
                    Title = "Animal Farm",
                    PublicationYear = 1945,
                    ISBN = "978-0451526342",
                    Description = "A fairy story",
                    PageCount = 140,
                    AuthorId = 1,
                    GenreId = 1,
                    Author = author,
                    Genre = genre,
                    CreatedAt = new DateTime(2023, 1, 1),
                    UpdatedAt = new DateTime(2023, 1, 1)
                },
                new Book
                {
                    Id = 3,
                    Title = "Brave New World",
                    PublicationYear = 1932,
                    ISBN = "978-0060850524",
                    Description = "A dystopian novel",
                    PageCount = 288,
                    AuthorId = 1,
                    GenreId = 1,
                    Author = author,
                    Genre = genre,
                    CreatedAt = new DateTime(2023, 1, 1),
                    UpdatedAt = new DateTime(2023, 1, 1)
                }
            };
        }

        private class TestLibraryDbContext : P3MOContext
        {
            public TestLibraryDbContext(DbContextOptions<P3MOContext> options)
                : base(options)
            {
            }

            // Override OnModelCreating to avoid issues with seed data
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Configure relationships only
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

                // Skip seeding data for tests
            }
        }
    }
}
