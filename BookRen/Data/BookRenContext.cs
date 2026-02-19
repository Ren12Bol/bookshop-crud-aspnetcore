using BookRen.Models;
using Microsoft.EntityFrameworkCore;

namespace BookRen.Data
{
    public class BookRenContext : DbContext
    {
        public BookRenContext(DbContextOptions<BookRenContext> options) 
            : base(options)
        {

        }

        public DbSet<Book> Book { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<Cart> Cart { get; set; }
    }
}
