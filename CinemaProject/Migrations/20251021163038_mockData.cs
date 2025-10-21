using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaProject.Migrations
{
    /// <inheritdoc />
    public partial class mockData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Categories
            migrationBuilder.Sql(@"
        INSERT INTO Categories (Name) VALUES
        ('Action'),
        ('Comedy'),
        ('Drama'),
        ('Horror');
    ");

            // Cinemas
            migrationBuilder.Sql(@"
        INSERT INTO Cinemas (Name, Description, CinemaLogo, Address) VALUES
        ('Cinema Cairo', 'Main branch in Cairo', 'cairo.jpg', 'Downtown Cairo'),
        ('Galaxy Mall', 'Modern cinema experience', 'galaxy.jpg', 'Mall of Egypt'),
        ('IMAX October', 'Largest IMAX screen', 'imax.jpg', '6th of October City');
    ");

            // Actors
            migrationBuilder.Sql(@"
        INSERT INTO Actors (FirstName, LastName, Bio, ProfilePicture, News) VALUES
        ('Tom', 'Cruise', 'Action movie star', 'tom.jpg', 'Mission Impossible 8 soon'),
        ('Emma', 'Stone', 'Oscar-winning actress', 'emma.jpg', 'New comedy film coming'),
        ('Leonardo', 'DiCaprio', 'Famous Hollywood actor', 'leo.jpg', 'Upcoming Scorsese project');
    ");

            // Movies (using subqueries to get correct IDs)
            migrationBuilder.Sql(@"
        INSERT INTO Movies (Name, Description, Price, ImgUrl, Quantity, TrailerUrl, StartDate, EndDate, MovieStatus, CinemaId, CategoryId)
        VALUES
        ('Mission Impossible', 'Spy action movie', 120.00, 'mi.jpg', 100, 'https://youtu.be/trailer1', '2025-01-01', '2025-03-01', 1,
            (SELECT TOP 1 Id FROM Cinemas WHERE Name = 'Cinema Cairo'),
            (SELECT TOP 1 Id FROM Categories WHERE Name = 'Action')),

        ('La La Land', 'Musical romantic movie', 100.00, 'lala.jpg', 80, 'https://youtu.be/trailer2', '2025-02-01', '2025-04-01', 1,
            (SELECT TOP 1 Id FROM Cinemas WHERE Name = 'Galaxy Mall'),
            (SELECT TOP 1 Id FROM Categories WHERE Name = 'Drama')),

        ('Inception', 'Mind-bending thriller', 150.00, 'inception.jpg', 50, 'https://youtu.be/trailer3', '2025-03-01', '2025-05-01', 1,
            (SELECT TOP 1 Id FROM Cinemas WHERE Name = 'IMAX October'),
            (SELECT TOP 1 Id FROM Categories WHERE Name = 'Action'));
    ");

            // ActorMovies
            migrationBuilder.Sql(@"
        INSERT INTO ActorMovies (ActorId, MovieId)
        VALUES
        ((SELECT TOP 1 Id FROM Actors WHERE FirstName = 'Tom' AND LastName = 'Cruise'),
         (SELECT TOP 1 Id FROM Movies WHERE Name = 'Mission Impossible')),

        ((SELECT TOP 1 Id FROM Actors WHERE FirstName = 'Emma' AND LastName = 'Stone'),
         (SELECT TOP 1 Id FROM Movies WHERE Name = 'La La Land')),

        ((SELECT TOP 1 Id FROM Actors WHERE FirstName = 'Leonardo' AND LastName = 'DiCaprio'),
         (SELECT TOP 1 Id FROM Movies WHERE Name = 'Inception'));
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DELETE FROM ActorMovies;
        DELETE FROM Movies;
        DELETE FROM Actors;
        DELETE FROM Cinemas;
        DELETE FROM Categories;
    ");
        }


    }
}
