using BookRen.Data;
using BookRen.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookRen.Controllers
{
    public class CartController : Controller
    {
        private readonly BookRenContext _context;

        public CartController(BookRenContext context)
        {
            _context = context;
        }

        public IActionResult UserCart()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = _context.User.FirstOrDefault(x => x.Id == int.Parse(userId));

            var cart = _context.Cart.FirstOrDefault(x => x.User == user);

            var cartItems = _context.CartItem.Where(c => c.Cart == cart)
                .Include(c => c.Book);

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(string? returnUrl, int id)
        {
            var book = _context.Book.FirstOrDefault(x => x.Id == id);

            if (book is null)
            {
                return Problem("That book doesn't exist.");
            }

            if (!User.Identity.IsAuthenticated)
            {
                //session-based logic
            }
            else
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var user = _context.User.FirstOrDefault(x => x.Id == int.Parse(userId));

                var cart = _context.Cart.FirstOrDefault(x => x.User == user);

                if (cart is null)
                {
                    cart = new Cart()
                    {
                        User = user
                    };

                    _context.Cart.Add(cart);
                } 
                
                var cartItem = _context.CartItem.FirstOrDefault(x => x.Book == book);

                if (cartItem is null)
                {
                    var newItem = new CartItem()
                    {
                        Book = book,
                        Quantity = 1,
                        Cart = cart
                    };

                    cart.Items?.Add(newItem);
                    _context.CartItem.Add(newItem);
                }
                else
                {
                    cartItem.Quantity += 1;
                    _context.CartItem.Update(cartItem);
                }

                await _context.SaveChangesAsync();
            }

            return Redirect(returnUrl ?? "/");
        }
    }
}
