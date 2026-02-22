using BookRen.Data;
using BookRen.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        public async Task<IActionResult> AddItem(string? returnUrl)
        {
            return Redirect(returnUrl ?? "/");
        }
    }
}
