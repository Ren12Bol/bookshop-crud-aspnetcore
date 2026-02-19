using BookRen.Data;
using BookRen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

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
            return View();
        }
    }
}
