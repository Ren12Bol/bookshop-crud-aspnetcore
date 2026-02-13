using BookRen.Data;
using BookRen.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BookRen.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookRenContext _context;

        public AccountController(BookRenContext context)
        {
            _context = context;
        }

        //GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        //POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string? returnUrl, [Bind("Id,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var users = from u in _context.User select u;
                var retrievedUser = users.FirstOrDefault(u => u.Email == user.Email);

                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }

                if (retrievedUser is null)
                {
                    ModelState.AddModelError(string.Empty, "Email not found.");
                    return View();
                }

                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

                if (passwordHasher.VerifyHashedPassword(user, retrievedUser.Password, user.Password) == 0)
                {
                    ModelState.AddModelError(string.Empty, "Invalid password.");
                    return View();
                }
                else
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, retrievedUser.Email),
                        new Claim(ClaimTypes.Name, retrievedUser.Username),
                        new Claim(ClaimTypes.Role, retrievedUser.Role)
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddHours(2)
                        });
                }

                return Redirect(returnUrl??"/");
            }

                return View(user);
        }

        //POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout(string? returnUrl)
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect(returnUrl??"/");
        }

        //GET: /Account/Register
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return BadRequest();
            }

            return View();
        }

        //POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register([Bind("Id,Email,Username,Password")]User user, string password)
        {
            if (user is null || user.Email is null || 
                user.Username is null)
            {
                ModelState.AddModelError(string.Empty, "You did not enter the required values.");
                return View();
            }

            if (ModelState.IsValid)
            {
                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                string hashedPassword = passwordHasher.HashPassword(user, password);
                user.Role = "User";
                user.Password = hashedPassword;

                _context.Add(user);
                await _context.SaveChangesAsync();
            }

            return Redirect("/Account/Login/");
        }
    }
}
