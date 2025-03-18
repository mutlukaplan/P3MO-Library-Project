
namespace P3MO.Shared.Models.Data
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public DateTime ReleaseDate { get; set; }

        public string Genre { get; set; } = default!;

        // New foreign key for Artist
        public int ArtistId { get; set; }

        // Navigation property
        public Artist Artist { get; set; } = default!;
    }
}

