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
        // POST: api/User/register
        /// <summary>
        /// Enregistre un nouvel utilisateur avec l'e-mail et le mot de passe fournis.
        /// </summary>
        /// <param name="userLogin">Les informations d'inscription de l'utilisateur.</param>
        /// <returns>Une réponse HTTP indiquant le succès de l'enregistrement.</returns>
        /// <response code="200">L'enregistrement a réussi.</response>
        /// <response code="400">Si les données d'inscription sont invalides ou si l'e-mail est déjà utilisé.</response>
        /// <response code="500">Si une erreur interne au serveur se produit.</response>
        /// <response code="403">Si l'utilisateur n'est pas autorisé à accéder à la ressource.</response>
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
