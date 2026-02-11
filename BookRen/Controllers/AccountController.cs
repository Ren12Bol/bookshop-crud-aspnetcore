using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BookRen.Controllers
{
    public class AccountController : Controller
    {

        public List<UserDummy> users = new List<UserDummy>()
        {
            new UserDummy {Email = "Fairuz", Password = "11", Role = "Admin"},
            new UserDummy {Email = "Ren", Password = "12", Role = "User"}
        };

        //GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        //POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string? returnUrl, [Bind("Email,Password")]UserDummy userDummy)
        {
            if (ModelState.IsValid)
            {
                UserDummy? user = users.FirstOrDefault(u =>
                    u.Email == userDummy.Email && u.Password == userDummy.Password);

                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
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

                return Redirect(returnUrl??"/");
            }

            return View(userDummy);
        }

        public class UserDummy
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
            public string? Role { get; set; }
        };
    }
}
