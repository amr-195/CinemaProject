using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Models
{
    public class Actor
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string FullName => FirstName + " " + LastName;

        public string? Bio { get; set; }

        public string? ProfilePicture { get; set; }

        public string? News { get; set; }

        public ICollection<ActorMovie>? ActorMovies { get; set; }
    }
}
