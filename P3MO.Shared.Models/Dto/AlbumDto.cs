namespace P3MO.Shared.Models.Dto
{
    public class AlbumDto
    {
        public string Title { get; set; } = default!;
        public string Artist { get; set; } = default!;
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; } = default!;
    }
}
