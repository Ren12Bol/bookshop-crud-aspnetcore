using BookRen.Data;
using Microsoft.EntityFrameworkCore;

namespace BookRen.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookRenContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BookRenContext>>()))
            {
                if (context.Book.Any())
                {
                    return;
                }

                context.Book.AddRange(
                    new Book
                    {
                        Title = "The Hobbit",
                        Author = "J.R.R. Tolkien",
                        Description = "A fantasy novel that follows the unexpected adventure " +
                        "of Bilbo Baggins, a hobbit content with his quiet life at " +
                        "Bag End. His routine is disrupted " +
                        "when the wizard Gandalf visits, accompanied by " +
                        "a group of thirteen dwarves led by Thorin Oakenshield.",
                        Publisher = "George Allen & Unwin",
                        ReleaseDate = DateTime.Parse("1937-9-23"),
                        Genre = "Fantasy"
                    },
                    new Book
                    {
                        Title = "Sputnik Sweetheart",
                        Author = "Haruki Murakami",
                        Description = "A melancholic novel exploring unrequited love, " +
                        "loneliness, and existential longing through a " +
                        "\"tangled triangle\" of relationships.",
                        Publisher = "Kodansha",
                        ReleaseDate = DateTime.Parse("1999-01-01"),
                        Genre = "Contemporary Romance"
                    });
                context.SaveChanges();
            }
        }
    }
}
