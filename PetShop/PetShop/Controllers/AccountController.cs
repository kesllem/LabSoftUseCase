
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Models;
using System.Security.Claims;

namespace PetShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbPetShopContext _context;

        public AccountController(DbPetShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "ReservaServico");
            }

            return View("~/Views/Account/Login.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Account/Login.cshtml", model);
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Cpf == model.Cpf);

            if (cliente == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "CPF não encontrado. Faça seu cadastro primeiro."
                );

                return View("~/Views/Account/Login.cshtml", model);
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    cliente.Codigo.ToString()
                ),
                new Claim(ClaimTypes.Name, cliente.Nome),
                new Claim("CPF", cliente.Cpf)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction("Index", "ReservaServico");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction(nameof(Login));
        }
    }
}
