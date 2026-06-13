using System.ComponentModel.DataAnnotations;

namespace BookRen.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Book Book { get; set; }
        public int Quantity { get; set; }
        public Cart Cart { get; set; }
    }
}
