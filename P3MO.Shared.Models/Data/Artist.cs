

namespace P3MO.Shared.Models.Data
{
   public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Bio { get; set; }
        public List<Album> Albums { get; set; } = new();
    }
}
