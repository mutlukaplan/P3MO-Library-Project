using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3MO.Shared.Models.Dto
{
    public class BookDTO
    {
        public int Id { get; set; }

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

        // Read-only fields
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // For responses only - not used in updates
        public AuthorDTO Author { get; set; }
        public GenreDTO Genre { get; set; }
    }
}
