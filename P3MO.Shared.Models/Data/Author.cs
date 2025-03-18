using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace P3MO.Shared.Models.Data
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(2000)]
        public string Biography { get; set; }

        public DateTime? BirthDate { get; set; }

        // Navigation property
        public ICollection<Book> Books { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
