using System.Security.Claims;
using appReversotask.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appReversotask.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbclinicaContext _context;

        public AccountController(DbclinicaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Cpf == model.Cpf);

            if (paciente == null)
            {
                ModelState.AddModelError("", "CPF não encontrado.");
                return View(model);
            }

            // Cria as claims (informações) do usuário autenticado
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, paciente.Nome),
                new Claim("PacienteId", paciente.Codigo.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Autentica o usuário de fato (necessário para o [Authorize] funcionar)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // Mantém a Session também, caso outras partes do código ainda usem
            HttpContext.Session.SetInt32("PacienteId", paciente.Codigo);
            HttpContext.Session.SetString("PacienteNome", paciente.Nome);

            return RedirectToAction("Index", "Consulta");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}