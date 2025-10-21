//using CinemaProject.Models;
//using Microsoft.EntityFrameworkCore;

//namespace CinemaProject.Data
//{
//    public class ApplicationDBContext:DbContext

//    {
//        public DbSet<Movie> Movies { get; set; }
//        public DbSet<Actor> Actors { get; set; }
//        public DbSet<Category> Categories { get; set; }
//        public DbSet<Cinema> Cinemas { get; set; }
//        public DbSet<ActorMovie> ActorMovies { get; set; }
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            base.OnConfiguring(optionsBuilder);
//            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=CinemaDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
//        }
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);


//            modelBuilder.Entity<ActorMovie>()
//                .HasKey(am => new { am.ActorId, am.MovieId });


//            modelBuilder.Entity<ActorMovie>()
//                .HasOne(am => am.Actor)
//                .WithMany(a => a.ActorMovies)
//                .HasForeignKey(am => am.ActorId);

//            modelBuilder.Entity<ActorMovie>()
//                .HasOne(am => am.Movie)
//                .WithMany(m => m.ActorMovies)
//                .HasForeignKey(am => am.MovieId);
//        }
//    }
//}
using CinemaProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CinemaProject.Data
{
    public class ApplicationDBContext : DbContext
    {
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
          base.OnConfiguring(optionsBuilder);
          optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=CinemaDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
      }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
           
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<ActorMovie> ActorMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<ActorMovie>()
                .HasKey(am => new { am.ActorId, am.MovieId });

            modelBuilder.Entity<ActorMovie>()
                .HasOne(am => am.Actor)
                .WithMany(a => a.ActorMovies)
                .HasForeignKey(am => am.ActorId);

            modelBuilder.Entity<ActorMovie>()
                .HasOne(am => am.Movie)
                .WithMany(m => m.ActorMovies)
                .HasForeignKey(am => am.MovieId);
        }
    }
}
