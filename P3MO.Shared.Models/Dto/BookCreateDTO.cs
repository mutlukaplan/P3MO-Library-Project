using System.ComponentModel.DataAnnotations;

namespace P3MO.Shared.Models.Dto
{
    public class BookCreateDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [Range(1000, 2100)]
        public int PublicationYear { get; set; }

        public string ISBN { get; set; }

        public string CoverImageUrl { get; set; }

        public string Description { get; set; }

        [Range(1, 10000)]
        public int PageCount { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int GenreId { get; set; }
    }
}
