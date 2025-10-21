﻿using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Models
{
    public class Movie
    {
        public int Id { get; set; }

        
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

       
        public decimal Price { get; set; }

        public string ImgUrl { get; set; } = "default.jpg";

        public int? Quantity { get; set; }

        public string? TrailerUrl { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int MovieStatus { get; set; }

        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<ActorMovie>? ActorMovies { get; set; }

    }
}
