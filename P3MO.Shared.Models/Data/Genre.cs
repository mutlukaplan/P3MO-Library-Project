using System.ComponentModel.DataAnnotations;

namespace P3MO.Shared.Models.Data
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        // Navigation property
        public ICollection<Book> Books { get; set; }
    }
}
