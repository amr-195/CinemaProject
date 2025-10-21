using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Models
{
    public class Cinema
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? CinemaLogo { get; set; }

        public string? Address { get; set; }

        public ICollection<Movie>? Movies { get; set; }
    }
}
