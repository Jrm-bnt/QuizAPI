using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.Models;

namespace QuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly QuizDbContext _context;

        public UserController(QuizDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserLoginModel userLogin)
        {
            if (ModelState.IsValid)
            {
                // Vérifiez si l'e-mail est déjà utilisé
                if (_context.Users.Any(u => u.Email == userLogin.Email))
                {
                    return BadRequest("L'e-mail est déjà utilisé.");
                }

                // Créez un nouvel utilisateur
                var newUser = new Users
                {
                    Email = userLogin.Email,
                    Password = userLogin.Password
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return Ok("Enregistrement réussi.");
            }

            return BadRequest("Les données d'inscription sont invalides.");
        }

    }
}
