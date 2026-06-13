using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookRen.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public ICollection<CartItem> Items { get; set; }
        public User User { get; set; }
    }
}
