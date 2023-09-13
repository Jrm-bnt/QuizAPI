using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using QuizAPI.Models;

namespace QuizAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly QuizDbContext _context;
        private readonly IConfiguration _config;

        public LoginController(QuizDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        // POST: api/Login/authenticate
        /// <summary>
        /// Authentifie un utilisateur et génère un jeton JWT s'il est valide.
        /// </summary>
        /// <param name="userLogin">Les informations de connexion de l'utilisateur.</param>
        /// <returns>Un jeton JWT en cas d'authentification réussie.</returns>
        /// <response code="200">L'utilisateur a été authentifié avec succès.</response>
        /// <response code="400">Si les données d'authentification sont invalides.</response>
        /// <response code="401">Si l'utilisateur n'est pas autorisé à accéder à la ressource.</response>
        /// <response code="500">Si une erreur interne au serveur se produit.</response>
        /// <response code="403">Si l'utilisateur n'est pas autorisé à accéder à la ressource.</response>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserLoginModel userLogin)
        {
            var user = AuthenticateUser(userLogin.Email, userLogin.Password);

            if (user != null)
            {
                var token = GenerateJwtToken(user);
                user.Token = token;
                _context.SaveChanges(); // Mettez à jour le token dans la base de données
                return Ok(new { Token = token });
            }

            return Unauthorized("Authentification échouée.");
        }

        private User AuthenticateUser(string email, string password)
        {
            var currentUser = _context.Users.FirstOrDefault(u =>
                u.Email.ToLower() == email.ToLower() && u.Password == password);

            if (currentUser != null)
            {
                var user = new User
                {
                    Id = currentUser.Id,
                    Email = currentUser.Email,
                    Token = GenerateJwtToken(currentUser)
                };

                return user;
            }

            return null;
        }

        private string GenerateJwtToken(Users users)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(users.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, users.Email));
            }

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
