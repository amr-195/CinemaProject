using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<Movie>? Movies { get; set; }
    }
}
