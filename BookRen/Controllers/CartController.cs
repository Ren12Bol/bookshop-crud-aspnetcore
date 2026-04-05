using BookRen.Data;
using BookRen.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Rules;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;

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
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                var user = _context.User.FirstOrDefault(x => x.Id == int.Parse(userId));

                var cart = _context.Cart.FirstOrDefault(x => x.User == user);

                var cartItems = _context.CartItem.Where(c => c.Cart == cart)
                    .Include(c => c.Book);
                return View(cartItems);
            }
            else
            {
                if (HttpContext.Session.GetString("CartItems") != null)
                {
                    var cartItems = JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("CartItems"));

                    return View(cartItems);
                }
            }

                return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddItem(string? returnUrl, int id)
        {
            var book = _context.Book.FirstOrDefault(x => x.Id == id);

            if (book is null)
            {
                return Problem("That book doesn't exist.");
            }

            else
            {
                if (User.Identity.IsAuthenticated)
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

                    var cartItem = _context.CartItem.FirstOrDefault(x => x.Book == book && x.Cart == cart);

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
                else
                {
                    var items = new List<CartItem>();

                    var item = new CartItem()
                    {
                        Id = new Random().Next(1, int.MaxValue),
                        Book = book,
                        Quantity = 1
                    };

                    if (HttpContext.Session.GetString("CartItems") == null)
                    {
                        items.Add(item);

                        HttpContext.Session.SetString("CartItems", JsonSerializer.Serialize(items));
                    }
                    else
                    {
                        items = JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("CartItems"));

                        bool containsBook = false;

                        foreach (var item1 in items)
                        {
                            if (item1.Book.Id == item.Book.Id)
                            {
                                item1.Quantity += 1;

                                containsBook = true;

                                break;
                            }
                        }

                        if (containsBook == false)
                        {
                            items.Add(item);
                        }

                        HttpContext.Session.SetString("CartItems", JsonSerializer.Serialize(items));
                    }
                }
            }

            return Redirect(returnUrl ?? "/");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(string? returnUrl, int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    removeAnItem(id);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return Problem("Something went wrong!");
                }
            }
            else
            {
                var cartItems = JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("CartItems"));

                foreach (var item in cartItems)
                {
                    if (item.Id == id)
                    {
                        cartItems.Remove(item);

                        break;
                    }
                }

                HttpContext.Session.SetString("CartItems", JsonSerializer.Serialize(cartItems));
            }

                return Redirect(returnUrl ?? "/");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSelectedItems(string? returnUrl, string ids)
        {
            string[] itemIds = ids.Split(',').Skip(1).ToArray();

            var cartItems = new List<CartItem>();

            if (HttpContext.Session.GetString("CartItems") != null)
            {
                cartItems = JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("CartItems"));
            }

            foreach (var id in itemIds)
            {
                if (User.Identity.IsAuthenticated)
                {
                    try
                    {
                        removeAnItem(int.Parse(id));
                    }
                    catch (Exception)
                    {
                        return Problem("Something went wrong!");
                    }
                }
                else
                {
                    cartItems = cartItems.Where(item => item.Id != int.Parse(id)).ToList();

                }
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("CartItems", JsonSerializer.Serialize(cartItems));

            return Redirect(returnUrl ?? "/");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItemQuantity(string? returnUrl, int id, int qntNum)
        {
            if (User.Identity.IsAuthenticated)
            {
                var cartItem = _context.CartItem.FirstOrDefault(x => x.Id == id);

                if (cartItem is not null)
                {
                    cartItem.Quantity = qntNum;
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                var cartItems = JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("CartItems"));

                foreach (var item in cartItems)
                {
                    if (item.Id == id)
                    {
                        item.Quantity = qntNum;
                    }
                }

                HttpContext.Session.SetString("CartItems", JsonSerializer.Serialize(cartItems));
            }

                return Redirect(returnUrl ?? "/");
        }

        public void removeAnItem(int id)
        {
            var cartItem = _context.CartItem.FirstOrDefault(x => x.Id == id);

            if (cartItem is not null)
            {
                _context.CartItem.Remove(cartItem);
            }
        }
    }
}
