namespace P3MO.Shared.Models.Dto
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public DateTime? BirthDate { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
