using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace P3MO.Shared.Models.Data
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [Range(1000, 2100)]
        public int PublicationYear { get; set; }

        [MaxLength(20)]
        public string ISBN { get; set; }

        [MaxLength(500)]
        public string? CoverImageUrl { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Range(1, 10000)]
        public int PageCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign keys
        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int GenreId { get; set; }

        // Navigation properties
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

        [ForeignKey("GenreId")]
        public Genre Genre { get; set; }
    }
}
