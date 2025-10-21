using CinemaProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaProject.ViewModels
{
    public class MovieVM
    {
        public Movie Movie { get; set; } = new Movie(); 
        public SelectList? Categories { get; set; }
        public SelectList? Cinemas { get; set; }
    }
}
